using Godot;
using System;
using System.Xml;
using System.Net.Http;
using System.Collections.Generic;

public class SettingsUiManager : Panel
{
	private List<Node> checkboxSettings;
	private RichTextLabel usernameLabel;
	private Tween usernameTween;
	private HTTPRequest usernameHttpRequest;

	public override void _Ready()
	{
		///<summary>
		/// Music checkbox = 0, Sfx checkbox = 1, Tutorial enabled checkbox = 2, Advertisements enabled checkbox = 3
		/// (Extra settings - Do not need to be on the list): Low graphics = 5, Medium graphics = 6, High = 7, Sublime = 8
		///</summary>
		checkboxSettings = new List<Node>
		{
			GetNode("CentrePanel").GetNode("Checkbox").GetNode<Checkbox>("Checkbox"),
			GetNode("CentrePanel").GetNode("Checkbox2").GetNode<Checkbox>("Checkbox"),
			GetNode("RightPanel").GetNode("Checkbox").GetNode<Checkbox>("Checkbox"),
			GetNode("RightPanel").GetNode("Checkbox2").GetNode<Checkbox>("Checkbox"),
		};

		//GetNode("RightPanel").GetNode<LineEdit>("UsernameEdit");
		usernameLabel = GetNode("RightPanel").GetNode("UsernameEdit").GetNode<RichTextLabel>("UsernameLabel");
		usernameTween = GetNode("RightPanel").GetNode("UsernameEdit").GetNode<Tween>("Tween");
		usernameHttpRequest = GetNode("RightPanel").GetNode("UsernameEdit").GetNode<HTTPRequest>("HTTPRequest");
	}

	public void Opened()
	{
		GetNode("RightPanel").GetNode<LineEdit>("UsernameEdit").PlaceholderText = GameData.Username;
		(checkboxSettings[0] as Checkbox).IsEnabled = GameData.Music;
		(checkboxSettings[1] as Checkbox).IsEnabled = GameData.SoundEffects;
		(checkboxSettings[2] as Checkbox).IsEnabled = GameData.TutorialEnabled;
		(checkboxSettings[3] as Checkbox).IsEnabled = GameData.Advertisements;
	}

	///<summary> Called by a signal when a setting is interacted with. </summary>
	private void SettingsChanged(int id)
	{
		(checkboxSettings[id] as Checkbox).IsEnabled = ((checkboxSettings[id] as Checkbox).IsEnabled ? false : true);
		switch (id)
		{
			case 0:
				GameData.Music = (checkboxSettings[id] as Checkbox).IsEnabled;
				break;
			case 1:
				GameData.SoundEffects = (checkboxSettings[id] as Checkbox).IsEnabled;
				break;
			case 2:
				GameData.TutorialEnabled = (checkboxSettings[id] as Checkbox).IsEnabled;
				break;
			case 3:
				GameData.Advertisements = (checkboxSettings[id] as Checkbox).IsEnabled;
				break;
		}
	}

	private async void UsernameEdited(string newText)
	{
		using (HttpClient client = new HttpClient()) //TODO: May have to use GODOT HTTP service
		{
			try
			{
				///<summary> Send a request to moderate the username using the purgomalum API. </summary>
				string responseBody = await client.GetStringAsync($"https://www.purgomalum.com/service/xml?text={newText}");

				XmlDocument document = new XmlDocument();
				document.LoadXml(responseBody);

				///<summary> Get the XML tag containing the filtered username. </summary>
				string filteredName = document.GetElementsByTagName("result")[0].InnerText;

				///<summary> If a change in the new username is found, set it to that, else, keep the username the same.</summary>
				if (filteredName != newText)
					newText = filteredName;
			}
			catch(Exception e)
			{
				if (e is HttpRequestException)
					GD.Print($"Could not access API to request username filter. {e}\nAccepting unfiltered username.");
				else
					GD.Print($"Error requesting username filter. {e}\nAccepting unfiltered username.");
			}
		}

		if (newText.Length <= Constants.MaxUsernameLength)
			GameData.Username = newText;
		else
			GameData.Username = newText.Substring(0, Constants.MaxUsernameLength);

		usernameLabel.Visible = true;
		usernameTween.InterpolateProperty(
			usernameLabel, //Object
			"rect_position", //Property being tweened
			new Vector2(0, 32), //from
			new Vector2(0, 64), //to
			0.5f, //speed
			Tween.TransitionType.Cubic,
			Tween.EaseType.Out
		);
		usernameTween.Start();

		usernameLabel.BbcodeText = $"[color=yellow]Username set to: {GameData.Username}[/color]";

	}
}
