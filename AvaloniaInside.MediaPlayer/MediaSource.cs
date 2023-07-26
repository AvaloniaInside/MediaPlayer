using System.Drawing;
using FFmpeg.AutoGen;

namespace AvaloniaInside.MediaPlayer;

public unsafe partial class MediaSource : IMediaSource
{
    private readonly string _mediaPath;

    // CODEC
    private AVCodec* _audioCodec;
    private AVFormatContext* _formatContext;
    private bool _isInitialized;

    // PACKETS
    private AVPacket* _packet;
    private bool _playingToEof;

    public MediaSource(string path)
    {
        _mediaPath = path;
    }

    public bool HasVideo => _videoStreamId != -1;
    public bool HasAudio => _audioStreamId != -1;
    public Size VideoSize { get; }

    public bool IsLoaded { get; }
    public bool IsBuffering { get; }
    public bool IsEndOfMedia { get; }
    public TimeSpan MediaLength { get; private set; }
    public event EventHandler<MediaSourceLoadedEventArgs>? Loaded;
    public event EventHandler<MediaSourceBufferingEventArgs>? Buffering;
    public event EventHandler<EndOfMediaEventArgs>? EndOfMediaReached;

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
                validPacket = ReadAndDecodePacket(packet);
            }
            else
            {
                HandleFailedPacketReading();
            }

            ffmpeg.av_packet_unref(packet);
        }

        ReturnPacket(packet);
        return validPacket;
    }
    
    private Packet? ReadAndDecodePacket(AVPacket* packet)
    {
        if (packet->stream_index == _videoStreamId)
        {
            return DecodeVideo(packet);
        }
        else if (packet->stream_index == _audioStreamId)
        {
            return DecodeAudio(packet);
        }
        else
        {
            Console.WriteLine($"Discarding packet for stream {packet->stream_index}");
            return null;
        }
    }
    private void HandleFailedPacketReading()
    {
        _playingToEof = true;
        EndOfMediaReached?.Invoke(this, new EndOfMediaEventArgs());
    }

    public void Dispose()
    {
        DeallocateResources();
        if (_audioCodec != null)
            ffmpeg.avcodec_flush_buffers(_audioContext);
        FreePacket();
        CloseFormatContext();
        GC.SuppressFinalize(this);
    }

    public void Initialize()
    {
        if (_isInitialized)
            return;

        AVFormatContext* formatContext;
        var ret = ffmpeg.avformat_open_input(&formatContext, _mediaPath, null, null);
        if (ret != 0 || formatContext == null)
        {
            Console.WriteLine("Failed to open file: " + _mediaPath);
            return;
        }

        _formatContext = formatContext;
        if (ffmpeg.avformat_find_stream_info(formatContext, null) < 0)
        {
            Console.WriteLine("Failed to find stream information: " + _mediaPath);
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

            if (codecParameters->codec_type == AVMediaType.AVMEDIA_TYPE_AUDIO && _audioStreamId == -1)
                _audioStreamId = i;

            if (codecParameters->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO && _videoStreamId == -1)
                _videoStreamId = i;
        }

        InitAudio();
        InitVideo();

        if (_formatContext->duration != ffmpeg.AV_NOPTS_VALUE)
            MediaLength = TimeSpan.FromTicks((long)(_formatContext->duration / 1000d * TimeSpan.TicksPerMillisecond));
        if (HasVideo || HasAudio)
        {
            _isInitialized = true;
            return;
        }

        Console.WriteLine("Failed to load audio or video");
        Dispose();
    }

    private AVPacket* GetPacket()
    {
        if (_packet == null) return ffmpeg.av_packet_alloc();

        var packet = _packet;
        _packet = null;
        return packet;
    }

    private void ReturnPacket(AVPacket* packet)
    {
        if (_packet == null)
            _packet = packet;
        else
            ffmpeg.av_packet_free(&packet);
    }

    private void FreePacket()
    {
        if (_packet == null) return;
        fixed (AVPacket** pPacket = &_packet)
        {
            ffmpeg.av_packet_free(pPacket);
        }

        _packet = null;
    }

    private void CloseFormatContext()
    {
        if (_formatContext == null) return;
        fixed (AVFormatContext** pFormatContext = &_formatContext)
        {
            ffmpeg.avformat_close_input(pFormatContext);
        }

        _formatContext = null;
    }
}