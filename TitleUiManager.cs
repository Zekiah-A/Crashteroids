using Godot;
using Crashteroids;
using System;

public class TitleUiManager : Node
{
	private Panel _mainPanel;
	private Panel _settingsPanel;
	private Panel _gamemodePanel;
	private Panel _matchsettingsPanel;
	private Panel _editorPanel;
	private Panel _helpPanel;
	private Panel _helpCreditsPanel;

	private Tween _settingsTween;
	private Tween _gamemodeTween;
	private Tween _matchsettingsTween;
	private Tween _editorTween;
	private Tween _helpTween;
	private Tween _usernameTween;

	private Label _moneyLabel;
	private LineEdit _usernameEdit;
	private RichTextLabel _usernameLabel;
	
	private Control _musicCheckbox;
	private Control  _sfxCheckbox;
	private Control  _helpBtnCheckbox;
	private Control  _advertisementsCheckbox;
	

	const int MaxUsernameLength = 10;

	public override void _Ready()
	{
		//GameSaveData.Load();
		//GameSaveDataUpdate();
		
		_mainPanel = (Panel) GetParent().GetNode("Main Panel");
		_settingsPanel = (Panel) GetParent().GetNode("Settings Panel");
		_gamemodePanel = (Panel) GetParent().GetNode("Gamemode Panel");
		_matchsettingsPanel = (Panel) GetParent().GetNode("Matchsettings Panel");
		_editorPanel = (Panel) GetParent().GetNode("Editor Panel");
		_helpPanel = (Panel) GetParent().GetNode("Help Panel");
		_helpCreditsPanel = (Panel) GetParent().GetNode("Help Panel").GetNode("Credits Panel");
		
		_settingsTween = (Tween) GetParent().GetNode("Settings Panel").GetNode("Panel Tween");
		_gamemodeTween = (Tween) GetParent().GetNode("Gamemode Panel").GetNode("Panel Tween");
		_matchsettingsTween = (Tween) GetParent().GetNode("Matchsettings Panel").GetNode("Panel Tween");
		_editorTween = (Tween) GetParent().GetNode("Editor Panel").GetNode("Panel Tween");
		_helpTween = (Tween) GetParent().GetNode("Help Panel").GetNode("Panel Tween");
		_usernameTween = (Tween) GetParent().GetNode("Settings Panel").GetNode("Right Panel").GetNode("Username Edit").GetNode("Tween");
		
		_moneyLabel = (Label) GetParent().GetNode("Main Panel").GetNode("Money");
		_usernameEdit = (LineEdit) GetParent().GetNode("Settings Panel").GetNode("Right Panel").GetNode("Username Edit");
		_usernameLabel = (RichTextLabel) GetParent().GetNode("Settings Panel").GetNode("Right Panel").GetNode("Username Edit").GetNode("Username Label");

		_musicCheckbox = (Control) GetParent().GetNode("Settings Panel").GetNode("Centre Panel").GetNode("Checkbox").GetNode("Checkbox");
		_sfxCheckbox = (Control) GetParent().GetNode("Settings Panel").GetNode("Centre Panel").GetNode("Checkbox2").GetNode("Checkbox");
		_helpBtnCheckbox = (Control) GetParent().GetNode("Settings Panel").GetNode("Right Panel").GetNode("Checkbox").GetNode("Checkbox");
		_advertisementsCheckbox = (Control) GetParent().GetNode("Settings Panel").GetNode("Right Panel").GetNode("Checkbox2").GetNode("Checkbox");

		_settingsPanel.Visible = false;
		_gamemodePanel.Visible = false;
		_matchsettingsPanel.Visible = false;
		_editorPanel.Visible = false;
		_helpPanel.Visible = false;
		_helpCreditsPanel.Visible = false;

		//GameConfigUpdate += //TODO: If index = 0, update instead for all functions.
		
		//TODO: Is ready called multiple times? - Make a function that subscribes for money change, (money change called by config update event too)
		//BUG: Loads too early, must call load before to wait for cofig
		
		/*GameSaveData.GameSaveDataUpdate += gameSaveDataUpdate;*/
	}
	
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
	{
		GameConfig.Gamemode = _index;
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
		
		if (GameSaveData.Load())
			GameSaveDataUpdate();
	}
	
	private async void _on_Back_pressed(int _index) //add a field to say which button
	{//TODO: Button scene (tscn)
		switch (_index)
		{
			case 1:
				_settingsPanel.Visible = false;
				await GameSaveData.Save();
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
			case 5:
				_helpPanel.Visible = false;
				break;
			case 6:
				_helpCreditsPanel.Visible = false;
				break;
		}
	}
	
	private void _on_GraphicsQuality_pressed(int _index)
	{
		GD.Print($"Graphics quality level set to {_index}");
		GameConfig.Instance.GraphicsQualitySetting = _index;
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
	
	private void _on_Help_pressed()
	{
		_helpPanel.Visible = true;
		_helpTween.InterpolateProperty (
			_helpPanel, //Object
			"rect_scale", //Property being tweened
			new Vector2(0, 0), //from
			new Vector2(1, 1), //to
			1, //speed
			Tween.TransitionType.Back,
			Tween.EaseType.Out
		);
		_helpTween.Start();
	}
	
	private void _on_Credits_pressed() =>
		_helpCreditsPanel.Visible = true;
	
	//General config: Save after changes from ui!
	private void _on_Username_Edit_text_entered(String _newText)
	{
		if (_newText.Length <= MaxUsernameLength)
			GameConfig.Instance.Username = _newText;
		else
			GameConfig.Instance.Username = _newText.Substring(0, MaxUsernameLength);
		
		_usernameLabel.Visible = true;
		_usernameTween.InterpolateProperty (
			_usernameLabel, //Object
			"rect_position", //Property being tweened
			new Vector2(0, 32), //from
			new Vector2(0, 64), //to
			0.5f, //speed
			Tween.TransitionType.Cubic,
			Tween.EaseType.Out
		);
		_usernameTween.Start();
		
		_usernameLabel.BbcodeText = $"[color=yellow]Username set to: {GameConfig.Instance.Username}[/color]";
		
		GD.Print($"Player username set to {GameConfig.Instance.Username}");
	}
	
	#region MATCH CONFIGURATION //NOTE: + General configuration (for the time being)
	private void _on_Matchconfig_update(int _configId, int _newValue)
	{
		GD.Print($"Signal custom recieved for match config {_configId}, {_newValue}");
		
		switch (_configId)
		{
			case 1:
				GameConfig.Match.RandomMap = Convert.ToBoolean(_newValue);
				break;
			case 2:
				GameConfig.Match.SpecialAbilities = Convert.ToBoolean(_newValue);
				break;
			case 3:
				GameConfig.Match.RocketBounces = _newValue;
				break;
			case 4:
				GameConfig.Match.Rounds = _newValue;
				break;
			//General config: Save after changes from UI.
			case 5:
				GameConfig.Instance.Music = Convert.ToBoolean(_newValue);
				break;
			case 6:
				GameConfig.Instance.SoundEffects = Convert.ToBoolean(_newValue);
				break;
		}
	}
	#endregion
	
	private void _on_Start_pressed()
	{
		GD.Print("Starting Game with configuration:");
		GD.Print($"Gamemode: {(Gamemodes) GameConfig.Gamemode}.");
		GD.Print($"Random Map: {GameConfig.Match.RandomMap}");
		GD.Print($"Special Abilities: {GameConfig.Match.SpecialAbilities}");
		GD.Print($"Rocket Bounces: {GameConfig.Match.RocketBounces}");
		GD.Print($"Rounds: {GameConfig.Match.Rounds}");
		
		GetTree().ChangeScene("res://scenes/Game.tscn");
		//NOTE: Starting match is called from button signal
	}
	#endregion

	//When the game's config is loaded, update all UI elements to reflect that of the setting
	public void GameSaveDataUpdate() //CALL THIS!
	{
		GD.Print("GameSaveData update received.");
		
		try{
			(_musicCheckbox as Checkbox).IsEnabled = GameConfig.Instance.Music;
			(_sfxCheckbox as Checkbox).IsEnabled = GameConfig.Instance.SoundEffects;
			(_helpBtnCheckbox as Checkbox).IsEnabled = GameConfig.Instance.Advertisements;
			//TODO: There is no void to actually update this, so that needs to be done.
			(_advertisementsCheckbox as Checkbox).IsEnabled = GameConfig.Instance.Advertisements;
			_usernameEdit.Text = GameConfig.Instance.Username;
			//_on_Username_Edit_text_entered(GameConfig.Instance.Username);
		}
		catch(Exception e)
		{
			GD.PrintErr($"Could not update game save data \n {e}");
		}
	}
}
