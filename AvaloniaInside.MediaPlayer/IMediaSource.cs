using System.Drawing;

namespace AvaloniaInside.MediaPlayer;

public interface IMediaSource : IDisposable
{
    bool HasVideo { get; }
    bool HasAudio { get; }
    Size VideoSize { get; }
    void Load(string path);
    void Load(Uri uri);
    void Load(Stream stream);
    bool IsLoaded { get; }
    bool IsBuffering { get; }
    bool IsEndOfMedia { get; }

    TimeSpan MediaLength { get; }
    PlayState PlayState { get; }
    int AudioSampleRate { get; }
    int AudioChannelCount { get; }

    event EventHandler<MediaSourceLoadedEventArgs> Loaded;
    event EventHandler<MediaSourceBufferingEventArgs> Buffering;
    event EventHandler<PlayStateChangedEventArgs> PlayStateChanged;
    event EventHandler<EndOfMediaEventArgs> EndOfMedia;

    Packet? NextPocket();
}