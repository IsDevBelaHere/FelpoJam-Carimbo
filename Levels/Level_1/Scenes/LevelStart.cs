using Godot;
using System;
public partial class LevelStart : Control
{
	public static LevelStart instance;
	public double resetTimer;
	public Control resetingControl;
	public AnimatedSprite2D paper;
	public Sprite2D bluePen;
	public Label startLabel;
	public Label endLabel;
	public override void _Ready()
	{
		if (instance != this)
		{
			resetingControl = GetChild<Control>(0);
			paper = resetingControl.GetChild<AnimatedSprite2D>(0);
			bluePen = resetingControl.GetChild<Sprite2D>(1);
			startLabel = GetChild<Label>(1);
			endLabel = GetChild<Label>(2);
			instance = this;
		}else
		{
			QueueFree();
		}
	}

	public void EndGame()
	{
		endLabel.Visible = true;
	}
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("confirm") && startLabel.Visible)
		{
			GetChild<Label>(1).Visible = false;
			CarimboManager.instance.Start();
			Player.instance.speedMultiplier = Player.instance.sceneSpeedMultiplier;
		}else if (Input.IsActionJustPressed("confirm") && endLabel.Visible)
		{
			CallDeferred(MethodName.PerformSceneChange, (int)LevelGoal.instance.nextLevel);
		}

		if (Input.IsActionPressed("confirm"))
		{
			if (!paper.IsPlaying())
			{
				paper.Play("writing");	
			}
			resetTimer += delta;
			if (resetTimer > 0.5)
			{
				resetingControl.Visible = true;
			}
			if (resetTimer > 1.5)
			{
				GetTree().ReloadCurrentScene();
			}
		}
		if (Input.IsActionJustReleased("confirm"))
		{
			paper.Stop();
			resetingControl.Visible = false;
			resetTimer = 0;
		}

	}
	public void PerformSceneChange(int nextLevel)
    {
        GetTree().ChangeSceneToFile(Globals.LevelsToScene[(Levels)nextLevel]);
    }
}
