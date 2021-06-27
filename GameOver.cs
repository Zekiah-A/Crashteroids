using Godot;
using System;

public class GameOver : Control
{
	private static Panel _panel;
	private static Label _winner;
	private static Label _winnerOutline;
	private static RichTextLabel _details;
	private static Button _again;
	private static Button _quit;
	
	public override void _Ready()
	{
		_panel = GetNode<Panel>("Panel");
		_panel.Visible = false;
		
		_winner = GetNode("Panel").GetNode<Label>("Winner");
		_winnerOutline = GetNode("Panel").GetNode<Label>("Winner Outline");
		_details = GetNode("Panel").GetNode<RichTextLabel>("Details");
		_again = GetNode("Panel").GetNode<Button>("Again");
		_quit = GetNode("Panel").GetNode<Button>("Quit");
	}
	
	public static void InitialiseGameOver()
	{	//HACK: This may not always be correct
		_panel.Visible = true;
		
		_winner.Text = "Player " + GameManager.GameMatch.CurrentTurn + " won!";
		_winnerOutline.Text = "Player " + GameManager.GameMatch.CurrentTurn + " won!";
		
		_details.BbcodeText = $"[wave amp=10 freq=5][color=yellow][center]Details:[/center][/color][/wave] \n Rounds: NULL \n Bounces: NULL \n Match Length: {GameManager.GameMatch.MatchLength}";
	}
	
	private void _on_Button_pressed(int _index)
	{	//<note> _index 1 = Again </note>
		if (_index == 1)
		{
			//TODO: Tell game to restart match
		}
		else
		{
			GetTree().ChangeScene("res://scenes/Title.tscn");
		}
	}
}
