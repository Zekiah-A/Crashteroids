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
		//TODO: Does not work on android
	//"Android", "BlackBerry 10", "Flash", "Haiku", "iOS", "HTML5", "OSX", "Server", "Windows", "WinRT", "X11"-
	private static string _appDataFolder
	{
		get
		{
			switch (_platform.ToLower())
			{
				case "x11":
				case "osx":
				case "winrt":
				case "windows":
				//case "linux":
					return System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
					break;
				case "android":
					return Directory.GetCurrentDirectory();
					break;
				case "ios":
					return null;
					break;
			}
		}	
	}
	private const string FileName = "GameConfigCrashteroids.json";
	public static string _platform
	{
		get
		{
			return OS.GetName();
		}	
	}

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
		try {
			//switch (_platform.ToLower())
			//{
			//	case "x11":
			//	case "android":
			//		_appDataFolder = Directory.GetCurrentDirectory();
			//		GD.Print(_appDataFolder); //^ linux home/zek..i
			//		break;
			//	default:
			//		break;
			//}
			GD>Print(_appDataFolder);
			GD.Print(_platform);
			string _stream = System.IO.File.ReadAllText(System.IO.Path.Combine(_appDataFolder, FileName));
			GameConfig _newConfig = JsonSerializer.Deserialize<GameConfig>(_stream);

			GD.Print(_newConfig);

			//APPLYING TO CLASS
			GameConfig.GenerateInstance(_newConfig);
		}
		catch (Exception exep)
		{
			GD.Print($"Could not load data {exep}");
		}
		//~~TODO: action to all to update UI with new config.~~
		/*EventHandler _handler = GameSaveDataUpdate;*/
		/*_handler?.Invoke(null, null); //new GameSaveData -> "this" can't be used as static | null -> no args!*/
	}
}
