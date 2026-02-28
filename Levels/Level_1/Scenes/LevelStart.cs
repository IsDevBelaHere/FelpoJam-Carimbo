using Godot;
using System;
public partial class LevelStart : Control
{
	[Export] public AudioStreamOggVorbis levelMusic;
	public static LevelStart instance;
	public double resetTimer;
	public Control resetingControl;
	public AnimatedSprite2D paper;
	public Sprite2D bluePen;
	public Label startLabel;
	public Label endLabel;
	public Configs configFile;
	public override void _Ready()
	{
		if (instance != this)
		{
			resetingControl = GetChild<Control>(0);
			paper = resetingControl.GetChild<AnimatedSprite2D>(0);
			bluePen = resetingControl.GetChild<Sprite2D>(1);
			startLabel = GetChild<Label>(1);
			endLabel = GetChild<Label>(2);

			configFile = GetChild<InGameMenu>(3).configResource;

			if (levelMusic != null)
			{
				StaticAudioPlayer.instance.PlayCD(levelMusic,true);
			}
			instance = this;
		}else
		{
			QueueFree();
		}
		if (((int)LevelGoal.instance.nextLevel - 1) % 2 == 1)
		{
			ProgressManager.Progress.levels[((int)LevelGoal.instance.nextLevel/2) - 1] = 1;
			ProgressManager.Progress.SaveData(ProgressManager.Progress.path);
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

		if (Input.IsActionJustReleased("pause"))
		{
			Control fodase = GetChild<Control>(3);
			fodase.Visible =! fodase.Visible;

			Player.instance.frozen = fodase.Visible;
			if (CarimboManager.instance.newCarimboOverlay != null)
			{
				CarimboManager.instance.freezeOverlayMovement = fodase.Visible;
				CarimboManager.instance.newCarimboOverlay.Visible = fodase.Visible;	
			}
		}

	}
	public void PerformSceneChange(int nextLevel)
    {
        GetTree().ChangeSceneToFile(Globals.LevelsToScene[(Levels)nextLevel]);
    }
}
