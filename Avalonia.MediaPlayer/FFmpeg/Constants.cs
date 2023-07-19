namespace Avalonia.MediaPlayer.FFmpeg;

public static class Constants
{
    public const int AVSEEK_FLAG_ANY = 0x4;

    public const int SWS_FAST_BILINEAR = 0x1;
    public const int SWS_ACCURATE_RND = 0x40000;

    public const int AV_CH_FRONT_LEFT = 0x1;
    public const int AV_CH_FRONT_RIGHT = 0x2;
    public const int AV_CH_FRONT_CENTER = 0x4;

    public const int AV_CH_LAYOUT_MONO = AV_CH_FRONT_CENTER;
    public const int AV_CH_LAYOUT_STEREO = AV_CH_FRONT_LEFT | AV_CH_FRONT_RIGHT;

    public const string LIBAVUTIL = "avutil-56";
    public const string LIBAVFORMAT = "avformat-58";
    public const string LIBAVCODEC = "avcodec-58";
    public const string LIBSWSCALE = "swscale-5";
    public const string LIBSWRESAMPLE = "swresample-3";

    public static readonly long AV_NOPTS_VALUE = unchecked((long)0x8000000000000000L);
}