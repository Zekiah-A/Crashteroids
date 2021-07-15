using Godot;
using Crashteroids;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

public class GameSaveData
{
	/*public static event EventHandler GameSaveDataUpdate;*/
		
	private static string _appDataFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
	private const string FileName = "GameConfigCrashteroids.json";

	public static async Task Save()
	{   //Easier than assigning all variables independantly
		var _config = GameConfig.Instance;

		//CONFIG
		var _options = new JsonSerializerOptions { WriteIndented = true };

		//FILE CREATION & SERIALISATION
		using (FileStream _createStream = System.IO.File.Create(System.IO.Path.Combine(_appDataFolder, FileName)))
		{
			await JsonSerializer.SerializeAsync(_createStream, _config, _options);
			_createStream.Close();
			_createStream.Dispose();
		}
	}

	public static void Load()
	{
		//DESERIALISATION
		string _stream = System.IO.File.ReadAllText(System.IO.Path.Combine(_appDataFolder, FileName));
		GameConfig _newConfig = JsonSerializer.Deserialize<GameConfig>(_stream);

		GD.Print(_newConfig);

		//APPLYING TO CLASS
		GameConfig.GenerateInstance(_newConfig);
		
		//~~TODO: action to all to update UI with new config.~~
		/*EventHandler _handler = GameSaveDataUpdate;*/
		/*_handler?.Invoke(null, null); //new GameSaveData -> "this" can't be used as static | null -> no args!*/
	}
}
