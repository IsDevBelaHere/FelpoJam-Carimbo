using Godot;
using System;

public partial class MainMenu : MarginContainer
{
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
			rectSelected = null;

			levelSelected = Levels.none;
			return;
		}

		rectScript.SetActive(true);
		rectSelected = rect;

		levelSelected = (Levels)level;
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
