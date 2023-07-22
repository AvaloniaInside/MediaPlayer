using FFmpeg.AutoGen;

namespace AvaloniaInside.MediaPlayer;

public unsafe partial class MediaSource
{
    private const int MaxAudioSamples = 192000;
    private AVCodecContext* _audioContext;
    private byte* _audioPcmBuffer;
    private AVFrame* _audioRawBuffer;
    private AVStream* _audioStream;
    private int _audioStreamId = -1;
    private SwrContext* _audioSwContext;
    private AVCodecParameters* _codecParameters;

    public int AudioSampleRate => HasAudio ? _audioContext->sample_rate : -1;
    public int AudioChannelCount { get; private set; } = -1;

    private void InitAudio()
    {
        if (!AttemptToInitAudioByAvailableDecoder())
        {
            ResetStateDueToFailedAudioInit();
            return;
        }

        if (!AllocateAudioBuffer())
        {
            ResetStateDueToFailedAudioBufferAllocation();
            return;
        }

        if (!CreateAudioResamplingContext())
        {
            ResetStateDueToFailedResamplingContextCreation();
            return;
        }

        if (!CreateAudioSamplesBuffer())
        {
            ResetStateDueToFailedSamplesBufferCreation();
            return;
        }

        if (!InitAudioResamplingContext())
        {
            ResetStateDueToFailedAudioInit();
            return;
        }

        AudioChannelCount = 2;
    }

    private bool AttemptToInitAudioByAvailableDecoder()
    {
        if (FindAndAllocateAudioDecoder()) return InitializeAudioDecoder();

        return false;
    }

    private bool AllocateAudioBuffer()
    {
        _audioRawBuffer = ffmpeg.av_frame_alloc();
        return _audioRawBuffer != null;
    }

    private void ResetStateDueToFailedAudioBufferAllocation()
    {
        DeallocateResources();
        Console.WriteLine("Failed to allocate audio buffer");
        _audioStreamId = -1;
    }

    private bool CreateAudioResamplingContext()
    {
        _audioSwContext = ffmpeg.swr_alloc_set_opts(null,
            ffmpeg.av_get_default_channel_layout(_audioContext->channels), AVSampleFormat.AV_SAMPLE_FMT_S16,
            _audioContext->sample_rate,
            ffmpeg.av_get_default_channel_layout(_audioContext->channels), _audioContext->sample_fmt,
            _audioContext->sample_rate, 0, null);
        return _audioSwContext != null;
    }

    private void ResetStateDueToFailedResamplingContextCreation()
    {
        DeallocateResources();
        Console.WriteLine("Failed to create audio resampling context");
        _audioStreamId = -1;
    }

    private bool CreateAudioSamplesBuffer()
    {
        var audioPcmBuffer = _audioPcmBuffer;
        var res =  ffmpeg.av_samples_alloc(&audioPcmBuffer, null, _audioContext->channels,
            ffmpeg.av_samples_get_buffer_size(null, _audioContext->channels, MaxAudioSamples,
                AVSampleFormat.AV_SAMPLE_FMT_S16, 0),
            AVSampleFormat.AV_SAMPLE_FMT_S16, 0);
        if (res < 0)
            return false;
        _audioPcmBuffer = audioPcmBuffer;
        return true;
    }

    private void ResetStateDueToFailedSamplesBufferCreation()
    {
        DeallocateResources();
        Console.WriteLine("Failed to create audio samples buffer");
        _audioStreamId = -1;
    }

    private bool InitAudioResamplingContext()
    {
        var swContextInit = ffmpeg.swr_init(_audioSwContext);
        return swContextInit == 0;
    }

    private void ResetStateDueToFailedAudioInit()
    {
        DeallocateResources();
        Console.WriteLine("Failed to initialize audio resampling context");
        _audioStreamId = -1;
    }

    private bool FindAndAllocateAudioDecoder()
    {
        _audioStream = _formatContext->streams[_audioStreamId];
        _codecParameters = _audioStream->codecpar;

        _audioCodec = ffmpeg.avcodec_find_decoder(_codecParameters->codec_id);
        if (_audioCodec == null)
        {
            Console.WriteLine("Failed to find codec");
            _audioStreamId = -1;
            return false;
        }

        _audioContext = ffmpeg.avcodec_alloc_context3(_audioCodec);
        if (_audioContext != null)
            return true;
        Console.WriteLine("Failed to allocate codec context");
        _audioStreamId = -1;
        return false;
    }

    private bool InitializeAudioDecoder()
    {
        if (ffmpeg.avcodec_parameters_to_context(_audioContext, _codecParameters) < 0)
        {
            Console.WriteLine("Failed to initialize codec context");
            FreeAudioContext();
            _audioStreamId = -1;
            return false;
        }

        if (ffmpeg.avcodec_open2(_audioContext, _audioCodec, null) != 0)
        {
            Console.WriteLine("Failed to open codec");
            FreeAudioContext();
            _audioStreamId = -1;
            return false;
        }

        return true;
    }

    private void DeallocateResources()
    {
        FreeAudioSwContext();
        FreeAudioRawBuffer();
        ffmpeg.avcodec_close(_audioContext);
        FreeAudioContext();
    }

    private void FreeAudioContext()
    {
        fixed (AVCodecContext** pAudioContext = &_audioContext)
        {
            ffmpeg.avcodec_free_context(pAudioContext);
        }
    }

    private void FreeAudioRawBuffer()
    {
        fixed (AVFrame** pAudioRawBuffer = &_audioRawBuffer)
        {
            ffmpeg.av_frame_free(pAudioRawBuffer);
        }
    }

    private void FreeAudioSwContext()
    {
        if(_audioSwContext != null)
            fixed (SwrContext** pAudioSwContext = &_audioSwContext)
                ffmpeg.swr_free(pAudioSwContext);
    }

    private AudioPacket? DecodeAudio(AVPacket* packet)
    {
        if (ffmpeg.avcodec_send_packet(_audioContext, packet) < 0) return null;
        if (ffmpeg.avcodec_receive_frame(_audioContext, _audioRawBuffer) < 0) return null;

        var audioPcmBuffer = _audioPcmBuffer;
        var convertedLength = ffmpeg.swr_convert(_audioSwContext, &audioPcmBuffer, _audioRawBuffer->nb_samples,
            _audioRawBuffer->extended_data, _audioRawBuffer->nb_samples);

        return convertedLength <= 0
            ? null
            : new AudioPacket(_audioPcmBuffer, convertedLength, AudioChannelCount);
    }
}