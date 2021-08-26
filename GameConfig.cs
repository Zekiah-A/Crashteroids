using Godot;
using System;
using System.Collections.Generic;

namespace Crashteroids
{
	public class GameConfig
	{
		public static GameConfig Instance = new GameConfig();

		public int GraphicsQualitySetting { get; set; }
		public bool Music { get; set; }
		public bool SoundEffects { get; set; }
		public int SkinID { get; set; }
		public int Money { get; set; }
		public string Username { get; set; }

		public bool Advertisements { get; set; }

		//public List<EditorItem>EditorItems {get; set;}
		public List<int> BoughtItems = new List<int>();
		public List<int> EquippedItems = new List<int>();

		#region MATCH

		public static int Gamemode { get; set; }

		public struct Match
		{
			public static bool RandomMap { get; set; }
			public static bool SpecialAbilities { get; set; }
			public static int RocketBounces { get; set; }
			public static int Rounds { get; set; }
			public static int MatchMoney { get; set; }
		}

		#endregion

		public static void GenerateInstance(GameConfig _newConfig) => Instance = _newConfig;
	}
}

public enum Gamemodes
{
	TwoPlayer = 1,
	AiPlayer,
	Multiplayer,
	Designer
}

public enum RewardsType
{
	GameWin = 1,
	Random,
	Bounces,
	Rounds,
	MatchLength,
	SpecialAbilities
}

public enum EditorIds
{
	Null = 0,
	Laser,
	Other2,
	Other3,
	Other4,
	Other5,
	Other6,
	Other7,
	Other8,
	Other9,
	Other10,
	Other11,
	Other12,
	Other13
}
