using System.Threading.Tasks;
using Godot;

public partial class LevelGoal : Area2D
{
    #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    
    [Export] public Levels nextLevel;
    public async Task CallTween()
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(Player.instance,"scale",new Vector2(Position.X,Position.Y),0.5);
        tween.Play();
    }

	public void AreaEnter(Node collider)
    {
        if (collider is Player && !Player.instance.frozen)
        {
            Player.instance.frozen = true;

            CallTween();

        }
    }
    public void PerformSceneChange()
    {
        GetTree().ChangeSceneToFile(Globals.LevelsToScene[nextLevel]);
    }
}
