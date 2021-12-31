using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class GameData
{
	public delegate void ConfigEventHandler(object sender, ConfigChangeEventArgs eventArgs);
	public static event ConfigEventHandler ConfigSaveEvent;
	public static event ConfigEventHandler ConfigLoadEvent;

	//Runtime Game Data (not saved to config)
	//put things like bounces for match here
	//skin should be chosen at start match time, it should be able to be bought in the shop.
	public struct TwoPlayerMatchConfiguration
	{
		public static bool RandomMap { get; set; }
		public static bool SpecialAbilities { get; set; }
		public static int RocketBounces { get; set; }
		public static int Rounds { get; set; }
		public static int MatchMoney { get; set; }
	}

	//Settings Data (saved to config)
	private static bool music; //Inward facing, actively saved and loaded, save and load function only acesses this.
	public static bool Music //Outwardly facing, other classes access and change the private variable through this.
	{
		get
		{
			Load(ref music, nameof(music));
			return music;
		}
		set
		{
			music = value;
			Save(ref music, nameof(music));
		}
	}

	private static bool sfx;
	public static bool SoundEffects
	{
		get
		{
			Load(ref sfx, nameof(sfx));
			return sfx;
		}
		set
		{
			sfx = value;
			Save(ref sfx, nameof(sfx));
		}
	}

	private static bool tutorial;
	public static bool TutorialEnabled
	{
		get
		{
			Load(ref tutorial, nameof(tutorial));
			return tutorial;
		}
		set
		{
			tutorial = value;
			Save(ref tutorial, nameof(tutorial));
		}
	}

	private static bool adverts;
	public static bool Advertisements
	{
		get
		{
			Load(ref adverts, nameof(adverts));
			return adverts;
		}
		set
		{
			adverts = value;
			Save(ref adverts, nameof(adverts));
		}
	}

	private static int money;
	public static int Money
	{
		get
		{
			Load(ref money, nameof(money));
			return money;
		}
		set
		{
			money = value;
			Save(ref money, nameof(money));
		}
	}

	private static int graphicsQuality;
	public static int GraphicsQualitySetting
	{
		get
		{
			Load(ref graphicsQuality, nameof(graphicsQuality));
			return graphicsQuality;
		}
		set
		{
			graphicsQuality = value;
			Save(ref graphicsQuality, nameof(graphicsQuality));
		}
	}

	private static string name;
	public static string Username
	{
		get
		{
			Load(ref name, nameof(name));
			return name;
		}
		set
		{
			name = value;
			Save(ref name, nameof(name));
		}
	}

	private static List<string> boughtTools;

	public static List<string> BoughtTools
	{
		get
		{
			Load(ref boughtTools, nameof(boughtTools));
			return boughtTools;
		}
		set
		{
			boughtTools = value;
			Save(ref boughtTools, nameof(boughtTools));
		}
	}


	//Called when any of the settings are changed, should never be called by functions outside of this class.
	private static void Save<T>(ref T setting, string key)
	{
		var config = new ConfigFile();
		var error = config.Load("user://game_config_crashteroids.cfg");
		if (error is Error.FileNotFound) //If file not found, make file, and fix the error by itself.
		{
			config.Save("user://game_config_crashteroids.cfg"); //This is in ~/.local/share/Crashteroids on linux
			error = config.Load("user://game_config_crashteroids.cfg");
		}
		if (error is Error.Ok)
		{
			config.SetValue("SETTINGS", key, setting); //Section, Key, Value
			config.Save("user://game_config_crashteroids.cfg");
			ConfigSaveEvent?.Invoke(null, new ConfigChangeEventArgs(key, setting));
		}
		else
			GD.PrintErr($"Error loading game config: {error}");
	}

	//Called when any of the settings are accessed, should never be called by functions outside of this class.
	private static void Load<T>(ref T setting, string key)
	{
		var config = new ConfigFile();
		var error = config.Load("user://game_config_crashteroids.cfg");
		if (error is Error.FileNotFound)
			GD.PushWarning($"Could not find game config {error}.");
		if (error is Error.Ok)
		{
			if (config.HasSectionKey("SETTINGS", key))
			{
				setting = (T) config.GetValue("SETTINGS", key);
				ConfigLoadEvent?.Invoke(null, new ConfigChangeEventArgs(key, setting));
			}
			else
				GD.PrintErr($"Error loading game config, could not get section \"SETTINGS\"/ or value: {setting}");
		}
		else
			GD.PrintErr($"Error loading game config: {error}");
	}
}

//TODO: Move to it's own location
// Sending out the name and key is more efficient than reflection.
public class ConfigChangeEventArgs
{
	public ConfigChangeEventArgs(string name, object config)
	{
		ConfigName = name;
		Config = config;
	}
	
	public string ConfigName { get; }
	public object Config { get; }
}
