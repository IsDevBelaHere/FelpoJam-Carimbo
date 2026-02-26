using Godot;
using System;

public partial class TeamIntro : Control
{
	public void GoToMainMenu()
	{
		CallDeferred(MethodName.ChangeScene);
	}
	public void ChangeScene()
	{
		GetTree().ChangeSceneToFile("res://Levels/MainMenu/Scenes/main_menu.tscn");
	}
}
