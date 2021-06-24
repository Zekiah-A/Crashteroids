using Godot;
using System;
using System.ComponentModel;
using System.Collections.Generic;

public class Match
{
	public int CurrentTurn;
	//private Label _turnUI;
	
	public Match()
	{	//HACK: Remove until found a way
		//_turnUI = GetNode<Label>("/root/Node2D/TurnUI/Label");
		GameManager.Players[0].IsCurrent = true;
	}

	public void SwitchTurn(int _finishedTurn)
	{	///<summary> Stop current player from moving </summary>
		GameManager.Players[CurrentTurn].IsCurrent = false;
		///<summary> Switch the current player for the match </summary>
		if (GameConfig.Gamemode == (int) Gamemodes.TwoPlayer)
		{
			switch (_finishedTurn)
			{
				case 0:
					CurrentTurn = 1;
					break;
				case 1:
					CurrentTurn = 0;
					break;
			}
		}
		///<summary>Re-set the new current player to be able to move</summary>
		GameManager.Players[CurrentTurn].IsCurrent = true;
		///<note> Add 1 to current turn in order to not confuse players. </note>
		GameManager.TurnUI.Text = "Player " + (CurrentTurn + 1) +"'s turn.";
	}
}
//TODO: Shift player numbers up 1, and 1 back for arrays.
