using System.Runtime.InteropServices;

namespace Avalonia.MediaPlayer.FFmpeg;

[StructLayout(LayoutKind.Explicit)]
internal unsafe struct AVStream
{
    [FieldOffset(8)] internal AVCodecContext* codec;
    [FieldOffset(16)] internal AVRational time_base;
    [FieldOffset(32)] internal long duration;
    [FieldOffset(40)] internal long nb_frames;
    [FieldOffset(68)] internal AVRational avg_frame_rate;
    [FieldOffset(124)] internal AVRational r_frame_rate;
}