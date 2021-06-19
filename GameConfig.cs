using Godot;
using System;

public class GameConfig
{
	public static int GraphicsQualitySetting;
	
	public struct Match
	{
		public int Gamemode;
		public bool RandomMap;
		public bool SpecialAbilities;
		public int RocketBounces;
		public int Rounds;
	}
}
