using Godot;
using System;
using System.Collections.Generic;

public partial class TitleScreen : Control
{
	private Panel[] panels;
	private Control graphicsSelected = null!;

	public override void _Ready()
	{
		Config.Initialise();
		
		panels = new[]
		{
			GetNode<Panel>("%SettingsPanel"),
			GetNode<Panel>("%GamemodePanel"),
			GetNode<Panel>("%TwoPlayerMatchPanel"),
			GetNode<Panel>("%ShopPanel"),
			GetNode<Panel>("%HelpPanel"),
			GetNode<Panel>("%CreditsPanel"),
			GetNode<Panel>("%TwoPlayerMatchPanel")
		};
		graphicsSelected = (Control) GetNode("%GraphicsSelector");
		//graphicsSelectorTween = (Tween) GetNode("GraphicsSelectorTween");

		foreach (var panel in panels)
		{
			panel.Visible = false;
		}

		ApplyConfigSettings();
		UpdateMoneyLabels();
	}

	private void OpenPanel(int index)
	{
		var tween = CreateTween();
		panels[index].Visible = true;

		switch (index)
		{
			case 0 or 1 or 2:
				panels[index].Position = new Vector2(-GetViewportRect().Size.X, 0);
				tween.TweenProperty(panels[index], "position", Vector2.Zero, 1)
					.SetTrans(Tween.TransitionType.Cubic)
					.SetEase(Tween.EaseType.Out);
				break;
			case 3:
				panels[index].Position = new Vector2(0, GetViewportRect().Size.Y);
				tween.TweenProperty(panels[index], "position", Vector2.Zero, 1)
					.SetTrans(Tween.TransitionType.Cubic)
					.SetEase(Tween.EaseType.Out);
				break;
			case 4 or 6:
				panels[index].Scale = Vector2.Zero;
				tween.TweenProperty(panels[index], "scale", Vector2.One, 1)
					.SetTrans(Tween.TransitionType.Back)
					.SetEase(Tween.EaseType.Out);
				break;
			default:
				panels[index].Visible = true;
				break;
		}
		
		tween.Play();
	}

	private void ClosePanel(int index)
	{
		panels[index].Visible = false;
	}

	public void GraphicsSettingPressed(int index)
	{
		if (index == Config.Load<int>("graphics_quality"))
		{
			return;
		}
		
		Config.Save("graphics_quality", index);
		UpdateGraphicsSelector();
	}

	public void OnUsernameTextChanged(string input)
	{
		Config.Save("name", input);
	}

	public void OnSettingsCheckboxChanged(string setting) //Make a dictionary between setting name and corresponding value
	{
		switch (setting)
		{
			case "music":
				Config.Save("music", ((Checkbox) GetNode("%MusicCheckbox").GetChild(0)).IsEnabled);
				break;
			case "sfx":
				Config.Save("sfx", ((Checkbox) GetNode("%SfxCheckbox").GetChild(0)).IsEnabled);
				break;
			case "tutorial":
				Config.Save("tutorial", ((Checkbox) GetNode("%TutorialCheckbox").GetChild(0)).IsEnabled);
				break;
			case "adverts":
				Config.Save("adverts", ((Checkbox) GetNode("%AdvertisementsCheckbox").GetChild(0)).IsEnabled);
				break;
		}
	}
	
	public void UpdateMoneyLabels()
	{
		GetNode<Label>("%MainPanel/MoneyLabel").Text = "£" + $"{Config.Load<int>("money"):#,##0.##}";
		GetNode<Label>("%ShopPanel/MoneyLabel").Text = "£" + $"{Config.Load<int>("money"):#,##0.##}";
	}

	public void ApplyConfigSettings()
	{
		((Checkbox) GetNode("%MusicCheckbox").GetChild(0)).IsEnabled = Config.Load<bool>("music");
		((Checkbox) GetNode("%SfxCheckbox").GetChild(0)).IsEnabled = Config.Load<bool>("sfx");
		((Checkbox) GetNode("%TutorialCheckbox").GetChild(0)).IsEnabled = Config.Load<bool>("tutorial");
		((Checkbox) GetNode("%AdvertisementsCheckbox").GetChild(0)).IsEnabled = Config.Load<bool>("adverts");
		((LineEdit) GetNode("%UsernameEdit")).Text = Config.Load<string>("name");
		UpdateGraphicsSelector();
	}

	private void UpdateGraphicsSelector()
	{
		var tween = CreateTween()
			.SetEase(Tween.EaseType.Out)
			.SetTrans(Tween.TransitionType.Sine);
		tween.TweenProperty(graphicsSelected, "position",
			new Vector2(graphicsSelected.Size.Y,
				GetNode<Control>("SettingsPanel/LeftPanel/Low").Size.Y * Config.Load<int>("graphics_quality") +
				GetNode<Control>("SettingsPanel/LeftPanel/Title").Size.Y), 0.2f);
		tween.Play();
	}
	
	public void TwoPlayerPlayPressed() => GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
	public void OnWindowResize() => CallDeferred(nameof(UpdateGraphicsSelector));
}
