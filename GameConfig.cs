using Godot;
using System;

public class GameConfig
{
	public static int GraphicsQualitySetting;
	public static bool Music = true;
	public static bool SoundEffects = true;
	
	public static int Gamemode;
	
	public struct Match
	{
		public static bool RandomMap;
		public static bool SpecialAbilities;
		public static int RocketBounces;
		public static int Rounds;
	}
}

public enum Gamemodes
{
	TwoPlayer = 1,
	AiPlayer,
	Multiplayer,
	Designer
}
