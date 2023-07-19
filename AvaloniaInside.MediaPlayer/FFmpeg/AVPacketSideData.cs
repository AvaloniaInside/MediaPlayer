using System.Runtime.InteropServices;

namespace AvaloniaInside.MediaPlayer.FFmpeg;

[StructLayout(LayoutKind.Explicit)]
internal unsafe struct AVPacketSideData
{
    [FieldOffset(0)] internal byte* data;
    [FieldOffset(8)] internal ulong size;
    [FieldOffset(16)] internal AVPacketSideDataType type;
}