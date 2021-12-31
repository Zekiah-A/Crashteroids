using Godot;
using System;
using System.Net;

public class DeveloperConsole : Button
{
	private LineEdit console;
	
	public override void _Ready()
	{
		console = GetNode<LineEdit>("Console");
		Connect("pressed", this, nameof(VersionIndicatiorPressed));
		console.Connect("text_entered", this, nameof(CommandEntered));
	}

	private void VersionIndicatiorPressed() =>
		console.Visible = !console.Visible;

	private void CommandEntered(string input)
	{
		console.Text = "";
		
		switch (input.Split(" ")[0].ToLower())
		{
			case "username":
				GameData.Username = input.Split(" ")[1];
				break;
			case "money":
					if (Int32.TryParse(input.Split(" ")[1], out int money));
						GameData.Money = money;
					break;
			default:
				console.Text = "Error: Could not parse input.";
				break;
		}
	}
}
