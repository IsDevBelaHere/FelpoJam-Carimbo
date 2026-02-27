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

		commonButtonSize = buttonControlsLocation.GetChild<Control>(0).GetChild<Control>(1).GetChild<TextureButton>(0).Size;

		for (int i = 0; i < childrenAmt-1; i++)
		{
			Control item = buttonControlsLocation.GetChild<HBoxContainer>(i).GetChild<Control>(1);
			
			
			TextureButton button = item.GetChild<TextureButton>(0);

			
			button.ButtonUp += () => ButtomUp_SelectCarimbo(item.GetMeta("res").As<Carimbo>(),button);
		}
	}
	
	public void ButtomUp_SelectCarimbo(Carimbo carimbo, Control node)
	{
		CarimboManager.instance.CarimboSelect(Carimbo.GetIndexByCarimbo(carimbo.ResourceName));
	}
}