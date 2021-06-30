using Godot;
using Crashteroids;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

public class GameSaveData
{
	//public GameConfig Config = new GameConfig();

	public static async void Test()
	{
		var _config = new GameConfig
		{
			GraphicsQualitySetting = GameConfig.Instance.GraphicsQualitySetting,
			Music = GameConfig.Instance.Music,
			SoundEffects = GameConfig.Instance.SoundEffects,
			SkinID = GameConfig.Instance.SkinID
		};
		
		var _options = new JsonSerializerOptions { WriteIndented = true };
		string _jsonConfig = JsonSerializer.Serialize(_config, _options);
		GD.Print(_jsonConfig);
		
		string fileName = "GameConfig.json";
		FileStream createStream = System.IO.File.Create(fileName);
		await JsonSerializer.SerializeAsync(createStream, _config, _options);
		//await createStream.DisposeAsync();
		
		GameConfig weatherForecast = JsonSerializer.Deserialize<GameConfig>(_jsonConfig);
		GD.Print(weatherForecast);
	}
/*
	public async Task WriteData()
	{
		return;
	}
	
	public async Task ReadData()
	{
		return;
	}
	public Task<string>GetData()
	{
		return null;
	}
*/
}
