using Godot;

public partial class MainMenu : MarginContainer
{
	[Export] public MarginContainer levelMenu;
	[Export] public MarginContainer configContent;
	[Export] public PackedScene okCarimbo;
	[Export] public TextureRect tableRect;
	[Export] public TextureRect carimbo3DTexture;
	[Export] public AnimationPlayer carimboAnimationPlayer;
	[Export] public Texture2D[] tableTextures;
	[Export] public CharacterBody2D player;
	[Export] public TextureRect overlay;
	[Export] public MarginContainer settingMenu;
	[Export] public AudioStreamPlayer2D monhado;
	public Node2D instantiated_okCarimbo;
	Levels levelSelected;
	TextureRect rectSelected;
	bool canClick = true;
	bool confirmating;


	[ExportGroup("ConfigMenuTopics")]
	[Export] Control videoConfigs;
	[Export] Control audioConfigs;
	[Export] Control keyBindsConfigs;

    public override void _Ready()
    {
        ItemSelected_WindowMode(2);
    }

    

	private Vector2I ResolutionByIndex(int index)
	{
		switch (index)
		{
			case 0: 
				return new(2560,1440);
			case 1:
				return new(1920,1080);
			case 2:
				return new(1280,720);
			case 3:
				return new(640,360);
		}
		return new(1920,1080);
	}
	public void ItemSelected_Resolution(int index)
	{
		DisplayServer.WindowSetSize(ResolutionByIndex(index));
	}
	public void ItemSelected_WindowMode(int index)
	{
		OptionButton optionButton = videoConfigs.GetChild<Control>(0).GetChild<OptionButton>(1);
		switch (index)
		{
			case 0:
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
				optionButton.SetItemText(3,"640x360");
				optionButton.Disabled = false;
				break;
			case 1:
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
				optionButton.SetItemText(3, GetViewport().GetWindow().Size.X + "x" + GetViewport().GetWindow().Size.Y);
				optionButton.Disabled = true;
				break;
			case 2:
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
				optionButton.SetItemText(3, GetViewport().GetWindow().Size.X + "x" + GetViewport().GetWindow().Size.Y);
				optionButton.Disabled = true;
				break;
		}
		optionButton.Select(3);
		ItemSelected_Resolution(3);
	}
	public void ValueChanged_BusVolume(float value, string busName)
	{
		int busIndex = AudioServer.GetBusIndex(busName);
		float db = Mathf.LinearToDb(value);

		AudioServer.SetBusVolumeDb(busIndex,db);
	}
	public void ButtommUp_Configs()
	{
		if (!canClick)
		{
			return;
		}
		settingMenu.Visible =! settingMenu.Visible;
		levelMenu.Visible = false;
		
	}
	public void ButtomUp_PlayButton()
	{
		if (!canClick)
		{
			return;
		}
		levelMenu.Visible =! levelMenu.Visible;
		settingMenu.Visible = false;

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
