using Godot;
using System;

public partial class MainMenu : MarginContainer
{
	Levels levelSelected;

	public void ButtomUp_LevelWasSelected(int level)
	{
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
