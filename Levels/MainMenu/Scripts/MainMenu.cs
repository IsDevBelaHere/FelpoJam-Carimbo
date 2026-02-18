using System;
using Godot;

public partial class MainMenu : MarginContainer
{
	[Export] public MarginContainer levelMenu;
	[Export] public PackedScene okCarimbo;
	[Export] public TextureRect tableRect;
	[Export] public TextureRect carimbo3DTexture;
	[Export] public AnimationPlayer carimboAnimationPlayer;
	[Export] public Texture2D[] tableTextures;
	[Export] public CharacterBody2D player;
	[Export] public TextureRect overlay;
	[Export] public AudioStreamPlayer2D monhado;
	public Node2D instantiated_okCarimbo;
	Levels levelSelected;
	TextureRect rectSelected;
	bool canClick = true;
	bool confirmating;
	public void ButtomUp_PlayButton()
	{
		if (!canClick)
		{
			return;
		}
		levelMenu.Visible =! levelMenu.Visible;

		tableRect.Texture = levelMenu.Visible ? tableTextures[1] : tableTextures[0];

	}

	public void ButtomUp_LevelWasSelected(int level, string rectNodePath)
	{
		if (!canClick)
		{
			return;
		}
		TextureRect rect = GetNode<TextureRect>(rectNodePath);
		BLevelBehaviour rectScript = rect as BLevelBehaviour;

		if (rectSelected != null)
		{
			BLevelBehaviour antique_rectScript = rectSelected as BLevelBehaviour;
			antique_rectScript.SetActive(false);
			
			instantiated_okCarimbo.QueueFree();
			instantiated_okCarimbo = null;

			levelSelected = Levels.none;
			
		}
		if (rectSelected == rect)
		{
			rectSelected = null;
			return;
		}
		rectSelected = null;
		instantiated_okCarimbo = okCarimbo.Instantiate<Node2D>();
		AddChild(instantiated_okCarimbo);
		instantiated_okCarimbo.Visible = false;
		Vector2 placePosition = rect.GlobalPosition + new Vector2(20f,10f);
		rectScript.SetActive(true);
		rectSelected = rect;

		levelSelected = (Levels)level;
		
		carimbo3DTexture.GlobalPosition = new(placePosition.X - carimbo3DTexture.Size.X/2, placePosition.Y - carimbo3DTexture.Size.Y/2);
		carimboAnimationPlayer.Play("Stamping");
		
	}
	public void PutAStamp()
	{
		monhado.Play();
		if (confirmating)
		{
			player.Visible = false;
			overlay.Visible = true;
			return;
		}
		Vector2 placePosition = rectSelected.GlobalPosition + new Vector2(20f,10f);
		instantiated_okCarimbo.GlobalPosition = placePosition;
		instantiated_okCarimbo.Visible = true;
	}
	public async void ButtomUp_LevelConfirm(string buttonNodePath)
	{
		if ((int)levelSelected < 1)
		{
			return;
		}
		canClick = false;
		TextureButton button = GetNode<TextureButton>(buttonNodePath);
		Vector2 placePosition = button.GlobalPosition + new Vector2(20f,10f);
		Tween tween = GetTree().CreateTween();
		
		confirmating = true;
		carimbo3DTexture.GlobalPosition = new(placePosition.X - carimbo3DTexture.Size.X/2, placePosition.Y - carimbo3DTexture.Size.Y/2);
		carimboAnimationPlayer.Play("Stamping");

		tween.TweenProperty(GetParent<Control>(), "modulate", new Color(0,0,0),4.0f);

		await ToSignal(carimboAnimationPlayer, "animation_finished");

		tween.Play();
		
		await ToSignal(tween,"finished");

		SceneTreeTimer timer = GetTree().CreateTimer(1.0f);

		await ToSignal(timer, "timeout");
		
		GetTree().ChangeSceneToFile(Globals.LevelsToScene[levelSelected]);
	}
}
