using System.Runtime.InteropServices;

namespace AvaloniaInside.MediaPlayer.FFmpeg;

[StructLayout(LayoutKind.Explicit)]
internal struct AVCodecContext
{
    [FieldOffset(8)] internal AVMediaType codec_type;
    [FieldOffset(16)] internal AVCodecID codec_id;
    [FieldOffset(76)] internal AVRational time_base;
    [FieldOffset(92)] internal int width;
    [FieldOffset(96)] internal int height;
    [FieldOffset(112)] internal AVPixelFormat pix_fmt;
    [FieldOffset(344)] internal int sample_rate;
    [FieldOffset(348)] internal int channels;
    [FieldOffset(352)] internal AVSampleFormat sample_fmt;
    [FieldOffset(376)] internal ulong channel_layout;
}