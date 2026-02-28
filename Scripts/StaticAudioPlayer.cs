using System.Threading.Tasks;
using Godot;


public partial class StaticAudioPlayer : Node
{
    public static StaticAudioPlayer instance;
    public static AudioStreamPlayer2D audioPlayer;
    public static AudioListener2D audioListener;
    
    public AudioStreamOggVorbis currentStream;
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

        if (stream == currentStream)
        {
            GD.Print('a');
            return;
        }
        if (audioPlayer.Playing)
        {
            Tween tween = GetTree().CreateTween();
            tween.TweenProperty(audioPlayer,"volume_linear",0,0.5f);
            await ToSignal(tween,"finished");
        }
        stream.Loop = looping;
        audioPlayer.Stream = stream;
        audioPlayer.Play();
        audioPlayer.VolumeLinear = 1;
        currentStream = stream;
        
    }
    public async Task<AudioStreamPlayer2D> CreatePlaySFX(AudioStreamOggVorbis stream, bool looping = false)
    {
        stream.Loop = looping;
        AudioStreamPlayer2D sfxAudioPlayer = new()
        {
            Stream = stream,
            Bus = "Effects"
        };
        sfxAudioPlayer.Play();
        ManageSFXLoop(sfxAudioPlayer, looping);
        return sfxAudioPlayer;
    }
    public async void ManageSFXLoop(AudioStreamPlayer2D obj, bool looping)
    {
        if (!looping)
        {
            await ToSignal(obj,"finished");
            obj.QueueFree();
        }
    }
    public async Task<AudioStreamPlayer2D> CreateSFX(string streamPath, bool looping = false)
    {
        AudioStreamPlayer2D sfxAudioPlayer = await CreatePlaySFX(GD.Load<AudioStreamOggVorbis>(streamPath).Duplicate() as AudioStreamOggVorbis, looping);
        return sfxAudioPlayer;
    }
}
