using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Metadata;

namespace AvaloniaInside.MediaPlayer;

public class MediaPlayerElement : ContentControl
{
    public static readonly StyledProperty<IMediaPlayer> MediaPlayerProperty =
        AvaloniaProperty.Register<MediaPlayerElement, IMediaPlayer>(nameof(MediaPlayer));

    public static readonly StyledProperty<IMediaSource> MediaSourceProperty =
        AvaloniaProperty.Register<MediaPlayerElement, IMediaSource>(nameof(MediaSource));

    public IMediaPlayer MediaPlayer
    {
        get => GetValue(MediaPlayerProperty);
        set => SetValue(MediaPlayerProperty, value);
    }

    public IMediaSource MediaSource
    {
        get => GetValue(MediaSourceProperty);
        set => SetValue(MediaSourceProperty, value);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }
}