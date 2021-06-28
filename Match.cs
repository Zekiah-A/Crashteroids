using Godot;
using Crashteroids;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Match : Node
{
	public int CurrentTurn;
	public float MatchLength;
	
	public Match()
	{
		GameManager.Players[0].IsCurrent = true;
		GameManager.Players[0].UpdateSkin();
		GameManager.TimerNode.Connect("timeout", this, nameof(_on_Timer_timeout));
		GameManager.TimerNode.Start();
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
	
	public void Crash(/*Player _sender, string _hit*/)
	{
		EndMatch();
		//HACK: User curplr - not correct!
		GameManager.Players[CurrentTurn].Explode();
	}
	
	private void _on_Timer_timeout() =>
		MatchLength += GameManager.TimerNode.WaitTime;
	
	public void EndMatch()
	{
		//TODO: Destroy this instance
		GameOver.InitialiseGameOver();
	}
}
//TODO: Shift player numbers up 1, and 1 back for arrays.
