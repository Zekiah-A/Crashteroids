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
	
	private const string FileName = "GameConfigCrashteroids.json";
	
	public static async void Save()
	{  //Easier than assigning all variables independantly
		var _config = GameConfig.Instance; 
		
		//SERIALISATION
		var _options = new JsonSerializerOptions { WriteIndented = true };
		string _jsonConfig = JsonSerializer.Serialize(_config, _options);
		GD.Print(_jsonConfig);
		
		//FILE CREATIONN
		FileStream _createStream = System.IO.File.Create(System.IO.Path.Combine(_appDataFolder, FileName));
		await JsonSerializer.SerializeAsync(_createStream, _config, _options);
		_createStream.Dispose(); //make aync / use "use"
	}
	
	public static async void Load()
	{
		//DESERIALISATION
		string _stream = System.IO.File.ReadAllText(System.IO.Path.Combine(_appDataFolder, FileName));
		GameConfig _newConfig = JsonSerializer.Deserialize<GameConfig>(_stream);
		GD.Print(_newConfig);
		
		//APPLYING TO CLASS
		GameConfig.GenerateInstance(_newConfig);
		//TODO: Broadcast signal to all to update UI with new config.
	}
}
