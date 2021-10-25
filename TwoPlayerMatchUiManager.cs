using Godot;
using System;
using System.Collections.Generic;

public class TwoPlayerUiManager : Panel
{
	private List<Node> matchSettings;
	public override void _Ready()
	{
		///<summary> Random map = 1, Special abilities = 2, Rocket bounces = 3, Rounds = 4 </summary>
		matchSettings = new List<Node>
		{
			GetNode("LeftPanel").GetNode("Checkbox").GetNode<Checkbox>("Checkbox"),
			GetNode("LeftPanel").GetNode("Checkbox2").GetNode<Checkbox>("Checkbox2"),
			GetNode("LeftPanel").GetNode("Checkbox").GetNode<Iteratebox>("Iteratebox"),
			GetNode("LeftPanel").GetNode("Checkbox2").GetNode<Iteratebox>("Iteratebox2"),
			//GetNode("CentrePanel").GetNode<Picker>("MapPicker"),
			//GetNode("RightPanel").GetNode<Picker>("PlayerOnePicker"),
			//GetNode("RightPanel").GetNode<Picker>("PlayerTwoPicker"),
		};
	}

	private void MatchSettingsChanged()
	{

	}
}
