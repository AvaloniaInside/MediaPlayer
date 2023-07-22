namespace AvaloniaInside.MediaPlayer;

public class MediaPlayer : IMediaPlayer
{
    private readonly Playback<AudioPacket>? _audioPlayback;
    private readonly Playback<VideoPacket>? _videoPlayback;
    private CancellationTokenSource? _cancellationTokenSource;
    private DateTime _lastUpdate;
    private IMediaSource? _mediaSource;
    private PlayState _playState = PlayState.Stopped;

    public MediaPlayer()
    {
#if MACOS
        _audioPlayback = new AppleAudioPlayback();
#else
        _audioPlayback = new AudioPlayback();
#endif
    }

    public MediaPlayer(Playback<VideoPacket> videoPlayback, Playback<AudioPacket> audioPlayback)
    {
        _videoPlayback = videoPlayback;
        _audioPlayback = audioPlayback;
    }

    public IMediaSource? MediaSource
    {
        get => _mediaSource;
        set
        {
            TerminateMediaSource();
            _mediaSource = value;
            if (_mediaSource != null)
                _mediaSource.EndOfMediaReached += MediaSourceOnEndOfMediaReached;
        }
    }

    public PlayState PlayState
    {
        get => _playState;
        private set
        {
            var oldState = _playState;
            _playState = value;
            PlayStateChanged?.Invoke(this, new PlayStateChangedEventArgs(oldState, value));
        }
    }

    public bool IsEndOfMedia => MediaSource?.IsEndOfMedia ?? false;

    public TimeSpan PlayingOffset { get; private set; }
    public event EventHandler<PlayStateChangedEventArgs>? PlayStateChanged;

    public void Play()
    {
        if (MediaSource == null)
            return;

        if (_cancellationTokenSource != null)
            return;

        MediaSource.Init();
        _audioPlayback?.Init(MediaSource);
        _videoPlayback?.Init(MediaSource);

        _lastUpdate = DateTime.Now;
        PlayState = PlayState.Playing;

        _cancellationTokenSource = new CancellationTokenSource();
        Task.Run(() => InternalPlay(_cancellationTokenSource.Token));

        _videoPlayback?.SourceReloaded();
        _audioPlayback?.SourceReloaded();
    }

    public void Pause()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();

        _cancellationTokenSource = null;
        PlayState = PlayState.Paused;
    }

    public void Seek(TimeSpan position)
    {
        throw new NotImplementedException();
    }

    private void MediaSourceOnEndOfMediaReached(object? sender, EndOfMediaEventArgs e)
    {
        Pause();
        PlayState = PlayState.Stopped;
    }

    private void InternalPlay(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var packet = MediaSource?.NextPocket();
                if (packet != null)
                    switch (packet)
                    {
                        case AudioPacket audioPacket:
                            _audioPlayback?.PushPacket(audioPacket);
                            break;
                        case VideoPacket videoPacket:
                            _videoPlayback?.PushPacket(videoPacket);
                            break;
                    }
            }
            catch (Exception ex)
            {
            }

            Update();
        }
    }

    private void Update()
    {
        var now = DateTime.Now;
        var deltaTime = now - _lastUpdate;
        _lastUpdate = now;
        if (PlayState == PlayState.Playing)
            PlayingOffset += deltaTime;

        // avoid huge jumps
        //if(deltaTime < TimeSpan.FromMilliseconds(100))
        _videoPlayback?.Update(deltaTime);
        _audioPlayback?.Update(deltaTime);
    }

    private void TerminateMediaSource()
    {
        if (_mediaSource == null) return;

        Pause();
        _mediaSource.Dispose();
        _mediaSource = null;

        PlayState = PlayState.Stopped;
    }
}