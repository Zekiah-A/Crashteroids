using Godot;
using System;
using System.Collections.Generic;

public class GameManager : Node2D
{
	public static Match GameMatch;
	public static List<Player> Players = new List<Player>();
	
	///<summary> Called when the scene is created (match started). </sumamry>
	public override void _EnterTree()
	{
		if (GameConfig.Gamemode == (int) Gamemodes.TwoPlayer)
		{
			Players.Add(GetNode("P1") as Player);
			Players.Add(GetNode("P2") as Player);
		}
		
		GameMatch = new Match(); 
	}
	
	private void _on_Back_pressed() =>
		GetTree().ChangeScene("res://scenes/Title.tscn"); //TODO: Make match finish screen, pause, etc
}
