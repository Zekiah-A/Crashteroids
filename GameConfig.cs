using Godot;
using System;

namespace Crashteroids
{
	public class GameConfig
	{
		public static GameConfig Instance = new GameConfig();
		
		public int GraphicsQualitySetting { get; set; }
		public bool Music { get; set; }
		public bool SoundEffects { get; set; }
		public int SkinID { get; set; }
		
		public static int Gamemode { get; set; }
		public struct Match
		{
			public static bool RandomMap { get; set; }
			public static bool SpecialAbilities { get; set; }
			public static int RocketBounces { get; set; }
			public static int Rounds { get; set; }
		}
		
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
