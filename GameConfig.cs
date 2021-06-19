using Godot;
using System;

public class GameConfig
{
	public static int GraphicsQualitySetting;
	public static int Gamemode;

	public struct Match
	{
		public bool RandomMap;
		public bool SpecialAbilities;
		public int RocketBounces;
		public int Rounds;
	}
}
