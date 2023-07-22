using System.Drawing;

namespace AvaloniaInside.MediaPlayer;

public interface IMediaSource : IDisposable
{
    bool HasVideo { get; }
    bool HasAudio { get; }
    Size VideoSize { get; }
    bool IsLoaded { get; }
    bool IsBuffering { get; }
    bool IsEndOfMedia { get; }

    TimeSpan MediaLength { get; }
    int AudioSampleRate { get; }
    int AudioChannelCount { get; }

    public void Init();

    event EventHandler<MediaSourceLoadedEventArgs> Loaded;
    event EventHandler<MediaSourceBufferingEventArgs> Buffering;
    event EventHandler<EndOfMediaEventArgs> EndOfMediaReached;

    Packet? NextPocket();
}