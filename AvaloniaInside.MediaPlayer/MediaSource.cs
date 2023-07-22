using System.Drawing;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using FFmpeg.AutoGen;

namespace AvaloniaInside.MediaPlayer;

public unsafe class MediaSource : IMediaSource, IDisposable
{
    private const int MaxAudioSamples = 192000;
    private AVCodec* _audioCodec;
    private AVCodecContext* _audioContext;
    private byte* _audioPcmBuffer;
    private AVFrame* _audioRawBuffer;
    private int _audioStreamId = -1;
    private SwrContext* _audioSwContext;

    private AVFormatContext* _formatContext;
    private bool _playingToEof;
    private int _videoStreamId = -1;
    private SwsContext* _videoSwContext;

    private AVPacket* _packet;

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
        ffmpeg.RootPath = "/opt/homebrew/Cellar/ffmpeg/6.0/lib/";
        AVFormatContext* formatContext;
        var ret = ffmpeg.avformat_open_input(&formatContext, path, null, null);
        if (ret != 0 || formatContext == null)
        {
            Console.WriteLine("Failed to open file: " + path);
            return;
        }

        _formatContext = formatContext;
        if (ffmpeg.avformat_find_stream_info(formatContext, null) < 0)
        {
            Console.WriteLine("Failed to find stream information: " + path);
            return;
        }

        for (var i = 0; i < formatContext->nb_streams; i++)
        {
            var stream = formatContext->streams[i];
            var codecParameters = stream->codecpar;

            if (formatContext->streams[i] == null)
            {
                Console.WriteLine("Stream is null!");
                continue;
            }

            if (codecParameters->codec_type == AVMediaType.AVMEDIA_TYPE_AUDIO)
            {
                if (_audioStreamId == -1) _audioStreamId = i;
            }

            if (codecParameters->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
            {
                if (_videoStreamId == -1) _videoStreamId = i;
            }
        }

        InitAudio();

        //InitVideo();
        if (_formatContext->duration != ffmpeg.AV_NOPTS_VALUE)
            MediaLength = TimeSpan.FromTicks((long)(_formatContext->duration / 1000d * TimeSpan.TicksPerMillisecond));
        if (!HasVideo && !HasAudio)
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
        // Get audio stream
        var audioStream = _formatContext->streams[_audioStreamId];
        var codecParameters = audioStream->codecpar;

        var audioCodec = ffmpeg.avcodec_find_decoder(codecParameters->codec_id);
        string audioCodecName = ffmpeg.avcodec_get_name(codecParameters->codec_id);
        if (audioCodec == null)
        {
            Console.WriteLine("Failed to find audio codec");
            _audioStreamId = -1;
            return;
        }

        var audioContext = ffmpeg.avcodec_alloc_context3(audioCodec);
        if (audioContext == null)
        {
            Console.WriteLine("Failed to allocate audio codec context");
            _audioStreamId = -1;
            return;
        }

        if (ffmpeg.avcodec_parameters_to_context(audioContext, codecParameters) < 0)
        {
            Console.WriteLine("Failed to initialize audio codec context");
            ffmpeg.avcodec_free_context(&audioContext);
            _audioStreamId = -1;
            return;
        }

        if (ffmpeg.avcodec_open2(audioContext, audioCodec, null) != 0)
        {
            Console.WriteLine("Failed to open audio codec");
            ffmpeg.avcodec_free_context(&audioContext);
            _audioStreamId = -1;
            return;
        }

        var audioRawBuffer = ffmpeg.av_frame_alloc();
        if (audioRawBuffer == null)
        {
            Console.WriteLine("Failed to allocate audio buffer");
            ffmpeg.avcodec_close(audioContext);
            ffmpeg.avcodec_free_context(&audioContext);
            _audioStreamId = -1;
            return;
        }

        var audioSwContext = ffmpeg.swr_alloc_set_opts(null,
            ffmpeg.av_get_default_channel_layout(audioContext->channels), AVSampleFormat.AV_SAMPLE_FMT_S16,
            audioContext->sample_rate,
            ffmpeg.av_get_default_channel_layout(audioContext->channels), audioContext->sample_fmt,
            audioContext->sample_rate, 0, null);
        if (audioSwContext == null)
        {
            Console.WriteLine("Failed to create audio resampling context");
            ffmpeg.av_frame_free(&audioRawBuffer);
            ffmpeg.avcodec_close(audioContext);
            ffmpeg.avcodec_free_context(&audioContext);
            _audioStreamId = -1;
            return;
        }

        var audioPcmBuffer = _audioPcmBuffer;
        if (ffmpeg.av_samples_alloc(&audioPcmBuffer, null, audioContext->channels,
                ffmpeg.av_samples_get_buffer_size(null, audioContext->channels, MaxAudioSamples,
                    AVSampleFormat.AV_SAMPLE_FMT_S16, 0), AVSampleFormat.AV_SAMPLE_FMT_S16, 0) < 0)
        {
            Console.WriteLine("Failed to create audio samples buffer");
            _audioStreamId = -1;
            return;
        }

        _audioPcmBuffer = audioPcmBuffer;

        var swContextInit = ffmpeg.swr_init(audioSwContext);
        if (swContextInit != 0)
        {
            Console.WriteLine("Failed to initialize audio resampling context");
            ffmpeg.swr_free(&audioSwContext);
            ffmpeg.av_frame_free(&audioRawBuffer);
            ffmpeg.avcodec_close(audioContext);
            ffmpeg.avcodec_free_context(&audioContext);
            _audioStreamId = -1;
            return;
        }

        _audioRawBuffer = audioRawBuffer;
        _audioSwContext = audioSwContext;
        _audioContext = audioContext;
        AudioChannelCount = 2;
    }

    private AudioPacket? DecodeAudio(AVPacket* packet)
    {
        if (ffmpeg.avcodec_send_packet(_audioContext, packet) < 0) return null;
        if (ffmpeg.avcodec_receive_frame(_audioContext, _audioRawBuffer) < 0) return null;

        var audioPcmBuffer = _audioPcmBuffer;
        var convertlength = ffmpeg.swr_convert(_audioSwContext, &audioPcmBuffer, _audioRawBuffer->nb_samples,
            _audioRawBuffer->extended_data, _audioRawBuffer->nb_samples);

        if (convertlength <= 0) return null;

        // Convert audioPcmBuffer to byte[] if it's not already
        //var audioData = ConvertToByteArray(audioPcmBuffer, convertlength); // Assuming you have a function to convert

        return new AudioPacket(_audioPcmBuffer, convertlength, AudioChannelCount);
    }

    //private byte[] ConvertToByteArray(byte* audioPcmBuffer, int length)
    //{
    //    var audioData = new byte[length];
    //    Marshal.Copy((IntPtr)audioPcmBuffer, audioData, 0, length);
    //    return audioData;
    //}

    private AVPacket* GetPacket()
    {
        if (_packet == null)
        {
            return ffmpeg.av_packet_alloc();
        }

        var packet = _packet;
        _packet = null;
        return packet;
    }

    private void ReturnPacket(AVPacket* packet)
    {
        if (_packet == null)
        {
            _packet = packet;
        }
        else
        {
            ffmpeg.av_packet_free(&packet);
        }
    }

    public Packet? NextPocket()
    {
        if (_playingToEof) return null;

        var packet = GetPacket();

        Packet? validPacket = null;
        while (validPacket == null && !_playingToEof)
        {
            ffmpeg.av_init_packet(packet);
            if (ffmpeg.av_read_frame(_formatContext, packet) == 0)
            {
                if (packet->stream_index == _videoStreamId)
                {
                    //validPacket = DecodeVideo(packet);
                }
                else if (packet->stream_index == _audioStreamId)
                {
                    validPacket = DecodeAudio(packet);
                }
                else
                {
                    Console.WriteLine("Discarding packet for stream " + packet->stream_index);
                }
            }
            else
            {
                _playingToEof = true;
            }

            ffmpeg.av_packet_unref(packet);
        }

        ReturnPacket(packet);
        return validPacket;
    }

    static ulong select_channel_layout(AVCodec* codec)
    {
        ulong* p;
        ulong best_ch_layout = 0;
        int best_nb_channels = 0;

        if (codec->channel_layouts == null)
            return ffmpeg.AV_CH_LAYOUT_STEREO;

        p = codec->channel_layouts;
        while (*p != 0)
        {
            int nb_channels = ffmpeg.av_get_channel_layout_nb_channels(*p);

            if (nb_channels > best_nb_channels)
            {
                best_ch_layout = *p;
                best_nb_channels = nb_channels;
            }
            p++;
        }

        return best_ch_layout;
    }
}
