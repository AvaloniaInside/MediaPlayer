namespace Avalonia.MediaPlayer.FFmpeg;

internal unsafe struct BytePtrArray8
{
    private byte* _0;
    private byte* _1;
    private byte* _2;
    private byte* _3;
    private byte* _4;
    private byte* _5;
    private byte* _6;
    private byte* _7;

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
                case 4: return _4;
                case 5: return _5;
                case 6: return _6;
                case 7: return _7;
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
                case 4:
                    _4 = value;
                    break;
                case 5:
                    _5 = value;
                    break;
                case 6:
                    _6 = value;
                    break;
                case 7:
                    _7 = value;
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }

    public static implicit operator byte*[](BytePtrArray8 arr)
    {
        return new[]
        {
            arr._0,
            arr._1,
            arr._2,
            arr._3,
            arr._4,
            arr._5,
            arr._6,
            arr._7
        };
    }
}