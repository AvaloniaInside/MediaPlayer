namespace AvaloniaInside.MediaPlayer.FFmpeg;

internal struct AVStreamInfo
{
    public long last_dts;
    public long duration_gcd;
    public int duration_count;
    public long rfps_duration_sum;
    public long codec_info_duration;
    public long codec_info_duration_fields;
    public int found_decoder;
    public long last_duration;
    public long fps_first_dts;
    public int fps_first_dts_idx;
    public long fps_last_dts;
    public int fps_last_dts_idx;
}