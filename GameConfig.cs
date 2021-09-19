using Godot;
using System;
using System.Collections.Generic;

namespace Crashteroids
{
	[Serializable]
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
		public List<int> BoughtItems { get; set; } = new List<int>();
		//public List<int> EquippedItems = new List<int>();

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
	Laser = 0,
	Warp,
	ComingSoon2,
	ComingSoon3,
	ComingSoon4,
	ComingSoon5,
	ComingSoon6,
	ComingSoon7,
	ComingSoon8,
	ComingSoon9,
	ComingSoon10,
	ComingSoon11,
	ComingSoon12,
	ComingSoon13
}
