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

    public bool IsLoaded { get; }
    public bool IsBuffering { get; }
    public TimeSpan MediaLength { get; }
    public PlayState PlayState { get; private set; }
    public event EventHandler<MediaSourceLoadedEventArgs>? Loaded;
    public event EventHandler<MediaSourceBufferingEventArgs>? Buffering;
}