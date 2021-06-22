using Godot;
using System;
using System.Collections.Generic;

public class GameManager : Node2D
{
	public static int CurrentTurn;
	public static List<int, Player> Players = new List<int, Player>();
	
	private void _on_Back_pressed() =>
		GetTree().ChangeScene("res://scenes/Title.tscn"); //TODO: Make match finish screen, pause, etc

	public static int SwitchTurn(int _currentTurn)
	{
		//if (GameConfig.TwoPlayer) //TODO: Multi mode
		if (_currentTurn == 1)
			_currentTurn = 2;
		else
			_currentTurn = 1;
		return _currentTurn;
	}
}
