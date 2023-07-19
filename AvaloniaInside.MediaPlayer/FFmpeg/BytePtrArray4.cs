namespace AvaloniaInside.MediaPlayer.FFmpeg;

internal unsafe struct BytePtrArray4
{
    private byte* _0;
    private byte* _1;
    private byte* _2;
    private byte* _3;

    internal byte* this[int i]
    {
        get
        {
            switch (i)
            {
                case 0: return _0;
                case 1: return _1;
                case 2: return _2;
                case 3: return _3;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        set
        {
            switch (i)
            {
                case 0:
                    _0 = value;
                    break;
                case 1:
                    _1 = value;
                    break;
                case 2:
                    _2 = value;
                    break;
                case 3:
                    _3 = value;
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}