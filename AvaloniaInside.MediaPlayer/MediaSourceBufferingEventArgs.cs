namespace AvaloniaInside.MediaPlayer;

public sealed class MediaSourceBufferingEventArgs : EventArgs
{
    public double BufferingProgress { get; }
}