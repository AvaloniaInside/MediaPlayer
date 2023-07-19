namespace AvaloniaInside.MediaPlayer;

// Interface for the MediaController class. This alow controll ofer the media like play, pause, stop, etc.
public interface IMediaController
{
    public IMediaSource MediaSource { get; }
    public void Play();
    public void Pause();
    public void Stop();
    
    public PlayState PlayState { get; }
    public bool IsPlaying { get; }
    public bool IsPaused { get; }
    public void Seek(TimeSpan position);
    public void SetVolume(double volume);
    public void IncreaseVolume(double volume);
    public void DecreaseVolume(double volume);
    public double Volume { get; set; }
    public void Mute();
    public void Unmute();
    public void ToggleMute();
    public bool IsMuted { get; }
}