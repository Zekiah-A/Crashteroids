using Godot;
using Crashteroids;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

public class GameSaveData
{
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
					return System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
				case "android":
					return System.IO.Directory.GetCurrentDirectory();
				case "ios":
					return "null";
				default:
					return "null";
			}
		}
	}

	public static string _platform
	{
		get { return OS.GetName(); }
	}

	private const string FileName = "GameConfigCrashteroids.json";

	public static async Task Save()
	{
		try
		{
			var _config = GameConfig.Instance;
			//<summary>Configuration.</summary>
			var _options = new JsonSerializerOptions { WriteIndented = true };

			//<summary>File creation and serialisation.</summary>
			using (FileStream _createStream = System.IO.File.Create(System.IO.Path.Combine(_appDataFolder, FileName)))
			{
				await JsonSerializer.SerializeAsync(_createStream, _config, _options);
				_createStream.Close();
				_createStream.Dispose();
			}
		}
		catch
		{
			GD.PrintErr("Could not save data");
		}
	}

	public static bool Load() //won't always have a sender, needs managing
	{
		//<summary>Locating file & deserislisation.</summary>
		try
		{
			GD.Print(_appDataFolder);
			GD.Print(_platform);
			string _stream = System.IO.File.ReadAllText(System.IO.Path.Combine(_appDataFolder, FileName));
			GameConfig _newConfig = JsonSerializer.Deserialize<GameConfig>(_stream);
			GD.Print(_newConfig);

			//<summary>Applying to class</summary>
			GameConfig.GenerateInstance(_newConfig);
			//this won't work because , just need a signal :angry:
			return true;
		}
		catch (Exception exep)
		{
			GD.PrintErr($"Could not load data {exep}");
			return false;
		}
	}
}
