namespace Avalonia.MediaPlayer;

public interface IMediaPlayer
{
    public IMediaSource Source { get; }
    public IMediaController Controller { get; }
    
    public void SetSource(IMediaSource source);
    public void SetController(IMediaController controller);
}