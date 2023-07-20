namespace AvaloniaInside.MediaPlayer;

public interface IMediaPlayer
{
    public IMediaSource MediaSource { get; }
    public void Play();
    public void Pause();

    public PlayState PlayState { get; }
    public void Seek(TimeSpan position);
    public TimeSpan PlayingOffset { get; }
}