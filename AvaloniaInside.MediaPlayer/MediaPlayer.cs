namespace AvaloniaInside.MediaPlayer;

public class MediaPlayer : IMediaPlayer, IDisposable
{
    private Playback<AudioPacket>? _audioPlayback;
    private CancellationTokenSource? _cancellationTokenSource;
    private DateTime _lastUpdate;
    private IMediaSource? _mediaSource;
    private PlayState _playState = PlayState.Stopped;
    private Playback<VideoPacket>? _videoPlayback;

    public MediaPlayer()
    {
    }

    public MediaPlayer(Playback<AudioPacket> audioPlayback, Playback<VideoPacket> videoPlayback)
    {
        _audioPlayback = audioPlayback;
        _videoPlayback = videoPlayback;
    }

    public void Dispose()
    {
        TerminateMediaSource();
        CancelAndDisposeToken();
        _audioPlayback?.Dispose();
        _videoPlayback?.Dispose();
    }

    public IMediaSource? MediaSource
    {
        get => _mediaSource;
        set
        {
            TerminateMediaSource();
            _mediaSource = value;
            PrepareNewMediaSource();
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
        if (_mediaSource == null || _cancellationTokenSource != null)
            return;

        _mediaSource.Initialize();
        InitializePlayback();
        ChangePlayStateTo(PlayState.Playing);
        _cancellationTokenSource = new CancellationTokenSource();
        Task.Run(() => InternalPlay(_cancellationTokenSource.Token));
    }

    public void Pause()
    {
        CancelAndDisposeToken();
        ChangePlayStateTo(PlayState.Paused);
    }

    public void Seek(TimeSpan position)
    {
        throw new NotImplementedException();
    }

    private void PrepareNewMediaSource()
    {
        if (_mediaSource != null) _mediaSource.EndOfMediaReached += MediaSourceOnEndOfMediaReached;
    }

    private void InitializePlayback()
    {
        BuildPlaybacks();
        _lastUpdate = DateTime.Now;
    }

    private void ChangePlayStateTo(PlayState state)
    {
        PlayState = state;
    }

    private void CancelAndDisposeToken()
    {
        if (_cancellationTokenSource == null)
            return;

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = null;
    }

    private void BuildPlaybacks()
    {
        var builder = new PlaybackBuilder();
        _audioPlayback ??= builder
            .WithAudio()
            .WithMediaSource(_mediaSource)
            .Build() as Playback<AudioPacket>;
        _videoPlayback ??= builder
            .WithVideo()
            .WithMediaSource(_mediaSource)
            .Build() as Playback<VideoPacket>;
    }

    private void MediaSourceOnEndOfMediaReached(object? sender, EndOfMediaEventArgs e)
    {
        Pause();
        ChangePlayStateTo(PlayState.Stopped);
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
                            _audioPlayback?.AddPacket(audioPacket);
                            break;
                        case VideoPacket videoPacket:
                            _videoPlayback?.AddPacket(videoPacket);
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