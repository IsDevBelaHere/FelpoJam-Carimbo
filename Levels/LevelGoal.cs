using System.Threading.Tasks;
using Godot;

public partial class LevelGoal : Area2D
{
    #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

    [Export] public Levels nextLevel;
    [Export] public AnimationPlayer animationPlayer;
    public async Task CallTween()
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(Player.instance,"global_position",new Vector2(GlobalPosition.X,GlobalPosition.Y + 5f) ,0.5);
        tween.Play();
        await ToSignal(tween,"finished");
        GetChild<Button>(8).Visible = true;
        //animationPlayer.Play("stamping");

    }

	public void AreaEnter(Node collider)
    {
        if (collider is Player && !Player.instance.frozen)
        {
            Player.instance.frozen = true;
            CarimboManager.instance.freezeOverlayMovement = true;
            CarimboManager.instance.newCarimboOverlay.Visible = false;
            CallTween();
        }
    }
    public void PutAStamp()
	{
        Player.instance.Visible = false;
		GetChild<TextureRect>(1 ).Visible = true;
		GetChild<AudioStreamPlayer2D>(2).Play();
        GetChild<AnimationPlayer>(7).Stop();
	}
    public void ButtonUp_PlayAnimation()
    {
        GetChild<Button>(8).Visible = false;
        animationPlayer.Play("stamping");
    }
    public void PerformSceneChange()
    {
        GetTree().ChangeSceneToFile(Globals.LevelsToScene[nextLevel]);
    }
}
