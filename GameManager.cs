using Godot;
using Crashteroids;
using System;
using System.Threading;
using System.Collections.Generic;

public class GameManager : Node2D
{
	public static Match GameMatch;
	public static Label TurnUI;
	public static Godot.Timer TimerNode;
	public static List<Player> Players = new List<Player>();

	///<summary> Called when the scene is created (match started). </sumamry>
	public override void _Ready()
	{
		TurnUI = (Label)GetNode("Turn UI").GetNode("Label");
		TimerNode = (Godot.Timer)GetNode("Timer");

		Players.Clear();
		if (GameConfig.Gamemode == (int)Gamemodes.TwoPlayer)
		{
			foreach (Node node in GetTree().CurrentScene.GetChildren())
			{
				if (node.IsInGroup("Player"))
					Players.Add(node as Player);
			}
		}

		GameMatch = new Match();
	}

	public override void _ExitTree() =>
		GameMatch = null;

	private void _on_Back_pressed() =>
		GameOver.InitialiseGameOver(Players[GameMatch.CurrentTurn]);
}
