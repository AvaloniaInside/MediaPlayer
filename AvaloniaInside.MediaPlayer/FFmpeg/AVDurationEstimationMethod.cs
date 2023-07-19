namespace AvaloniaInside.MediaPlayer.FFmpeg;

public enum AVDurationEstimationMethod
{
    AVFMT_DURATION_FROM_PTS,          // Duration accurately estimated from PTSes
    AVFMT_DURATION_FROM_STREAM,       // Duration estimated from a stream with a known duration
    AVFMT_DURATION_FROM_BITRATE       // Duration estimated from bitrate (less accurate)
}