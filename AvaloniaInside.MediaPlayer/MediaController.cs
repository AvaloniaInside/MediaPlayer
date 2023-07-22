namespace AvaloniaInside.MediaPlayer;

public class MediaController : IMediaController
{
    public IMediaPlayer? MediaPlayer { get; }

    public void Play()
    {
        throw new NotImplementedException();
    }

    public void Pause()
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }

    public PlayState PlayState => MediaPlayer?.PlayState ?? PlayState.Stopped;

    public bool IsPlaying { get; }
    public bool IsPaused { get; }
    
    public void Seek(TimeSpan position)
    {
        throw new NotImplementedException();
    }

    public void SetVolume(double volume)
    {
        throw new NotImplementedException();
    }

    public void IncreaseVolume(double volume)
    {
        throw new NotImplementedException();
    }

    public void DecreaseVolume(double volume)
    {
        throw new NotImplementedException();
    }

    public double Volume { get; set; }
    public void Mute()
    {
        throw new NotImplementedException();
    }

    public void Unmute()
    {
        throw new NotImplementedException();
    }

    public void ToggleMute()
    {
        throw new NotImplementedException();
    }

    public bool IsMuted { get; }
}