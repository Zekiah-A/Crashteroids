using Godot;
using System;
using System.Threading;
using System.Collections.Generic;

public class GameManager : Node
{
	public static Tween CameraTween;
	public static Camera2D GameCamera;
	public static Match GameMatch;
	public static Label TurnUi;
	public static Godot.Timer TimerNode;
	public static Godot.Timer EndGameTimer;
	public static List<Player> Players = new List<Player>();

	///<summary> Called when the scene is created (match started). </summary>
	public override void _Ready()
	{
		TurnUi = GetNode("CanvasLayer").GetNode("Interface").GetNode("PlayerTurn").GetNode<Label>("Title");
		TimerNode = GetNode<Godot.Timer>("Timer");
		EndGameTimer = GetNode<Godot.Timer>("EndGameTimer"); //TODO: This is wrong!
		GameCamera = GetNode<Camera2D>("Camera2D");
		CameraTween = GetNode<Tween>("CameraTween");
		Players.Clear();

		//if (GameConfig.Gamemode == (int)Gamemodes.TwoPlayer)
		//{
			foreach (Node node in GetTree().CurrentScene.GetChildren())
			{
				if (node.IsInGroup("Player"))
					Players.Add(node as Player);
			}
			GameMatch = new TwoPlayerMatch();
		//}


	}

	private void QuitPressed() => GameMatch.EndGame();
}
