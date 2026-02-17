using Godot;
using System;

public partial class MainMenu : MarginContainer
{
	[Export] public PackedScene okCarimbo;
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

		rectScript.SetActive(true);
		rectSelected = rect;

		levelSelected = (Levels)level;

		instantiated_okCarimbo.GlobalPosition = rect.GlobalPosition + new Vector2(20f,10f);
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
