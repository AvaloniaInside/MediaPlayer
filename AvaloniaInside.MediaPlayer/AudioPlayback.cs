using NAudio.Wave;

namespace AvaloniaInside.MediaPlayer;

public class AudioPlayback : Playback<AudioPacket>
    {
        private WaveOutEvent outputDevice;
        private BufferedWaveProvider waveProvider;

        protected int _channelCount;
        protected int _sampleRate;

        internal AudioPlayback(MediaSource mediaSource) : base(mediaSource)
        {
            _channelCount = MediaSource.AudioChannelCount; // Assuming DataSource initializes with some default or known values
            _sampleRate = MediaSource.AudioSampleRate;

            // waveProvider = new BufferedWaveProvider(new WaveFormat(_sampleRate, _channelCount));
            // outputDevice = new WaveOutEvent();
            // outputDevice.Init(waveProvider);
        }

        public override void Dispose()
        {
            outputDevice?.Stop();
            outputDevice?.Dispose();
            waveProvider?.ClearBuffer();
            base.Dispose();
        }

        internal override void SourceReloaded()
        {
            if (!MediaSource.HasAudio) return;
            _channelCount = MediaSource.AudioChannelCount;
            _sampleRate = MediaSource.AudioSampleRate;
            StateChanged(MediaSource.PlayState, MediaSource.PlayState);
        }

        internal override void StateChanged(PlayState oldState, PlayState newState)
        {
            switch (newState)
            {
                case PlayState.Playing:
                    outputDevice.Play();
                    break;

                case PlayState.Paused:
                    outputDevice.Pause();
                    break;

                case PlayState.Stopped:
                    outputDevice.Stop();
                    waveProvider.ClearBuffer(); 
                    break;
            }
        }

        internal void Push(byte[] audioData)
        {
            return;
            // Check buffer status and fill as necessary
            int bufferAvailable = waveProvider.BufferLength - waveProvider.BufferedBytes;

            while (PacketQueue.Any() && bufferAvailable > 0) // Assuming PacketQueue gives packets with byte[]
            {
                if (PacketQueue.TryTake(out var packet))
                {
                    waveProvider.AddSamples(audioData, 0, audioData.Length);
                    bufferAvailable -= audioData.Length;
                }
            }
        }
    }