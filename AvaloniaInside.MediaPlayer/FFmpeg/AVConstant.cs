namespace AvaloniaInside.MediaPlayer.FFmpeg;

public static class AVConstant
{
    public const int AVSEEK_FLAG_ANY = 0x4;
    public const int AV_PARSER_PTS_NB = 4;
    public const int MAX_REORDER_DELAY = 16;

    public const int SWS_FAST_BILINEAR = 0x1;
    public const int SWS_ACCURATE_RND = 0x40000;

    public const int AV_CH_FRONT_LEFT = 0x1;
    public const int AV_CH_FRONT_RIGHT = 0x2;
    public const int AV_CH_FRONT_CENTER = 0x4;

    public const int AV_CH_LAYOUT_MONO = AV_CH_FRONT_CENTER;
    public const int AV_CH_LAYOUT_STEREO = AV_CH_FRONT_LEFT | AV_CH_FRONT_RIGHT;

    public const string LIBAVUTIL = "libavutil.dylib";
    public const string LIBAVFORMAT = "libavformat.dylib";
    public const string LIBAVCODEC = "libavcodec.dylib";
    public const string LIBSWSCALE = "libswscale.dylib";
    public const string LIBSWRESAMPLE = "libswresample.dylib";


    public static readonly long AV_NOPTS_VALUE = unchecked((long)0x8000000000000000L);
}