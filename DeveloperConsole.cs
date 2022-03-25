using Godot;
using System;
using System.Net;

public class DeveloperConsole : Button
{
	private int clicks = 1;
	private bool configOpen;
	private LineEdit console;
	
	public override void _Ready()
	{
		console = GetNode<LineEdit>("Console");
		Connect("pressed", this, nameof(VersionIndicatiorPressed));
		console.Connect("text_entered", this, nameof(CommandEntered));
	}

	private void VersionIndicatiorPressed()
	{
		if (clicks == 3 && !console.Visible)
		{
			console.Visible = true;
			clicks = -1;
		}
		else if (console.Visible)
			console.Visible = false;
		clicks++;
	}

	private void CommandEntered(string input)
	{
		console.Text = "";
		
		switch (input.Split(" ")[0].ToLower())
		{
			case "help":
				var helpPopup = new AcceptDialog
				{
					WindowTitle = "!!!! Crashteroids Developer Console !!!!",
					DialogText = "If you do not know how you got here,\n*exit by pressing on the version number\nin the top right corner of the screen.*\n\n------------------\nCommands:\n------------------\n- username \n- money \n- config \n- sysinfo \n- close / exit \n- help (this) \n- secret",
					Resizable = true,
					Visible = true
				};
				AddChild(helpPopup);
				helpPopup.Popup_();
				break;
			case "username":
				GameData.Username = input.Split(" ")[1];
				break;
			case "money":
					if (Int32.TryParse(input.Split(" ")[1], out int money));
						GameData.Money = money;
					break;
			case "config":
				var file = new File();
				file.Open("user://game_config_crashteroids.cfg", File.ModeFlags.ReadWrite);
				if (!configOpen)
				{
					var textEdit = new TextEdit
					{
						Name = "TextEdit",
						Text = file.GetAsText(),
						RectSize = new Vector2(800, 500),
						RectPosition = new Vector2(50 - (RectSize.x / 2), 100 - (RectSize.y / 2))
					};
					AddChild(textEdit);
					configOpen = true;
				}
				else
				{
					var textEdit = GetNode<TextEdit>("TextEdit");
					file.StoreString(textEdit.Text);
					file.Close();
					textEdit.QueueFree();
					configOpen = false;
					console.Text = "";
					console.PlaceholderText = "*Game must be reloaded*";
				}
				break;
			case "sysinfo":
				var sysinfoPopup = new AcceptDialog
				{
					WindowTitle = "System Info:",
					DialogText = $"version: {Text} \nresolution: {OS.GetScreenSize()} \ndpi: {OS.GetScreenDpi()} \nconfig: {OS.GetDataDir()} \nlocale: {OS.GetLocale()} \ndriver: {OS.GetCurrentVideoDriver()}",
					Resizable = false,
					Visible = true
				};
				AddChild(sysinfoPopup);
				sysinfoPopup.Popup_();
				break;
			case "close":
				console.Visible = false;
				break;
			case "exit":
				console.Visible = false;
				break;
			case "secret":
			#if GODOT_X11
				for (int i = 1; i < 5; i++)
					OS.Execute("notify-send", new []{"Crashteroids is the coolest!"}, false);
			#endif
				Input.VibrateHandheld(1000);
				break;
			default:
				console.Text = "";
				console.PlaceholderText = "Error: Could not parse input";
				break;
		}
	}
}
