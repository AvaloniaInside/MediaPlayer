using System.Drawing;
using System.Runtime.InteropServices;
using AvaloniaInside.MediaPlayer.FFmpeg;
using static AvaloniaInside.MediaPlayer.FFmpeg.AVInterop;

namespace AvaloniaInside.MediaPlayer;

public unsafe class MediaSource : IMediaSource, IDisposable
{
    private const int MaxAudioSamples = 192000;
    private AVCodec* _audioCodec;
    private AVCodecContext* _audioContext;
    private byte* _audioPcmBuffer;
    private AudioPlayback? _audioPlayback;
    private AVFrame* _audioRawBuffer;
    private int _audioStreamId = -1;
    private SwrContext* _audioSwContext;

    private AVFormatContext* _formatContext;
    private int _videoStreamId = -1;
    private SwsContext* _videoSwContext;
    private Thread _decodeThread;
    private bool _runDecodeThread;
    private bool _playingToEof;
    public int AudioSampleRate => HasAudio ? _audioContext->sample_rate : -1;

    public int AudioChannelCount { get; private set; } = -1;

    public void Dispose()
    {
        //throw new NotImplementedException();
    }

    public bool HasVideo => _videoStreamId != -1;
    public bool HasAudio => _audioStreamId != -1;
    public Size VideoSize { get; }
    public void Load(string path)
    {
        AVFormatContext* formatContext;
        int ret = avformat_open_input(&formatContext, path, null, null);
        if (ret != 0 || formatContext == null)
        {
            Console.WriteLine("Failed to open file: " + path);
            return;
        }

        _formatContext = formatContext;
        if (avformat_find_stream_info(formatContext, null) < 0)
        {
            Console.WriteLine("Failed to find stream information: " + path);
            return;
        }

        for (var i = 0; i < formatContext->nb_streams; i++)
        {
            AVStream* stream = formatContext->streams[i];

            Console.WriteLine("Stream Details:");
            Console.WriteLine($"Index: {stream->index}");
            Console.WriteLine($"Codec Type: {stream->codec->codec_type}");
            Console.WriteLine($"Codec ID: {stream->codec->codec_id}");
            Console.WriteLine($"Duration: {stream->duration}");
            Console.WriteLine($"Bit Rate: {stream->codec->bit_rate}");
            Console.WriteLine($"Inspecting stream {i} out of {formatContext->nb_streams}");
            
            if (formatContext->streams[i] == null)
            {
                Console.WriteLine("Stream is null!");
                continue;
            }
            if (formatContext->streams[i]->codec == null)
            {
                Console.WriteLine("Codec is null!");
                continue;
            }

            switch (formatContext->streams[i]->codec->codec_type)
            {
                case AVMediaType.AVMEDIA_TYPE_VIDEO:
                    if (_videoStreamId == -1) _videoStreamId = i;
                    break;
                case AVMediaType.AVMEDIA_TYPE_AUDIO:
                    if (_audioStreamId == -1) _audioStreamId = i;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        InitAudio();

        //InitVideo();
        if (_formatContext->duration != AVConstant.AV_NOPTS_VALUE)
            MediaLength = TimeSpan.FromTicks((long)(_formatContext->duration / 1000d * TimeSpan.TicksPerMillisecond));
        if (HasVideo || HasAudio)
        {
            StartDecodeThread();
        }
        else
        {
            Console.WriteLine("Failed to load audio or video");
            Dispose();
        }
    }

    public void Load(Uri uri)
    {
        throw new NotImplementedException();
    }

    public void Load(Stream stream)
    {
        throw new NotImplementedException();
    }

    public bool IsLoaded { get; }
    public bool IsBuffering { get; }
    public bool IsEndOfMedia { get; }
    public TimeSpan MediaLength { get; private set; }
    public PlayState PlayState { get; private set; }
    public event EventHandler<MediaSourceLoadedEventArgs>? Loaded;
    public event EventHandler<MediaSourceBufferingEventArgs>? Buffering;
    public event EventHandler<PlayStateChangedEventArgs>? PlayStateChanged;
    public event EventHandler<EndOfMediaEventArgs>? EndOfMedia;

    private void SetPlayState(PlayState playState)
    {
        var oldPlayState = PlayState;
        PlayState = playState;
        PlayStateChanged?.Invoke(this, new PlayStateChangedEventArgs(oldPlayState, playState));
    }

    private void InitAudio()
    {
        _audioContext = _formatContext->streams[_audioStreamId]->codec;
        if (_audioContext == null)
        {
            Console.WriteLine("Failed to get audio codec context");
            _audioStreamId = -1;
            return;
        }

        _audioCodec = avcodec_find_decoder(_audioContext->codec_id);
        if (_audioCodec == null)
        {
            Console.WriteLine("Failed to find audio codec");
            _audioStreamId = -1;
            return;
        }

        if (avcodec_open2(_audioContext, _audioCodec, null) != 0)
        {
            Console.WriteLine("Failed to load audio codec");
            _audioStreamId = -1;
            return;
        }

        _audioRawBuffer = av_frame_alloc();
        if (_audioRawBuffer == null)
        {
            Console.WriteLine("Failed to allocate audio buffer");
            _audioStreamId = -1;
            return;
        }

        var audioPcmBuffer = _audioPcmBuffer;
        if (av_samples_alloc(&audioPcmBuffer, null, _audioContext->channels,
                av_samples_get_buffer_size(null, _audioContext->channels, MaxAudioSamples,
                    AVSampleFormat.AV_SAMPLE_FMT_S16, 0), AVSampleFormat.AV_SAMPLE_FMT_S16, 0) < 0)
        {
            Console.WriteLine("Failed to create audio samples buffer");
            _audioStreamId = -1;
            return;
        }

        _audioPcmBuffer = audioPcmBuffer;

        av_frame_unref(_audioRawBuffer);
        _audioSwContext = swr_alloc();
        if (_videoSwContext == null)
        {
            Console.WriteLine("Failed to create audio resampling context");
            _audioStreamId = -1;
            return;
        }

        var inchanlayout = _audioContext->channel_layout;
        if (inchanlayout == 0) inchanlayout = (ulong)av_get_default_channel_layout(_audioContext->channels);
        var outchanlayout = inchanlayout;
        if (outchanlayout != AVConstant.AV_CH_LAYOUT_MONO) outchanlayout = AVConstant.AV_CH_LAYOUT_STEREO;
        av_opt_set_int(_audioSwContext, "in_channel_layout", (long)inchanlayout, 0);
        av_opt_set_int(_audioSwContext, "out_channel_layout", (long)outchanlayout, 0);
        av_opt_set_int(_audioSwContext, "in_sample_rate", _audioContext->sample_rate, 0);
        av_opt_set_int(_audioSwContext, "out_sample_rate", _audioContext->sample_rate, 0);
        av_opt_set_sample_fmt(_audioSwContext, "in_sample_fmt", _audioContext->sample_fmt, 0);
        av_opt_set_sample_fmt(_audioSwContext, "out_sample_fmt", AVSampleFormat.AV_SAMPLE_FMT_S16, 0);
        swr_init(_audioSwContext);
        AudioChannelCount = av_get_channel_layout_nb_channels(outchanlayout);

        _audioPlayback = new AudioPlayback(this);
    }

    private bool DecodeAudio(AVPacket* packet)
    {
        if (avcodec_send_packet(_audioContext, packet) < 0) return false;
        if (avcodec_receive_frame(_audioContext, _audioRawBuffer) < 0) return false;

        var audioPcmBuffer = _audioPcmBuffer;
        var convertlength = swr_convert(_audioSwContext, &audioPcmBuffer, _audioRawBuffer->nb_samples,
            _audioRawBuffer->extended_data, _audioRawBuffer->nb_samples);

        if (convertlength <= 0) return false;

        // Convert audioPcmBuffer to byte[] if it's not already
        var audioData = ConvertToByteArray(audioPcmBuffer, convertlength); // Assuming you have a function to convert 

        _audioPlayback.Push(audioData); // Directly add the audio data to the BufferedWaveProvider

        return true;
    }

    private byte[] ConvertToByteArray(byte* audioPcmBuffer, int length)
    {
        var audioData = new byte[length];
        Marshal.Copy((IntPtr)audioPcmBuffer, audioData, 0, length);
        return audioData;
    }
    
    private void StartDecodeThread() {
        if(_decodeThread != null)
            return;
        _runDecodeThread = true;
        _decodeThread = new Thread(DecodeThreadRun) { Name = "Video Decode" };
        _decodeThread.Start();
    }

    private void StopDecodeThread() {
        if(_decodeThread == null || !_runDecodeThread)
            return;
        _runDecodeThread = false;
        _decodeThread.Join();
        _decodeThread = null;
    }

    private void DecodeThreadRun() {
        var packet = av_packet_alloc();
        while(_runDecodeThread) {
            while(_runDecodeThread && !_playingToEof) {
                bool validPacket = false;
                while(!validPacket && _runDecodeThread) {
                    av_init_packet(packet);
                    if(av_read_frame(_formatContext, packet) == 0) {
                        if(packet->stream_index == _videoStreamId) {
                            //validPacket = DecodeVideo(packet);
                        }
                        else if(packet->stream_index == _audioStreamId) {
                            validPacket = DecodeAudio(packet);
                        }
                        else Console.WriteLine("Discarding packet for stream " + packet->stream_index);
                    }
                    else {
                        _playingToEof = true;
                        validPacket = true;
                    }
                    av_packet_unref(packet);
                }
            }
            Thread.Sleep(50);
        }
        av_packet_free(&packet);
    }
}