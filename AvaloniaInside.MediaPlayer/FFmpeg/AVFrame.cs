using System.Runtime.InteropServices;

namespace AvaloniaInside.MediaPlayer.FFmpeg;

[StructLayout(LayoutKind.Explicit)]
internal unsafe struct AVFrame
{
    [FieldOffset(0)] internal BytePtrArray8 data;
    [FieldOffset(32)] internal IntArray8 linesize;
    [FieldOffset(64)] internal byte** extended_data;
    [FieldOffset(68)] internal int width;
    [FieldOffset(72)] internal int height;
    [FieldOffset(76)] internal int nb_samples;
}