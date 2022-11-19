using Godot;
using System;
using System.Collections.Generic;

public partial class TitleScreen : Control
{
	private Panel[] panels;
	//private Tween graphicsSelectorTween;
	private Control graphicsSelected;

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

	private void OpenPanel(int i)
	{
		///<summary> Object, Property being tweened, From, To, Time, Ease type, Tween type </summary>
		switch (i)
		{
			case 0:
				panels[i].Visible = true;

				/*panels[i].GetNode<Tween>("Panel Tween").InterpolateProperty(
					panels[i],
					"rect_position",
					new Vector2(-1024, 0),
					new Vector2(0, 0),
					1,
					Tween.TransitionType.Cubic,
					Tween.EaseType.Out
				);
				panels[i].GetNode<Tween>("Panel Tween").Start();*/
				break;
			case 1:
			case 2:
				panels[i].Visible = true;

				/*panels[i].GetNode<Tween>("Panel Tween").InterpolateProperty(
					panels[i],
					"rect_position",
					new Vector2(-1024, 0),
					new Vector2(0, 0),
					1,
					Tween.TransitionType.Cubic,
					Tween.EaseType.Out
				);
				panels[i].GetNode<Tween>("Panel Tween").Start();*/
				break;
			case 3:
				panels[i].Visible = true;

				/*panels[i].GetNode<Tween>("Panel Tween").InterpolateProperty(
					panels[i],
					"rect_position",
					new Vector2(0, 600),
					new Vector2(0, 0),
					1,
					Tween.TransitionType.Cubic,
					Tween.EaseType.Out
				);
				panels[i].GetNode<Tween>("Panel Tween").Start();*/
				break;
			case 4:
			case 6:
				panels[i].Visible = true;
				/*panels[i].GetNode<Tween>("Panel Tween").InterpolateProperty(
					panels[i],
					"rect_scale",
					new Vector2(0, 0),
					new Vector2(1, 1),
					1,
					Tween.TransitionType.Back,
					Tween.EaseType.Out
				);
				panels[i].GetNode<Tween>("Panel Tween").Start();*/
				break;
			default:
				panels[i].Visible = true;
				break;
		}
	}
	private void ClosePanel(int i) => panels[i].Visible = false;

	public void GraphicsSettingPressed(int i) //should be On - Event
	{
		if (i == (int) Config.Load("graphics_quality")) return;
		Config.Save("graphics_quality", i);
		UpdateGraphicsSelector();
	}

	public void OnUsernameTextChanged(string input) => Config.Save("name", input);

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
		GetNode<Label>("%MainPanel/MoneyLabel").Text = "£" + $"{Config.Load("money"):#,##0.##}";
		GetNode<Label>("%ShopPanel/MoneyLabel").Text = "£" + $"{Config.Load("money"):#,##0.##}";
	}

	public void ApplyConfigSettings()
	{
		((Checkbox) GetNode("%MusicCheckbox").GetChild(0)).IsEnabled = (bool) Config.Load("music");
		((Checkbox) GetNode("%SfxCheckbox").GetChild(0)).IsEnabled = (bool) Config.Load("sfx");
		((Checkbox) GetNode("%TutorialCheckbox").GetChild(0)).IsEnabled = (bool) Config.Load("tutorial");
		((Checkbox) GetNode("%AdvertisementsCheckbox").GetChild(0)).IsEnabled = (bool) Config.Load("adverts");
		((LineEdit) GetNode("%UsernameEdit")).Text = (string) Config.Load("name");
		UpdateGraphicsSelector();
	}

	private void UpdateGraphicsSelector()
	{
		/*graphicsSelectorTween.InterpolateProperty(
			graphicsSelected, //SettingsPanel/LeftPanel/Title
			"rect_position",
			graphicsSelected.RectPosition,
			new Vector2(graphicsSelected.Size.x, GetNode<Control>("SettingsPanel/LeftPanel/Low").Size.y * (int) Config.Load("graphics_quality") + GetNode<Control>("SettingsPanel/LeftPanel/Title").Size.y),
			0.2f,
			Tween.TransitionType.Sine,
			Tween.EaseType.Out
		);
		graphicsSelectorTween.Start();*/
	}
	
	public void TwoPlayerPlayPressed() => GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
	public void OnWindowResize() => CallDeferred(nameof(UpdateGraphicsSelector));
}