using System.Runtime.InteropServices;

namespace AvaloniaInside.MediaPlayer.FFmpeg;

[StructLayout(LayoutKind.Explicit)]
internal struct AVPacket
{
    [FieldOffset(32)] internal int stream_index;
}