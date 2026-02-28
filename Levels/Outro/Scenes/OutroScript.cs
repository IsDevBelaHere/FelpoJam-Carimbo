using Godot;
using System;

public partial class OutroScript : Control
{
	[Export] public Label label;
	public string text;
	public override void _Ready()
	{
		StaticAudioPlayer.instance.PlayCD("res://Models/Audios/fx/tecladoPC.ogg",true);

		text = label.Text;
		label.Text = "";
		WriteIt();
	}
	public async void WriteIt()
	{
		await ToSignal(GetTree().CreateTimer(0.2),"timeout");
		foreach (char character in text)
		{
			await ToSignal(GetTree().CreateTimer(0.03),"timeout");
			label.Text += character;
		}
		StaticAudioPlayer.audioPlayer.Stop();
	}
    public override void _Process(double delta)
    {
		if (Input.IsActionJustPressed("pause"))
		{
			CallDeferred(MethodName.PerformGoToMainMenu);
		}
    }

	public void PerformGoToMainMenu()
    {
        GetTree().ChangeSceneToFile("res://Levels/MainMenu/Scenes/main_menu.tscn");
    }
}
