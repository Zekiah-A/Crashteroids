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
			Load(ref music);
			return music;
		}
		set
		{
			music = value;
			Save(ref music);
		}
	}

	private static bool sfx;
	public static bool SoundEffects
	{
		get
		{
			Load(ref sfx);
			return sfx;
		}
		set
		{
			sfx = value;
			Save(ref sfx);
		}
	}

	private static bool adverts;
	public static bool Advertisements
	{
		get
		{
			Load(ref adverts);
			return adverts;
		}
		set
		{
			adverts = value;
			Save(ref adverts);
		}
	}

	private static int money;
	public static int Money
	{
		get
		{
			Load(ref money);
			return money;
		}
		set
		{
			money = value;
			Save(ref money);
		}
	}

	private static int graphicsQuality;
	public static int GraphicsQualitySetting
	{
		get
		{
			Load(ref graphicsQuality);
			return graphicsQuality;
		}
		set
		{
			graphicsQuality = value;
			Save(ref graphicsQuality);
		}
	}
	
	private static string name;
	public static string Username
	{
		get
		{
			Load(ref name);
			return name;
		}
		set
		{
			name = value;
			Save(ref name);
		}
	}



	//Called when any of the settings are changed, should never be called by functions outside of this class.
	private static void Save<T>(ref T setting)
	{
		var config = new ConfigFile();
		var error = config.Load("user://game_config_crashteroids.cfg");
		if (error == Error.FileNotFound) //If file not found, make file, and fix the error by itself.
		{   // This is in ~/.local/share/Crashteroids on linux
			config.Save("user://game_config_crashteroids.cfg");
			error = config.Load("user://game_config_crashteroids.cfg");
		}
		if (error == Error.Ok)
		{
			config.SetValue(nameof(setting).ToUpper(), nameof(setting), setting); //Section, Key, Value
			config.Save("user://game_config_crashteroids.cfg");
		}
		GD.Print($"Sucessfully saved config, with value {nameof(setting)}, {setting}, with Error status: {error}.");
	}

	//Called when any of the settings are acessed, should never be called by functions outside of this class.
	public static void Load<T>(ref T setting)
	{
		var config = new ConfigFile();
		var error = config.Load("user://game_config_crashteroids.cfg");
		if (error == Error.FileNotFound)
			GD.PushWarning("Could not find game config.");
		if (error == Error.Ok)
		{
			GD.Print(config.GetValue(nameof(setting).ToUpper(), nameof(setting))); //THE NAME WILL ALWAYS BE "SETTING, since that is what it's caled in this func", THAT is what causes the error
			setting = (T) config.GetValue(nameof(setting).ToUpper(), nameof(setting));
		}
		GD.Print($"Sucessfully loaded config, with value {nameof(setting)}, {setting}, with Error status: {error}.");
	}
}
