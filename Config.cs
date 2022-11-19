using Godot;
using System.Collections.Generic;

public static class Config
{
	private static Dictionary<string, object> defaultConfig = new Dictionary<string, object>()
	{
		{ "music", true },
		{ "sfx",  true },
		{ "tutorial", true },
		{ "adverts", false },
		{ "money", 0 },
		{ "graphics_quality", 3 },
		{ "name", "morbius" },
		{ "bought_tools", new[] {""} }
	};

	public static void Initialise()
	{
		var config = new ConfigFile();
		var err = config.Load("user://crashteroids.cfg");
		if (err == Error.Ok) return;
		foreach (var configPair in defaultConfig) config.SetValue("settings", configPair.Key, (Variant) configPair.Value);
		config.Save("user://crashteroids.cfg");
	}
	
	public static void Save(string key, object value)
	{
		var config = new ConfigFile();
		config.Load("user://crashteroids.cfg");
		config.SetValue("settings", key, (Variant) value);
		config.Save("user://crashteroids.cfg");
	}

	public static object Load(string key)
	{
		var config = new ConfigFile();
		var err = config.Load("user://crashteroids.cfg");
		if (err != Error.Ok) return null;
		return config.GetValue("settings", key);
	}
}
