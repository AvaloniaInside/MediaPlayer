using System.Drawing;

namespace AvaloniaInside.MediaPlayer;

public class MediaSource : IMediaSource
{
    public bool HasVideo { get; }
    public bool HasAudio { get; }
    public Size VideoSize { get; }
    
    public void Load(string path)
    {
        throw new NotImplementedException();
    }

    public void Load(Uri uri)
    {
        throw new NotImplementedException();
    }

    public void Load(Stream stream)
    {
        throw new NotImplementedException();
    }

    public bool IsLoaded { get; private set; }
    public bool IsBuffering { get; private set; }
    public TimeSpan MediaLength { get; }
    public PlayState PlayState { get; private set; }
    public event EventHandler<MediaSourceLoadedEventArgs>? Loaded;
    public event EventHandler<MediaSourceBufferingEventArgs>? Buffering;
    public event EventHandler<PlayStateChangedEventArgs>? PlayStateChanged;
    
    private void SetPlayState(PlayState playState)
    {
        var oldPlayState = PlayState;
        PlayState = playState;
        PlayStateChanged?.Invoke(this, new PlayStateChangedEventArgs(oldPlayState, playState));
    }
}