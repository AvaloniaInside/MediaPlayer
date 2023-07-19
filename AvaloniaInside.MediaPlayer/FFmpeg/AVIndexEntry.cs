using System.Runtime.InteropServices;

namespace AvaloniaInside.MediaPlayer.FFmpeg;

[StructLayout(LayoutKind.Sequential)]
internal struct AVIndexEntry
{
    public long pos;
    public long timestamp;
    public int flags;
    public int size;
    public int min_distance;
}