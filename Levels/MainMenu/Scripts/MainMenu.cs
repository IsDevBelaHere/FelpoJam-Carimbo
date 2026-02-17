using System;
using Godot;

public partial class MainMenu : MarginContainer
{
	[Export] public PackedScene okCarimbo;
	[Export] public TextureRect carimbo3DTexture;
	[Export] public AnimationPlayer carimboAnimationPlayer;
	public Node2D instantiated_okCarimbo;
	Levels levelSelected;
	TextureRect rectSelected;

	public void ButtomUp_LevelWasSelected(int level, string rectNodePath)
	{
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
		Vector2 placePosition = rectSelected.GlobalPosition + new Vector2(20f,10f);
		instantiated_okCarimbo.GlobalPosition = placePosition;
		instantiated_okCarimbo.Visible = true;
	}
	public void ButtomUp_LevelConfirm()
	{
		if ((int)levelSelected < 1)
		{
			return;
		}

		GetTree().ChangeSceneToFile(Globals.LevelsToScene[levelSelected]);
	}
}
