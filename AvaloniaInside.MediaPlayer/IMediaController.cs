namespace AvaloniaInside.MediaPlayer;

// Interface for the MediaController class. This allow control over the media like play, pause, stop, etc.
public interface IMediaController
{
    public IMediaPlayer MediaPlayer { get; set; }
}