using System.Runtime.InteropServices;

namespace AvaloniaInside.MediaPlayer.FFmpeg;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct AVCodecContext
{
    public IntPtr av_class;
    public int log_level_offset;
    public AVMediaType codec_type;
    public IntPtr codec;
    public AVCodecID codec_id;
    public uint codec_tag;
    public IntPtr priv_data;
    public IntPtr internal_; // Use "internal_" to avoid conflict with C# keyword
    public IntPtr opaque;
    public long bit_rate;
    public int bit_rate_tolerance;
    public int global_quality;
    public int compression_level;
    public int flags;
    public int flags2;
    public IntPtr extradata;
    public int extradata_size;
    public AVRational time_base;
    public int ticks_per_frame;
    public int delay;
    public int width;
    public int height;
    public int coded_width;
    public int coded_height;
    public int gop_size;
    public AVPixelFormat pix_fmt;
    public IntPtr draw_horiz_band;
    public IntPtr get_format;
    public int max_b_frames;
    public float b_quant_factor;
    public int b_frame_strategy;
    public float b_quant_offset;
    public int has_b_frames;
    public int mpeg_quant;
    public float i_quant_factor;
    public float i_quant_offset;
    public float lumi_masking;
    public float temporal_cplx_masking;
    public float spatial_cplx_masking;
    public float p_masking;
    public float dark_masking;
    public int slice_count;
    public int[] slice_offset; // or IntPtr if dynamically allocated
    public AVRational sample_aspect_ratio;
    public int me_cmp;
    public int me_sub_cmp;
    public int mb_cmp;
    public int ildct_cmp;
    public int dia_size;
    public int last_predictor_count;
    public int pre_me;
    public int me_pre_cmp;
    public int pre_dia_size;
    public int me_subpel_quality;
    public int me_range;
    public int slice_flags;
    public int mb_decision;
    public ushort[] intra_matrix; // or IntPtr if dynamically allocated
    public ushort[] inter_matrix; // or IntPtr if dynamically allocated
    public int scenechange_threshold;
    public int noise_reduction;
    public int intra_dc_precision;
    public int skip_top;
    public int skip_bottom;
    public int mb_lmin;
    public int mb_lmax;
    public int me_penalty_compensation;
    public int bidir_refine;
    public int brd_scale;
    public int keyint_min;
    public int refs;
    public int chromaoffset;
    public int mv0_threshold;
    public AVColorPrimaries color_primaries;
    public AVColorTransferCharacteristic color_trc;
    public AVColorSpace colorspace;
    public AVColorRange color_range;
    public AVChromaLocation chroma_sample_location;
    public int slices;
    public AVFieldOrder field_order;
    public int sample_rate;
    public int channels;
    public AVSampleFormat sample_fmt;
    public int frame_size;
    public int frame_number;
    public int block_align;
    public int cutoff;
    public ulong channel_layout;
    public ulong request_channel_layout;
    public AVAudioServiceType audio_service_type;
    public AVSampleFormat request_sample_fmt;
    public IntPtr get_buffer2;
    public int refcounted_frames;
    public float qcompress;
    public float qblur;
    public int qmin;
    public int qmax;
    public int max_qdiff;
    public int rc_buffer_size;
    public int rc_override_count;
    public IntPtr rc_override;
    public long rc_max_rate;
    public long rc_min_rate;
    public float rc_max_available_vbv_use;
    public float rc_min_vbv_overflow_use;
    public int rc_initial_buffer_occupancy;
    public int coder_type;
    public int context_model;
    public int frame_skip_threshold;
    public int frame_skip_factor;
    public int frame_skip_exp;
    public int frame_skip_cmp;
    public int trellis;
    public int min_prediction_order;
    public int max_prediction_order;
    public long timecode_frame_start;
    public IntPtr rtp_callback;
    public int rtp_payload_size;
    public int mv_bits;
    public int header_bits;
    public int i_tex_bits;
    public int p_tex_bits;
    public int i_count;
    public int p_count;
    public int skip_count;
    public int misc_bits;
    public int frame_bits;
    public IntPtr stats_out;
    public IntPtr stats_in;
    public int workaround_bugs;
    public int strict_std_compliance;
    public int error_concealment;
    public int debug;
    public int err_recognition;
    public long reordered_opaque;
    public IntPtr hwaccel;
    public IntPtr hwaccel_context;
    public ulong[] error; // or IntPtr if dynamically allocated
    public int dct_algo;
    public int idct_algo;
    public int bits_per_coded_sample;
    public int bits_per_raw_sample;
    public int lowres;
    public IntPtr coded_frame; // or IntPtr if not used
    public int thread_count;
    public int thread_type;
    public int active_thread_type;
    public int thread_safe_callbacks;
    public IntPtr execute;
    public IntPtr execute2;
    public int nsse_weight;
    public int profile;
    public int level;
    public AVDiscard skip_loop_filter;
    public AVDiscard skip_idct;
    public AVDiscard skip_frame;
    public IntPtr subtitle_header; // or IntPtr if not used
    public int subtitle_header_size;
    public ulong vbv_delay;
    public int side_data_only_packets;
    public int initial_padding;
    public AVRational framerate;
    public AVPixelFormat sw_pix_fmt;
    public AVRational pkt_timebase;
    public IntPtr codec_descriptor;
    public long pts_correction_num_faulty_pts;
    public long pts_correction_num_faulty_dts;
    public long pts_correction_last_pts;
    public long pts_correction_last_dts;
    public IntPtr sub_charenc; // or IntPtr if not used
    public int sub_charenc_mode;
    public int skip_alpha;
    public int seek_preroll;
    public int debug_mv;
    public ushort[] chroma_intra_matrix; // or IntPtr if dynamically allocated
    public IntPtr dump_separator; // or IntPtr if not used
    public IntPtr codec_whitelist; // or IntPtr if not used
    public uint properties;
    public IntPtr coded_side_data; // or IntPtr if not used
    public int nb_coded_side_data;
    public IntPtr hw_frames_ctx; // or IntPtr if not used
    public int sub_text_format;
    public int trailing_padding;
    public long max_pixels;
    public IntPtr hw_device_ctx; // or IntPtr if not used
    public int hwaccel_flags;
    public int apply_cropping;
    public int extra_hw_frames;

}

internal enum AVColorPrimaries
{
    AVCOL_PRI_RESERVED0,
    AVCOL_PRI_BT709,
    AVCOL_PRI_UNSPECIFIED,
    AVCOL_PRI_RESERVED,
    AVCOL_PRI_BT470M,
    AVCOL_PRI_BT470BG,
    AVCOL_PRI_SMPTE170M,
    AVCOL_PRI_SMPTE240M,
    AVCOL_PRI_FILM,
    AVCOL_PRI_BT2020,
    AVCOL_PRI_SMPTE428,
    AVCOL_PRI_SMPTEST428_1,
    AVCOL_PRI_SMPTE431,
    AVCOL_PRI_SMPTE432,
    AVCOL_PRI_JEDEC_P22,
    AVCOL_PRI_NB
}

internal enum AVColorTransferCharacteristic
{
    AVCOL_TRC_RESERVED0,
    AVCOL_TRC_BT709,
    AVCOL_TRC_UNSPECIFIED,
    AVCOL_TRC_RESERVED,
    AVCOL_TRC_GAMMA22,
    AVCOL_TRC_GAMMA28,
    AVCOL_TRC_SMPTE170M,
    AVCOL_TRC_SMPTE240M,
    AVCOL_TRC_LINEAR,
    AVCOL_TRC_LOG,
    AVCOL_TRC_LOG_SQRT,
    AVCOL_TRC_IEC61966_2_4,
    AVCOL_TRC_BT1361_ECG,
    AVCOL_TRC_IEC61966_2_1,
    AVCOL_TRC_BT2020_10,
    AVCOL_TRC_BT2020_12,
    AVCOL_TRC_SMPTE2084,
    AVCOL_TRC_SMPTE428,
    AVCOL_TRC_SMPTEST428_1,
    AVCOL_TRC_ARIB_STD_B67,
    AVCOL_TRC_NB
}

internal enum AVColorSpace
{
    AVCOL_SPC_RGB,
    AVCOL_SPC_BT709,
    AVCOL_SPC_UNSPECIFIED,
    AVCOL_SPC_RESERVED,
    AVCOL_SPC_FCC,
    AVCOL_SPC_BT470BG,
    AVCOL_SPC_SMPTE170M,
    AVCOL_SPC_SMPTE240M,
    AVCOL_SPC_YCOCG,
    AVCOL_SPC_BT2020_NCL,
    AVCOL_SPC_BT2020_CL,
    AVCOL_SPC_SMPTE2085,
    AVCOL_SPC_CHROMA_DERIVED_NCL,
    AVCOL_SPC_CHROMA_DERIVED_CL,
    AVCOL_SPC_ICTCP,
    AVCOL_SPC_NB
}

internal enum AVColorRange
{
    AVCOL_RANGE_UNSPECIFIED,
    AVCOL_RANGE_MPEG,
    AVCOL_RANGE_JPEG,
    AVCOL_RANGE_NB
}

internal enum AVChromaLocation
{
    AVCHROMA_LOC_UNSPECIFIED,
    AVCHROMA_LOC_LEFT,
    AVCHROMA_LOC_CENTER,
    AVCHROMA_LOC_TOPLEFT,
    AVCHROMA_LOC_TOP,
    AVCHROMA_LOC_BOTTOMLEFT,
    AVCHROMA_LOC_BOTTOM,
    AVCHROMA_LOC_NB
}

internal enum AVAudioServiceType
{
    AV_AUDIO_SERVICE_TYPE_MAIN,
    AV_AUDIO_SERVICE_TYPE_EFFECTS,
    AV_AUDIO_SERVICE_TYPE_VISUALLY_IMPAIRED,
    AV_AUDIO_SERVICE_TYPE_HEARING_IMPAIRED,
    AV_AUDIO_SERVICE_TYPE_DIALOGUE,
    AV_AUDIO_SERVICE_TYPE_COMMENTARY,
    AV_AUDIO_SERVICE_TYPE_EMERGENCY,
    AV_AUDIO_SERVICE_TYPE_VOICE_OVER,
    AV_AUDIO_SERVICE_TYPE_KARAOKE,
    AV_AUDIO_SERVICE_TYPE_NB
}
