using Godot;
using System;
using System.Collections.Generic;

public class TwoPlayerMatch : Match
{
	public override void Crash(Player playerHit, Player sender)
	{
		Winner = sender;
		playerHit.Explode();
		
		GameManager.CameraTween.InterpolateProperty(
			GameManager.GameCamera,
			"position",
			GameManager.GameCamera.Position,
			(playerHit.Position + sender.Position) / 2, //TODO: This may be pointless 
			0.5f,
			Tween.TransitionType.Cubic,
			Tween.EaseType.Out
		);
		GameManager.CameraTween.InterpolateProperty(
			GameManager.GameCamera,
			"zoom",
			GameManager.GameCamera.Zoom,
			new Vector2(0.5f, 0.5f),
			0.5f,
			Tween.TransitionType.Cubic,
			Tween.EaseType.Out
		);
		GameManager.CameraTween.Start();

		Engine.TimeScale = 0.1f;
		GameManager.EndGameTimer.Start();
	}

	public override void EndGame()
	{
		//TODO: Add rewards here
		Engine.TimeScale = 1;

		GameManager.CameraTween.InterpolateProperty(
			GameManager.GameCamera,
			"position",
			GameManager.GameCamera.Position,
			new Vector2(504, 300), //TODO: Use a proper set number later (such as an original pos var)
			2,
			Tween.TransitionType.Cubic,
			Tween.EaseType.Out
		);
		GameManager.CameraTween.InterpolateProperty(
			GameManager.GameCamera,
			"zoom",
			GameManager.GameCamera.Zoom,
			new Vector2(1, 1),
			2,
			Tween.TransitionType.Cubic,
			Tween.EaseType.Out
		);
		GameManager.CameraTween.Start();

		GameOver.ShowGameOverScreen();
	}
}
