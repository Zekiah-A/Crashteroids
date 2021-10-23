using Godot;
using Crashteroids;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary> Match inherits 'Reference' in order to auto GC when no longer needed </summary>
public class Match : Reference
{
	public int CurrentTurn;
	public int MatchLength;
	//public List<int> TotalBounces = new List<int>() { 0, 0 }; //could be a fixed array as two?
	public int[] TotalBounces;
	public Dictionary<RewardsType, int> GameOverRewards = new Dictionary<RewardsType, int>();
	private Random _rand = new Random();

	public Match() //all unecessary, could just attach manager to node that reloads
	{
		GD.Print("Match instance generated.");

		foreach (Player player in GameManager.Players)
		{
			player.IsDead = false;
			player.Id = GameManager.Players.IndexOf(player);
		}

		TotalBounces = new int[GameManager.Players.Count];
		GameManager.Players[0].IsCurrent = true;
		GameManager.Players[0].UpdateSkin();
		GameManager.TimerNode.Connect("timeout", this, nameof(_on_Timer_timeout));
		GameManager.TimerNode.Start();
		if (!string.IsNullOrEmpty(GameConfig.Instance.Username))
			GameManager.TurnUI.Text = $"{GameConfig.Instance.Username[0].ToString().ToUpper()}{GameConfig.Instance.Username.Remove(0, 1)} {CurrentTurn + 1}'s turn.";
	}

	public void SwitchTurn(int _finishedTurn) //TODO: Finishedturn obsolete?
	{
		///<summary> Stop current player from moving </summary>
		GameManager.Players[CurrentTurn].IsCurrent = false;
		///<summary> Switch the current player for the match. Scales for different player amounts.</summary>
		CurrentTurn++;
		if (CurrentTurn >= GameManager.Players.Count)
			CurrentTurn = 0;
		///<summary>Re-set the new current player to be able to move</summary>
		GameManager.Players[CurrentTurn].IsCurrent = true;
		///<note> Add 1 to current turn in order to not confuse players. </note>
		if (!string.IsNullOrEmpty(GameConfig.Instance.Username))
			GameManager.TurnUI.Text = $"{GameConfig.Instance.Username[0].ToString().ToUpper()}{GameConfig.Instance.Username.Remove(0, 1)} {CurrentTurn + 1}'s turn.";
		else
			GameManager.TurnUI.Text = $"Player {CurrentTurn + 1}'s turn.";
	}

	public void Crash(Player _playerHit, Player _sender)
	{
		EndMatch(_sender); //wrong!, pass playerhit?
		GameManager.Players[_playerHit.Id].Explode();
	}

	private void _on_Timer_timeout() =>
		MatchLength += (int)GameManager.TimerNode.WaitTime;

	//make destructor 
	public async void EndMatch(Player _sender)
	{
		//TODO: Destroy this instance - lazy hack for winner too
		switch ((Gamemodes)GameConfig.Gamemode)
		{
			case Gamemodes.TwoPlayer:
				GameOverRewards.Add(RewardsType.GameWin, 10);
				GameOverRewards.Add(RewardsType.Random, _rand.Next(0, 100));
				GameOverRewards.Add(RewardsType.Bounces, TotalBounces[CurrentTurn]);
				GameOverRewards.Add(RewardsType.Rounds, GameConfig.Match.Rounds);
				GameOverRewards.Add(RewardsType.MatchLength, MatchLength);
				GameOverRewards.Add(RewardsType.SpecialAbilities, 0); //for now

				foreach (var _reward in GameOverRewards)
				{
					GameConfig.Instance.Money += _reward.Value;
					GameConfig.Match.MatchMoney += _reward.Value;
				}

				await GameSaveData.Save();
				break;
			case Gamemodes.AiPlayer:
				break;
			case Gamemodes.Multiplayer:
				break;
		}

		GameOver.InitialiseGameOver(_sender);
	}
}
//TODO: Shift player numbers up 1, and 1 back for arrays.
