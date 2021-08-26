using Godot;
using Crashteroids;
using System;
using System.Threading.Tasks;

public class GameOver : Control
{
	private static Control _control;
	private static Panel _panel;
	private static Panel _moneyPanel;
	private static Tween _controlTween;
	private static RichTextLabel _moneyDetails;
	private static Label _winner;
	private static Label _winnerOutline;
	private static RichTextLabel _details;
	private static Button _done;
	private static Button _again;
	private static Button _quit;

	public override void _Ready()
	{
		_control = (Control)this;
		_control.Visible = true;

		_panel = GetNode<Panel>("Panel");
		_moneyPanel = GetNode<Panel>("Panel2");
		_controlTween = GetNode<Tween>("Tween");
		_moneyDetails = GetNode("Panel2").GetNode<RichTextLabel>("Details");
		_winner = GetNode("Panel").GetNode<Label>("Winner");
		_winnerOutline = GetNode("Panel").GetNode<Label>("Winner Outline");
		_details = GetNode("Panel").GetNode<RichTextLabel>("Details");
		_done = GetNode("Panel2").GetNode<Button>("Done");
		_again = GetNode("Panel").GetNode<Button>("Again");
		_quit = GetNode("Panel").GetNode<Button>("Quit");

		Visible = false;
	}

	public static async void InitialiseGameOver(Player _sender)
	{
		_control.Visible = true;
		_controlTween.InterpolateProperty(
			_control, //Object
			"rect_scale", //Property being tweened
			new Vector2(0, 0), //from
			new Vector2(1, 1), //to
			1, //speed
			Tween.TransitionType.Back,
			Tween.EaseType.Out
		);
		_controlTween.Start();

		//TODO: Play a typewrite sound or something

		foreach (var _reward in GameManager.GameMatch.GameOverRewards)
		{
			string _title = _reward.Key.ToString();
			int _value = _reward.Value;
			_moneyDetails.AddText($"+ {_title} bonus -> £{_value} \n");
		}

		_moneyDetails.AppendBbcode($"[rainbow freq=0.2 sat=10 val=20]Total: £{GameConfig.Match.MatchMoney}[/rainbow]");

		_controlTween.InterpolateProperty(
			_moneyDetails, //object
			"percent_visible", //property
			0, //from
			1, //to
			3, //speed
			Tween.TransitionType.Cubic,
			Tween.EaseType.InOut
		);
		_controlTween.Start();

		if (GameConfig.Instance.Username != null)
			_winner.Text =
				$"{GameConfig.Instance.Username[0].ToString().ToUpper()}{GameConfig.Instance.Username.Remove(0, 1)} {GameManager.Players.IndexOf(_sender) + 1} won!";
		else
			_winner.Text = $"Player {GameManager.Players.IndexOf(_sender) + 1} won!"; //wrong!
		_winnerOutline.Text = _winner.Text;

		_details.BbcodeText =
			$"[wave amp=10 freq=5][color=yellow][center]Details:[/center][/color][/wave] \n Rounds: 0 \n Bounces: {GameManager.GameMatch.TotalBounces[GameManager.GameMatch.CurrentTurn]} \n Match Length: {GameManager.GameMatch.MatchLength}";
	}

	private void _on_Done_pressed() =>
		_moneyPanel.Visible = false;

	private void _on_Button_pressed(int _index)
	{
		///<note> _index 1 = Again </note>
		if (_index == 1)
		{
			GetTree().ReloadCurrentScene();
		}
		else
		{
			GetTree().ChangeScene("res://scenes/Title.tscn");
		}
	}
}
