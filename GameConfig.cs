using Godot;
using System;

public class GameConfig
{
	public static int GraphicsQualitySetting;
	public static int Gamemode;

	public struct Match
	{
		public static bool RandomMap;
		public static bool SpecialAbilities;
		public static int RocketBounces;
		public static int Rounds;
	}
}
