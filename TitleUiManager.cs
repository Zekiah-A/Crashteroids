using Godot;
using System;

public class TitleUiManager : Node
{
	private Panel _mainPanel;
	private Panel _settingsPanel;
	private Panel _gamemodePanel;
	private Panel _matchsettingsPanel;
	private Panel _editorPanel;

	private Tween _settingsTween;
	private Tween _gamemodeTween;
	private Tween _matchsettingsTween;
	private Tween _editorTween;

	public override void _Ready()
	{
		_mainPanel = (Panel) GetParent().GetNode("Main Panel");
		_settingsPanel = (Panel) GetParent().GetNode("Settings Panel");
		_gamemodePanel = (Panel) GetParent().GetNode("Gamemode Panel");
		_matchsettingsPanel = (Panel) GetParent().GetNode("Matchsettings Panel");
		_editorPanel = (Panel) GetParent().GetNode("Editor Panel");
		
		_settingsTween = (Tween) GetParent().GetNode("Settings Panel").GetNode("Panel Tween");
		_gamemodeTween = (Tween) GetParent().GetNode("Gamemode Panel").GetNode("Panel Tween");
		_matchsettingsTween = (Tween) GetParent().GetNode("Matchsettings Panel").GetNode("Panel Tween");
		_editorTween = (Tween) GetParent().GetNode("Editor Panel").GetNode("Panel Tween");
		
		_settingsPanel.Visible = false;
		_gamemodePanel.Visible = false;
		_matchsettingsPanel.Visible = false;
		_editorPanel.Visible = false;
	}
//  public override void _Process(float delta) {}
	
	//TODO: in future use something fancy like tween
	#region SIGNALS
	private void _on_Gamemode_pressed()
	{
		_gamemodePanel.Visible = true;
		
		_gamemodeTween.InterpolateProperty (
			_gamemodePanel, //Object
			"rect_position", //Property being tweened
			new Vector2(1024, 0), //from
			new Vector2(0,0), //to
			1, //speed
			Tween.TransitionType.Cubic,
			Tween.EaseType.Out
		);
		_gamemodeTween.Start();
	}
	
	private void _on_GamemodeOption_pressed(int _index)
	{	//NOTE: use index if i am not doing any fancy transitions with this :(
		GD.Print($"Panel {_index} was pressed.");
		_matchsettingsPanel.Visible = true;

		_matchsettingsTween.InterpolateProperty (
			_matchsettingsPanel, //Object
			"rect_position", //Property being tweened
			new Vector2(1024, 0), //from
			new Vector2(0,0), //to
			1, //speed
			Tween.TransitionType.Cubic,
			Tween.EaseType.Out
		);
		_matchsettingsTween.Start();
	}
	//Panel _gamemodePanel = (Panel) GetParent().GetNode("Gamemode Panel").GetNode("Left Panel");
	//Tween _gamemodeOptionTween = (Tween) GetParent().GetNode("Gamemode Panel").GetNode("Left Panel").GetNode("Rect Tween");

	private void _on_Settings_pressed()
	{
		_settingsPanel.Visible = true;

		_settingsTween.InterpolateProperty (
			_settingsPanel, //Object
			"rect_position", //Property being tweened
			new Vector2(-1024, 0), //from
			new Vector2(0,0), //to
			1, //speed
			Tween.TransitionType.Cubic,
			Tween.EaseType.Out
		);
		_settingsTween.Start();
	}
	
	private void _on_Back_pressed(int _index) //add a field to say which button
	{//TODO: Button scene (tscn)
		switch (_index)
		{
			case 1:
				_settingsPanel.Visible = false;
				break;
			case 2:
				_gamemodePanel.Visible = false;
				break;
			case 3:
				_matchsettingsPanel.Visible = false;
				break;
			case 4:
				_editorPanel.Visible = false;
				break;
		}
	}
	
	private void _on_GraphicsQuality_pressed(int _index)
	{
		GD.Print($"Graphics quality level set to {_index}");
	}
	
	private void _on_Editor_pressed()
	{
		_editorPanel.Visible = true;

		_editorTween.InterpolateProperty (
			_editorPanel, //Object
			"rect_position", //Property being tweened
			new Vector2(0, 600), //from
			new Vector2(0,0), //to
			1, //speed
			Tween.TransitionType.Cubic,
			Tween.EaseType.Out
		);
		_editorTween.Start();
	}
	#endregion
}
