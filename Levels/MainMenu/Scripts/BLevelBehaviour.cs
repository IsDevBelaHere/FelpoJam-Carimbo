using Godot;
using System;

public partial class BLevelBehaviour : TextureRect
{
	
	[Export] public bool Activated {get; private set;}
	public Label StampedLabel;
    public override void _Ready()
    {
        StampedLabel = GetChild<Label>(0);
    }

	public void SetActive(bool value)
	{
		Activated = value;
		StampedLabel.LabelSettings.FontColor = value ? new Color(1f,1f,1f,1f) : new Color(0f,0f,0f,0f);
	}
}
