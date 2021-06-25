using Godot;
using System;
using System.Collections.Generic;

public class GameManager : Node2D
{
	public static Match GameMatch;
	public static List<Player> Players = new List<Player>();
	public static Label TurnUI;
	
	///<summary> Called when the scene is created (match started). </sumamry>
	public override void _Ready()
	{
		TurnUI = (Label) GetNode("Turn UI").GetNode("Label");
		if (GameConfig.Gamemode == (int) Gamemodes.TwoPlayer)
		{
			Players.Add(GetNode("P1") as Player);
			Players.Add(GetNode("P2") as Player);
		}
		
		GameMatch = new Match();
	}
	
	public override void _ExitTree() =>
		GameMatch = null;
	
	private void _on_Back_pressed() =>
		GetTree().ChangeScene("res://scenes/Title.tscn"); //TODO: Make match finish screen, pause, etc
}
