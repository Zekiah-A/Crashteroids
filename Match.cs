using Godot;
using Crashteroids;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Match : Node
{
	public int CurrentTurn;
	public int MatchLength;
	public List<int> TotalBounces = new List<int>() { 0, 0 }; //could be a fixed array as two?
	public Dictionary<RewardsType, int> GameOverRewards = new Dictionary<RewardsType, int>();
	private Random _rand = new Random();
	
	public Match()
	{
		GameManager.Players[0].IsCurrent = true;
		GameManager.Players[0].UpdateSkin();
		GameManager.TimerNode.Connect("timeout", this, nameof(_on_Timer_timeout));
		GameManager.TimerNode.Start();
		if (GameConfig.Instance.Username != null)
			GameManager.TurnUI.Text = $"Player {GameConfig.Instance.Username} {CurrentTurn + 1}'s turn.";
	}

	public void SwitchTurn(int _finishedTurn)
	{	///<summary> Stop current player from moving </summary>
		GameManager.Players[CurrentTurn].IsCurrent = false;
		///<summary> Switch the current player for the match. Scales for different player amounts.</summary>
		if (_finishedTurn + 1 < GameManager.Players.Count)
			CurrentTurn++;
		else
			CurrentTurn = 0;
		///<summary>Re-set the new current player to be able to move</summary>
		GameManager.Players[CurrentTurn].IsCurrent = true;
		///<note> Add 1 to current turn in order to not confuse players. </note>
		if (GameConfig.Instance.Username != null)
			GameManager.TurnUI.Text = $"Player {GameConfig.Instance.Username} {CurrentTurn + 1}'s turn.";
		else
			GameManager.TurnUI.Text = $"Player {CurrentTurn + 1}'s turn.";
	}
	
	public void Crash(Player _playerHit)
	{
		EndMatch();
		GameManager.Players[_playerHit.Id].Explode();
	}
	
	private void _on_Timer_timeout() =>
		MatchLength += (int) GameManager.TimerNode.WaitTime;
	
	//make destructor 
	public void EndMatch()
	{
		//TODO: Destroy this instance - lazy hack for winner too
		switch((Gamemodes) GameConfig.Gamemode)
		{
			case Gamemodes.TwoPlayer:
				GameOverRewards.Add(RewardsType.GameWin, 10);
				GameOverRewards.Add(RewardsType.Random, _rand.Next(0, 100));
				GameOverRewards.Add(RewardsType.Bounces, TotalBounces[CurrentTurn]);
				GameOverRewards.Add(RewardsType.Rounds, 0); //for now
				GameOverRewards.Add(RewardsType.MatchLength, MatchLength);
				GameOverRewards.Add(RewardsType.SpecialAbilities, GameConfig.Match.Rounds);
				
				foreach(var _reward in GameOverRewards)
				{
					GameConfig.Instance.Money += _reward.Value;
					GameConfig.Match.MatchMoney += _reward.Value;
				}
				break;
			case Gamemodes.AiPlayer:
				break;
			case Gamemodes.Multiplayer:
				break;
		}

		GameOver.InitialiseGameOver();
	}
}
//TODO: Shift player numbers up 1, and 1 back for arrays.
