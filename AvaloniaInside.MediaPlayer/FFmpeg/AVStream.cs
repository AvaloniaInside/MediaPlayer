using System.Runtime.InteropServices;

namespace AvaloniaInside.MediaPlayer.FFmpeg;

[StructLayout(LayoutKind.Explicit)]
internal unsafe struct AVStream
{
    [FieldOffset(0)] internal int index;
    [FieldOffset(4)] internal int id;
    [FieldOffset(8)] internal AVCodecContext* codec;
    [FieldOffset(16)] internal void* priv_data;
    [FieldOffset(20)] internal AVRational time_base;
    [FieldOffset(28)] internal long start_time;
    [FieldOffset(36)] internal long duration;
    [FieldOffset(44)] internal long nb_frames;
    [FieldOffset(48)] internal int disposition;
    [FieldOffset(52)] internal AVDiscard discard;
    [FieldOffset(56)] internal AVRational sample_aspect_ratio;
    [FieldOffset(64)] internal AVDictionary* metadata;
    [FieldOffset(68)] internal AVRational avg_frame_rate;
    [FieldOffset(76)] internal AVPacket attached_pic;
    [FieldOffset(92)] internal AVPacketSideData* side_data;
    [FieldOffset(96)] internal int nb_side_data;
    [FieldOffset(100)] internal int event_flags;
    [FieldOffset(104)] internal AVStreamInfo info;
    [FieldOffset(156)] internal int pts_wrap_bits;
    [FieldOffset(160)] internal long first_dts;
    [FieldOffset(168)] internal long cur_dts;
    [FieldOffset(176)] internal long last_IP_pts;
    [FieldOffset(184)] internal int last_IP_duration;
    [FieldOffset(188)] internal int probe_packets;
    [FieldOffset(192)] internal int codec_info_nb_frames;
    [FieldOffset(196)] internal AVStreamParseType need_parsing;
    [FieldOffset(200)] internal AVCodecParserContext* parser;
    [FieldOffset(204)] internal AVPacketList* last_in_packet_buffer;
    [FieldOffset(208)] internal AVProbeData probe_data;
    [FieldOffset(240)] internal fixed long pts_buffer[AVConstant.MAX_REORDER_DELAY + 1];
    [FieldOffset(356)] internal AVIndexEntry* index_entries;
    [FieldOffset(360)] internal int nb_index_entries;
    [FieldOffset(364)] internal uint index_entries_allocated_size;
    [FieldOffset(368)] internal AVRational r_frame_rate;
    [FieldOffset(376)] internal int stream_identifier;
    [FieldOffset(380)] internal long interleaver_chunk_size;
    [FieldOffset(388)] internal long interleaver_chunk_duration;
    [FieldOffset(396)] internal int request_probe;
    [FieldOffset(400)] internal int skip_to_keyframe;
    [FieldOffset(404)] internal int skip_samples;
    [FieldOffset(408)] internal long start_skip_samples;
    [FieldOffset(416)] internal long first_discard_sample;
    [FieldOffset(424)] internal long last_discard_sample;
    [FieldOffset(432)] internal int nb_decoded_frames;
    [FieldOffset(436)] internal long mux_ts_offset;
    [FieldOffset(444)] internal long pts_wrap_reference;
    [FieldOffset(452)] internal int pts_wrap_behavior;
    [FieldOffset(456)] internal int update_initial_durations_done;
    [FieldOffset(460)] internal fixed long pts_reorder_error[AVConstant.MAX_REORDER_DELAY + 1];
    [FieldOffset(492)] internal fixed byte pts_reorder_error_count[AVConstant.MAX_REORDER_DELAY + 1];
    [FieldOffset(524)] internal long last_dts_for_order_check;
    [FieldOffset(532)] internal byte dts_ordered;
    [FieldOffset(533)] internal byte dts_misordered;
    [FieldOffset(536)] internal int inject_global_side_data;
    [FieldOffset(540)] internal char* recommended_encoder_configuration;
    [FieldOffset(544)] internal AVRational display_aspect_ratio;
}