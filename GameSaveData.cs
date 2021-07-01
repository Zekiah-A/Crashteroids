using Godot;
using Crashteroids;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

public class GameSaveData
{
	private static string _appDataFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
 
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
		
		string _fileName = "GameConfig.json";
		FileStream _createStream = System.IO.File.Create(System.IO.Path.Combine(_appDataFolder, _fileName));
		await JsonSerializer.SerializeAsync(_createStream, _config, _options);
		//await _createStream.DisposeAsync();
		
		GameConfig _newConfig = JsonSerializer.Deserialize<GameConfig>(_jsonConfig);
		GD.Print(_newConfig);
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
