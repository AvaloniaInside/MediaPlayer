using System.Drawing;

namespace AvaloniaInside.MediaPlayer;

public interface IMediaSource
{
    public bool HasVideo { get; }
    public bool HasAudio { get; }
    public Size VideoSize { get; }
    public void Load(string path);
    public void Load(Uri uri);
    public void Load(Stream stream);
    public bool IsLoaded { get; }
    public bool IsBuffering { get; }
    
    public TimeSpan MediaLength { get; }
    public PlayState PlayState { get; }
    public event EventHandler<MediaSourceLoadedEventArgs> Loaded;
    public event EventHandler<MediaSourceBufferingEventArgs> Buffering;
    public event EventHandler<PlayStateChangedEventArgs> PlayStateChanged;
}