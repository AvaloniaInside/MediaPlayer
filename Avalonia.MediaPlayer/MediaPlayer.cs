namespace Avalonia.MediaPlayer;

public class MediaPlayer : IMediaPlayer
{
    public IMediaSource Source { get; }
    public IMediaController Controller { get; }
    
    public void SetSource(IMediaSource source)
    {
        throw new NotImplementedException();
    }

    public void SetController(IMediaController controller)
    {
        throw new NotImplementedException();
    }
}