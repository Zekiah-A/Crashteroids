using Godot;
using System;
using System.Collections.Generic;

public class Match //: Node
{
	public int CurrentTurn;
	//private Label _turnUI;
	
	public Match()
	{	//HACK: Remove until found a way
		//_turnUI = GetNode<Label>("/root/Node2D/TurnUI/Label");
		GameManager.Players[0].IsCurrent = true;
	}

	public void SwitchTurn(int _finishedTurn)
	{
		if (GameConfig.Gamemode == (int) Gamemodes.TwoPlayer)
		{
			if (_finishedTurn == 0)
				CurrentTurn = 1;
			else
				CurrentTurn = 0;
		}
		
		foreach (Player _player in GameManager.Players)
		{
			if (_player.Id == CurrentTurn)
				GameManager.Players[CurrentTurn].IsCurrent = true;
			else
				GameManager.Players[CurrentTurn].IsCurrent = false;
		}
		
		//_turnUI.Text = "Player " + CurrentTurn +"'s turn.";
	}
}
