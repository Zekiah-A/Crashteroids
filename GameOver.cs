using Godot;
using System;
using System.Text;
using System.Threading.Tasks;

public class GameOver : Control
{
	private static Control control;
	private static Panel panel;
	private static Panel moneyPanel;
	private static Tween controlTween;
	private static RichTextLabel moneyDetails;
	private static Label winner;
	private static Label winnerOutline;
	private static RichTextLabel details;
	private static Button done;
	private static Button again;
	private static Button quit;

	public override void _Ready()
	{
		control = (Control)this;
		control.Visible = true;

		panel = GetNode<Panel>("MainPanel");
		moneyPanel = GetNode<Panel>("DetailsPanel");
		controlTween = GetNode<Tween>("Tween");
		moneyDetails = GetNode("DetailsPanel").GetNode<RichTextLabel>("Details");
		winner = GetNode("MainPanel").GetNode<Label>("Winner");
		winnerOutline = GetNode("MainPanel").GetNode<Label>("Winner Outline");
		details = GetNode("MainPanel").GetNode<RichTextLabel>("Details");
		done = GetNode("DetailsPanel").GetNode<Button>("Done");
		again = GetNode("MainPanel").GetNode<Button>("Again");
		quit = GetNode("MainPanel").GetNode<Button>("Quit");

		Visible = false;
	}

	public static void ShowGameOverScreen()
	{
		Player sender = GameManager.GameMatch.Winner;

		control.Visible = true;
		controlTween.InterpolateProperty(
			control, //Object
			"rect_scale", //Property being tweened
			new Vector2(0, 0), //from
			new Vector2(1, 1), //to
			1, //speed
			Tween.TransitionType.Back,
			Tween.EaseType.Out
		);
		controlTween.Start();

		//TODO: Play a typewrite sound or something

		//foreach (var _reward in GameManager.GameMatch.GameOverRewards)
		//{
		//	string _title = PascalToNormal(_reward.Key.ToString());
		//	int _value = _reward.Value;
		//	_moneyDetails.AddText($"+ {_title} bonus -> £{_value} \n");
		//}

		//moneyDetails.AppendBbcode($"[rainbow freq=0.2 sat=10 val=20]Total: £{GameConfig.Match.MatchMoney}[/rainbow]");

		controlTween.InterpolateProperty(
			moneyDetails, //object
			"percent_visible", //property
			0, //from
			1, //to
			3, //speed
			Tween.TransitionType.Cubic,
			Tween.EaseType.InOut
		);
		controlTween.Start();
		/*
		if (!string.IsNullOrEmpty(GameConfig.Instance.Username))
			_winner.Text = $"{GameConfig.Instance.Username[0].ToString().ToUpper()}{GameConfig.Instance.Username.Remove(0, 1)} {GameManager.Players.IndexOf(_sender) + 1} won!";
		else
			_winner.Text = $"Player {GameManager.Players.IndexOf(_sender) + 1} won!"; //wrong!
		*/
		winnerOutline.Text = winner.Text;

		details.BbcodeText = $"[wave amp=10 freq=5][color=yellow][center]Details:[/center][/color][/wave] \n Game rounds: 0 \n Winner bounces: {GameManager.GameMatch.TotalBounces[GameManager.GameMatch.CurrentTurn]} \n Match length: {GameManager.GameMatch.MatchLength}";
	}

	private static string PascalToNormal(string input)
	{
		var builder = new StringBuilder();

		for (var index = 0; index < input.Length; index++)
		{
			var letter = input[index];

			if (char.IsUpper(letter) && index != 0)
				builder.Append($" {char.ToLower(letter)}");
			else if (index == 0)
				builder.Append(char.ToUpper(letter));
			else
				builder.Append(letter);
		}
		return builder.ToString();
	}

	private void DonePressed() => moneyPanel.Visible = false;

	private void OptionPressed(int _index)
	{
		///<note> _index 1 = Again </note>
		if (_index == 1)
			GetTree().ReloadCurrentScene();
		else
			GetTree().ChangeScene("res://scenes/Title.tscn");
	}
}
