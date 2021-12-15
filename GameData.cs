using Godot;
using System;
using System.Text.Json;

public static class GameData
{
	//Runtime Game Data (not saved to config)
	//put things like bounces for match here
	//skin should be chosen at start match time, it should be able to be bought in the shop.

	//Settings Data (saved to config)
	private static bool music; //Inward facing, actively saved and loaded, sace and load function only acesses this.
	public static bool Music //Outwardly facing, other classes acess and change the private variable through this.
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



	//Called when any of the settings are changed, should never be called by functions outside of this class.
	private static void Save<T>(ref T setting, string key) //TODO: temporary hack until i find a better way of getting var name from ref for the config KEY (string key arg) -- using nameof for this is stupid waste of resources but i cba not
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
			GD.Print($"Successfully saved config, with value {setting}, {key}, with Error status: {error}.");
		}
		else
			GD.PrintErr($"Error loading game config: {error}");
	}

	//Called when any of the settings are accessed, should never be called by functions outside of this class.
	private static void Load<T>(ref T setting, string key) //TODO: temporary hack until i find a better way of getting var name from ref for the config KEY (string key arg) -- using nameof for this is stupid waste of resources but i cba not
	{
		var config = new ConfigFile();
		var error = config.Load("user://game_config_crashteroids.cfg");
		if (error is Error.FileNotFound)
			GD.PushWarning($"Could not find game config {error}.");
		if (error is Error.Ok)
		{
			setting = (T)config.GetValue("SETTINGS", key);
			GD.Print($"Successfully loaded config, with value {setting}, {key}, with Error status: {error}.");
		}
		else
			GD.PrintErr($"Error loading game config: {error}");
	}
}
