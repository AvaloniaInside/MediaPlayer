namespace AvaloniaInside.MediaPlayer.FFmpeg;

internal struct IntArray4
{
    internal static readonly int Size = 4;
    private int _0;
    private int _1;
    private int _2;
    private int _3;

    internal int this[int i]
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