using Godot;
using System;
using System.Threading.Tasks;

public class GameOver : Control
{
	private static Panel _panel;
	private static Tween _panelTween;
	private static Label _winner;
	private static Label _winnerOutline;
	private static RichTextLabel _details;
	private static Button _again;
	private static Button _quit;
	
	public override void _Ready()
	{
		_panel = GetNode<Panel>("Panel");
		_panel.Visible = false;
		
		_panelTween = GetNode("Panel").GetNode<Tween>("Tween");
		_winner = GetNode("Panel").GetNode<Label>("Winner");
		_winnerOutline = GetNode("Panel").GetNode<Label>("Winner Outline");
		_details = GetNode("Panel").GetNode<RichTextLabel>("Details");
		_again = GetNode("Panel").GetNode<Button>("Again");
		_quit = GetNode("Panel").GetNode<Button>("Quit");
	}
	
	public static void InitialiseGameOver()
	{	
		_panel.Visible = true;
		_panelTween.InterpolateProperty (
			_panel, //Object
			"rect_scale", //Property being tweened
			new Vector2(0, 0), //from
			new Vector2(1, 1), //to
			1, //speed
			Tween.TransitionType.Back,
			Tween.EaseType.Out
		);
		_panelTween.Start();

		_winner.Text = "Player " + (GameManager.GameMatch.CurrentTurn + 1) + " won!";
		_winnerOutline.Text = "Player " + (GameManager.GameMatch.CurrentTurn + 1) + " won!";
		
		_details.BbcodeText = $"[wave amp=10 freq=5][color=yellow][center]Details:[/center][/color][/wave] \n Rounds: NULL \n Bounces: {GameManager.GameMatch.TotalBounces[GameManager.GameMatch.CurrentTurn]} \n Match Length: {GameManager.GameMatch.MatchLength}";
	}
	
	private void _on_Button_pressed(int _index)
	{	///<note> _index 1 = Again </note>
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
