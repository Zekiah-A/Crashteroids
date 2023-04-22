using Godot;
using System.Collections.Generic;

public static class Config
{
	private static Dictionary<string, object> defaultConfig = new()
	{
		{ "music", true },
		{ "sfx",  true },
		{ "tutorial", true },
		{ "adverts", false },
		{ "money", 0 },
		{ "graphics_quality", 3 },
		{ "name", "Player" },
		{ "bought_tools", System.Array.Empty<string>() }
	};

	public static void Initialise()
	{
		var config = new ConfigFile();
		var error = config.Load("user://crashteroids.cfg");
		if (error == Error.Ok)
		{
			return;
		}
		foreach (var configPair in defaultConfig) config.SetValue("settings", configPair.Key, (Variant) configPair.Value);
		config.Save("user://crashteroids.cfg");
	}
	
	public static void Save<[MustBeVariant] T>(string key, T value)
	{
		var config = new ConfigFile();
		config.Load("user://crashteroids.cfg");
		config.SetValue("settings", key, Variant.From(value));
		config.Save("user://crashteroids.cfg");
	}

	public static T? Load<[MustBeVariant] T>(string key)
	{
		var config = new ConfigFile();
		var error = config.Load("user://crashteroids.cfg");
		if (error != Error.Ok)
		{
			return (T?) (object?) null;
		}
		
		return config.GetValue("settings", key).As<T>();
	}
}
