using Godot;


public partial class StaticAudioPlayer : Node
{
    public static StaticAudioPlayer instance;
    public static AudioStreamPlayer2D audioPlayer;
    public static AudioListener2D audioListener;
    
    public override void _Ready()
    {
        if (instance != this)
        {
            instance = this;
        }
        else
        {
            QueueFree();
        }
        audioPlayer = GetChild<AudioStreamPlayer2D>(0);
        audioListener = GetChild<AudioListener2D>(1);
    }
    
    public void PlayCD(string path, bool looping = false)
    {
        AudioStreamOggVorbis stream = GD.Load(path) as AudioStreamOggVorbis;
        PlayCD(stream,looping);
    }
    public async void PlayCD(AudioStreamOggVorbis stream, bool looping = false)
    { 
        AudioStreamOggVorbis dStream = stream.Duplicate() as AudioStreamOggVorbis;
        dStream.Loop = looping;

        if (dStream.ResourcePath == audioPlayer.Stream.ResourcePath)
        {
            return;
        }
        if (audioPlayer.Playing)
        {
            Tween tween = GetTree().CreateTween();
            tween.TweenProperty(audioPlayer,"volume_linear",0,0.1f);
            await ToSignal(tween,"finished");
        }
        
        audioPlayer.Stream = dStream;
        audioPlayer.Play();
        
    }
}
