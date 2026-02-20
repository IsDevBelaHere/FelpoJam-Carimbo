using Godot;
using System;

public partial class CarimbosSlotBar : Control
{
	[Export] Control buttonControlsLocation;
	public float buttonPopOffset = 1;
	public Vector2 buttonSizePopOffset = new Vector2(4f,4f);
	Vector2 commonButtonSize;

	
	public override void _Ready()
	{
		int childrenAmt = buttonControlsLocation.GetChildren().Count;

		commonButtonSize = buttonControlsLocation.GetChild<Control>(0).GetChild<TextureButton>(0).Size;

		for (int i = 0; i < childrenAmt-1; i++)
		{
			Control item = buttonControlsLocation.GetChild<Control>(i);
			
			var lambdaItem = item;

			TextureButton button = lambdaItem.GetChild<TextureButton>(0);

			var lambdaButton = button;
			button.ButtonUp += () => ButtonUp_SelectCarimbo(lambdaItem.GetMeta("res").As<Carimbo>(),lambdaButton);
			GD.Print(lambdaItem.GetMeta("res").As<Carimbo>().ResourceName);
		}
	}
	
	public void ButtonUp_SelectCarimbo(Carimbo carimbo, Control node)
	{
		Input.ActionPress(Carimbo.GetActionByCarimbo(carimbo.ResourceName));
	}
}
