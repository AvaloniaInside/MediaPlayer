using Avalonia.Controls.Primitives;

namespace AvaloniaInside.MediaPlayer;

public class MediaController : TemplatedControl, IMediaController
{
    public IMediaPlayer MediaPlayer { get; set; }
}