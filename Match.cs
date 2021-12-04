using Godot;
using System;

///<summary> Generic base class for all types of matches to inherit from. </summary>
public class Match : Reference
{
	public Player Winner;
	public int CurrentTurn;
	public int MatchLength;
	public int[] TotalBounces;

	public Match() //TODO: Should be instancing the player through code instead? Smartest methos id just makje differnet things for different maps lol, different map editions
	{
		foreach (Player player in GameManager.Players)
		{
			player.IsDead = false;
			player.Id = GameManager.Players.IndexOf(player);
		}

		TotalBounces = new int[GameManager.Players.Count];
		GameManager.Players[0].IsCurrent = true;
		GameManager.TimerNode.Connect("timeout", this, nameof(OnTimerTimeout));
		GameManager.EndGameTimer.Connect("timeout", this, nameof(EndGame));
		GameManager.TimerNode.Start();
		GameManager.TurnUI.Text = TitleUsername();
	}
	
	public virtual void SwitchTurn(int finishedTurn)
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
		GameManager.TurnUI.Text = TitleUsername();
	}

	public virtual void Crash(Player playerHit, Player sender) {}
	public virtual void EndGame() {}

	private void OnTimerTimeout()
	{
		MatchLength += (int) GameManager.TimerNode.WaitTime;
	}
	private string TitleUsername()
	{
		//if (!string.IsNullOrEmpty(GameConfig.Instance.Username))
		//	return $"{GameConfig.Instance.Username[0].ToString().ToUpper()}{GameConfig.Instance.Username.Remove(0, 1)} {CurrentTurn + 1}'s turn.";
		//else
			return $"Player {CurrentTurn + 1}'s turn.";

	}
}
