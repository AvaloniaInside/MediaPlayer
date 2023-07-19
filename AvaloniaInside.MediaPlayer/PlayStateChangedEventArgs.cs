namespace AvaloniaInside.MediaPlayer;

public sealed class PlayStateChangedEventArgs : EventArgs
{
    public PlayState OldPlayState { get; }
    public PlayState NewPlayState { get; }
    
    public PlayStateChangedEventArgs(PlayState oldPlayState, PlayState newPlayState)
    {
        OldPlayState = oldPlayState;
        NewPlayState = newPlayState;
    }
}