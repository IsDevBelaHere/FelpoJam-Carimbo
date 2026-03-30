using Godot;
using System;
using System.Threading.Tasks;
public partial class LevelStart : Control
{
	#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
	[Export] public AudioStreamOggVorbis levelMusic;
	public static LevelStart instance;
	public double resetTimer;
	public Control resetingControl;
	public AnimatedSprite2D paper;
	public Sprite2D bluePen;
	public Label startLabel;
	public Label endLabel;
	public Configs configFile;
	public AnimationPlayer folderAnimation;
	public ColorRect blackScreen;
	public override void _Ready()
	{
		if (instance != this)
		{
			resetingControl = GetChild<Control>(0);
			paper = resetingControl.GetChild<AnimatedSprite2D>(0);
			bluePen = resetingControl.GetChild<Sprite2D>(1);
			startLabel = GetChild<Label>(1);
			endLabel = GetChild<Label>(2);
			folderAnimation = GetChild<Sprite2D>(4).GetChild<AnimationPlayer>(0);
			blackScreen = GetChild<ColorRect>(5);

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
		if (IsLevelNewDocument())
		{
			ProgressManager.Progress.levels[((int)LevelGoal.instance.nextLevel/2) - 1] = 1;
			ProgressManager.Progress.SaveData(ProgressManager.Progress.path);
		}
		else
		{
			StaticAudioPlayer.instance.CreatePlaySFX("res://Models/Audios/fx/Papeis/Folhando3.ogg",false);
		}
	}

	public static bool IsLevelNewDocument()
	{
		return ((int)LevelGoal.instance.nextLevel - 1) % 2 == 1;
	}
	public void EndGame()
	{
		endLabel.Visible = true;
	}
	public async Task GoToNextLevel()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(blackScreen,"modulate", new Color(0,0,0,1),0.1f);
		blackScreen.Visible = true;
		tween.Play();
		await ToSignal(tween,"finished");
		StaticAudioPlayer.instance.SetEffectEnabled("Music",0,false);
		CallDeferred(MethodName.PerformSceneChange, (int)LevelGoal.instance.nextLevel);
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
			GoToNextLevel();
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

		if (Input.IsActionJustPressed("pause"))
		{
			Control fodase = GetChild<Control>(3);
			Control configsMenu = fodase.GetChild<Control>(1);
			configsMenu.Visible = false;
			fodase.Visible = !fodase.Visible;

			Player.instance.frozen = fodase.Visible;
			if (CarimboManager.instance.newCarimboOverlay != null)
			{
				CarimboManager.instance.freezeOverlayMovement = fodase.Visible;
				CarimboManager.instance.newCarimboOverlay.Visible = fodase.Visible;	
			}
		}

	}
	public void PullFolder()
	{
		folderAnimation.Play("pull");
	}
	public void PerformSceneChange(int nextLevel)
    {
        GetTree().ChangeSceneToFile(Globals.LevelsToScene[(Levels)nextLevel]);
    }
}
