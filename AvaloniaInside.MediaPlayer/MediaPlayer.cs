using System.Threading;

namespace AvaloniaInside.MediaPlayer;

public class MediaPlayer : IMediaPlayer
{

    private IMediaSource? _mediaSource;
    private DateTime _lastUpdate;
    private CancellationTokenSource? _cancellationTokenSource;
    private readonly Playback<VideoPacket>? _videoPlayback;
    private readonly Playback<AudioPacket>? _audioPlayback;

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
        }
    }

    public PlayState PlayState { get; set; }

    public TimeSpan PlayingOffset { get; set; }

    public void Play()
    {
        if (MediaSource == null)
            return;

        if (_cancellationTokenSource != null)
            return;

        _lastUpdate = DateTime.Now;
        PlayState = PlayState.Playing;

        _cancellationTokenSource = new CancellationTokenSource();
        Task.Run(() => InternalPlayAsync(_cancellationTokenSource.Token));
        //Task.Run(() => InternalRenderFrameAsync(_cancellationTokenSource.Token));

        _videoPlayback?.SourceReloaded();
        _audioPlayback?.SourceReloaded();
    }

    private async Task InternalPlayAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var packet = MediaSource?.NextPocket();
                if (packet != null)
                {
                    switch (packet)
                    {
                        case AudioPacket audioPacket:
                            _audioPlayback.PushPacket(audioPacket);
                            break;
                        case VideoPacket videoPacket:
                            _videoPlayback.PushPacket(videoPacket);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            Update();

            Task.Delay(50, cancellationToken);
        }
    }

    private async Task InternalRenderFrameAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
        }
    }

    public void Update()
    {
        if (PlayingOffset > MediaSource.MediaLength)
        {
            Pause();
            PlayState = PlayState.Stopped;
            //IsEndOfFileReached = true;
        }
        var now = DateTime.Now;
        var deltaTime = now - _lastUpdate;
        _lastUpdate = now;
        if (PlayState == PlayState.Playing) PlayingOffset += deltaTime;

        // avoid huge jumps
        //if(deltaTime < TimeSpan.FromMilliseconds(100))
        _videoPlayback?.Update(deltaTime);
        _audioPlayback?.Update(deltaTime);
    }

    public void Pause()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();

        _cancellationTokenSource = null;
        PlayState = PlayState.Paused;
    }

    public void Seek(TimeSpan position) => throw new NotImplementedException();

    private void TerminateMediaSource()
    {
        if (_mediaSource == null) return;

        Pause();
        _mediaSource.Dispose();
        _mediaSource = null;

        PlayState = PlayState.Stopped;
    }
}
