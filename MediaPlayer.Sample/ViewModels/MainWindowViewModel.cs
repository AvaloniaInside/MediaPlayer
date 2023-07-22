using AvaloniaInside.MediaPlayer;
using FFmpeg.AutoGen;

namespace MediaPlayer.Sample.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    public void Test()
    {
#if MACOS
        ffmpeg.RootPath = "/opt/homebrew/Cellar/ffmpeg/6.0/lib/";
#endif
        var player = new AvaloniaInside.MediaPlayer.MediaPlayer
        {
            MediaSource = new MediaSource("/Users/mw/Downloads/Free_Test_Data_1MB_MP3.mp3")
        };
        player.Play();
    }
}