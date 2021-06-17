using Godot;
using System;

public class TitleUiManager : Node
{
	private Panel _mainPanel;
	private Panel _settingsPanel;
	private Panel _gamemodePanel;
	private Panel _matchsettingsPanel;
	private Tween _mainTween;
	
	public override void _Ready()
	{
		_mainPanel = (Panel) GetParent().GetNode("Main Panel");
		_settingsPanel = (Panel) GetParent().GetNode("Settings Panel");
		_gamemodePanel = (Panel) GetParent().GetNode("Gamemode Panel");
		_matchsettingsPanel = (Panel) GetParent().GetNode("Matchsettings Panel");
		_mainTween = (Tween) GetParent().GetNode("Main Panel").GetNode("Panel Tween");
		
		_settingsPanel.Visible = false;
		_gamemodePanel.Visible = false;
		_matchsettingsPanel.Visible = false;
	}
//  public override void _Process(float delta) {}
	
	//TODO: in future use something fancy like tween
	#region SIGNALS
	private void _on_Gamemode_pressed() =>
		_gamemodePanel.Visible = true;
	private void _on_Settings_pressed() =>
		_settingsPanel.Visible = true;
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
		}
	}
	private void _on_GraphicsQuality_pressed(int _index)
	{
		GD.Print($"Graphics quality level set to {_index}");
	}
	#endregion
}
