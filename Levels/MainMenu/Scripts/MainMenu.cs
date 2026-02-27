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

	[Export] public Configs configResource;

	[ExportGroup("ConfigMenuTopics")]
	[Export] Control videoConfigs;
	[Export] Control audioConfigs;
	[Export] Control keyBindsConfigs;
	bool loadingMode = true;
    public override void _Ready()
    {
		if (FileAccess.FileExists("user://configs.tres"))
		{
			configResource = GD.Load<Configs>("user://configs.tres");
			UpdateConfigMenu();
			loadingMode = false;
		}
		else
		{
			for (int i = 0; i < 2; i++)
			{
				ItemSelected_WindowMode(0);
				ValueChanged_BusVolume(0.5f,AudioServer.GetBusName(i));
			}
			UpdateConfigResource();
		}
		
    }
	public void UpdateConfigResource()
	{
		configResource.resolution = ResolutionByIndex(videoConfigs.GetChild<Control>(0).GetChild<OptionButton>(1).Selected);
		configResource.windowMode = GetWindowModeByIndex(videoConfigs.GetChild<Control>(1).GetChild<OptionButton>(1).Selected);
		configResource.masterAudio = (float)audioConfigs.GetChild<Control>(0).GetChild<HSlider>(1).Value;
		configResource.effectsAudio = (float)audioConfigs.GetChild<Control>(1).GetChild<HSlider>(1).Value;
		configResource.musicAudio = (float)audioConfigs.GetChild<Control>(2).GetChild<HSlider>(1).Value;
		configResource.SaveData("user://configs.tres");
		
	}
	public void UpdateConfigMenu()
	{
		videoConfigs.GetChild<Control>(0).GetChild<OptionButton>(1).Select(GetIndexByResolution(configResource.resolution));
		videoConfigs.GetChild<Control>(0).GetChild<OptionButton>(1).EmitSignal("item_selected",GetIndexByResolution(configResource.resolution));

		videoConfigs.GetChild<Control>(1).GetChild<OptionButton>(1).Select(GetIndexByWindowMode(configResource.windowMode));
		videoConfigs.GetChild<Control>(1).GetChild<OptionButton>(1).EmitSignal("item_selected",GetIndexByWindowMode(configResource.windowMode));

		audioConfigs.GetChild<Control>(0).GetChild<HSlider>(1).Value = configResource.masterAudio;
		audioConfigs.GetChild<Control>(0).GetChild<HSlider>(1).EmitSignal("value_changed",configResource.masterAudio);

		audioConfigs.GetChild<Control>(1).GetChild<HSlider>(1).Value = configResource.effectsAudio;
		audioConfigs.GetChild<Control>(1).GetChild<HSlider>(1).EmitSignal("value_changed",configResource.effectsAudio);

		audioConfigs.GetChild<Control>(2).GetChild<HSlider>(1).Value = configResource.musicAudio;
		audioConfigs.GetChild<Control>(2).GetChild<HSlider>(1).EmitSignal("value_changed",configResource.musicAudio);
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
		if (loadingMode) return;
		DisplayServer.WindowSetSize(ResolutionByIndex(index));
		UpdateConfigResource();
	}
	public DisplayServer.WindowMode GetWindowModeByIndex(int index)
	{
		return index switch
		{
			0 => DisplayServer.WindowMode.Windowed,
			1 => DisplayServer.WindowMode.Fullscreen,
			2 => DisplayServer.WindowMode.ExclusiveFullscreen,
			_ => DisplayServer.WindowMode.Windowed
		};
	}
	private int GetIndexByResolution(Vector2I resolution)
	{
		Vector2I _0 = new(2560,1440);
		Vector2I _1 = new(1920,1080);
		Vector2I _2 = new(1280,720);
		Vector2I _3 = new(640,360);
		if (resolution == _0)
		{
			return 0;
		}else if (resolution == _1)
		{
			return 1;
		}else if (resolution == _2)
		{
			return 2;
		}else if (resolution == _3)
		{
			return 3;
		}
		return 0;
	}
	public int GetIndexByWindowMode(DisplayServer.WindowMode mode)
	{
		return mode switch
		{
			DisplayServer.WindowMode.Windowed => 0,
			DisplayServer.WindowMode.Fullscreen => 1,
			DisplayServer.WindowMode.ExclusiveFullscreen => 2,
			_ => 0
		};
	}
	public void ItemSelected_WindowMode(int index)
	{
		
		OptionButton optionButton = videoConfigs.GetChild<Control>(0).GetChild<OptionButton>(1);
		if (index != 0)
		{
			optionButton.SetItemText(3, GetViewport().GetWindow().Size.X + "x" + GetViewport().GetWindow().Size.Y);
			optionButton.Select(3);
			ItemSelected_Resolution(3);
			optionButton.Disabled = true;
		}
		else
		{
			optionButton.Disabled = false;
			optionButton.SetItemText(3, "640x360");
		}

		DisplayServer.WindowSetMode(GetWindowModeByIndex(index));

		if (loadingMode) return;
		UpdateConfigResource();
	}
	public void ValueChanged_BusVolume(float value, string busName)
	{
		int busIndex = AudioServer.GetBusIndex(busName);
		float db = Mathf.LinearToDb(value);

		AudioServer.SetBusVolumeDb(busIndex,db);
		
		if (loadingMode) return;
		UpdateConfigResource();
	}
	public void ButtonUp_Configs()
	{
		if (!canClick)
		{
			return;
		}
		settingMenu.Visible =! settingMenu.Visible;
		levelMenu.Visible = false;
		levelMenu.GetParent<Control>().Visible = levelMenu.Visible || settingMenu.Visible;
		
	}
	public void ButtonUp_VideoMenu()
	{
		videoConfigs.Visible = true;
		audioConfigs.Visible = false;
		
	}
	public void ButtonUp_AudioMenu()
	{
		videoConfigs.Visible = false;
		audioConfigs.Visible = true;
		
	}
	public void ButtonUp_KeybindMenu()
	{
		videoConfigs.Visible = false;
		audioConfigs.Visible = false;
	}

	public void ButtonUp_PlayButton()
	{
		if (!canClick)
		{
			return;
		}
		levelMenu.Visible =! levelMenu.Visible;
		settingMenu.Visible = false;

		levelMenu.GetParent<Control>().Visible = levelMenu.Visible || settingMenu.Visible;

		tableRect.Texture = levelMenu.Visible ? tableTextures[1] : tableTextures[0];

	}

	public void ButtonUp_LevelWasSelected(int level, string rectNodePath)
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
	public async void ButtonUp_LevelConfirm(string buttonNodePath)
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

		tween.TweenProperty(GetParent<Control>(), "modulate", new Color(0,0,0),2.0f);
		tween.TweenProperty(GetParent<Control>().GetChild<AudioStreamPlayer2D>(0), "volume_linear", 0f,2.0f);

		await ToSignal(carimboAnimationPlayer, "animation_finished");

		tween.Play();
		
		await ToSignal(tween,"finished");

		SceneTreeTimer timer = GetTree().CreateTimer(1.0f);

		await ToSignal(timer, "timeout");
		StaticAudioPlayer.instance.PlayCD("res://Models/Audios/Músicas/Impress-Calmo.ogg",true);
		GetTree().ChangeSceneToFile(Globals.LevelsToScene[levelSelected]);
	}
	public void ButtonUp_QuitGame()
	{
		GetTree().Quit();
	}
}
