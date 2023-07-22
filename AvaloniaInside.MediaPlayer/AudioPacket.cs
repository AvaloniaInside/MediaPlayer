namespace AvaloniaInside.MediaPlayer;

public sealed unsafe class AudioPacket : Packet
{
    public readonly byte[] SampleBuffer;
    public readonly int TotalSampleCount;

    public AudioPacket(byte* sampleBuffer, int sampleCount, int channelCount)
    {
        // I have no idea why the * 2 is necessary, but without it only half of each packet is played
        TotalSampleCount = sampleCount * channelCount * 2;
        SampleBuffer = new byte[TotalSampleCount];

        // copy buffer
        for(int i = 0; i < TotalSampleCount; i++) {
            SampleBuffer[i] = sampleBuffer[i];
        }
    }
}