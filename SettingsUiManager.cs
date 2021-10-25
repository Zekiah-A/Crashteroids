using Godot;
using System;
using System.Xml;
using System.Net.Http;
using System.Collections.Generic;

public class SettingsUiManager : Panel
{
	private List<Node> settings;
	private RichTextLabel usernameLabel;

	public override void _Ready()
	{
		///<summary>
		/// Music checkbox = 0, Sfx checkbox = 1, Tutorial enabled checkbox = 2, Advertisements enabled checkbox = 3, Username edit = 4,
		/// (Extra settings - Do not need to be on the list): Low graphics = 5, Medium graphics = 6, High = 7, Sublime = 8 
		///</summary>
		settings = new List<Node>
		{
			GetNode("CentrePanel").GetNode("Checkbox").GetNode<Checkbox>("Checkbox"),
			GetNode("CentrePanel").GetNode("Checkbox2").GetNode<Checkbox>("Checkbox"),
			GetNode("RightPanel").GetNode("Checkbox").GetNode<Checkbox>("Checkbox"),
			GetNode("RightPanel").GetNode("Checkbox2").GetNode<Checkbox>("Checkbox"),
			GetNode("RightPanel").GetNode<LineEdit>("UsernameEdit"),
		};

		usernameLabel = GetNode("RightPanel").GetNode("UsernameEdit").GetNode<RichTextLabel>("UsernameLabel");
	}

	///<summary> Called by a signal when a setting is interacted with. </summary>
	private void SettingsChanged(int id)
	{
		
		if (id < 4)
		{
			(settings[id] as Checkbox).IsEnabled = ((settings[id] as Checkbox).IsEnabled ? false : true);
		}
		else if (id == 4)
		{
			string newText = "new username goes here";

			using (HttpClient client = new HttpClient())
			{
				try 
				{
					///<summary> Send a request to moderate the username using the purgomalum API. </summary>
					string responseBody = ""; //= await client.GetStringAsync($"https://www.purgomalum.com/service/xml?text={newText}");

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
						GD.Print($"Could not acess API to request username filter. {e}\nAccepting unfiltered username.");
					else
						GD.Print($"Error requesting username filter. {e}\nAccepting unfiltered username.");
				}
			}
/*
			if (newText.Length <= Constants.MaxUsernameLength)
				GameConfig.Instance.Username = newText;
			else
				GameConfig.Instance.Username = newText.Substring(0, Constants.MaxUsernameLength);

			usernameLabel.Visible = true;
			usernameTween.InterpolateProperty(
				_usernameLabel, //Object
				"rect_position", //Property being tweened
				new Vector2(0, 32), //from
				new Vector2(0, 64), //to
				0.5f, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			usernameTween.Start();

			usernameLabel.BbcodeText = $"[color=yellow]Username set to: {GameConfig.Instance.Username}[/color]";
*/
		}
		else if (id == 5)
		{
			
		}
		else if (id == 6)
		{

		}
		else if (id ==  7)
		{

		}
		else if (id == 8)
		{
			
		}
	}

}
