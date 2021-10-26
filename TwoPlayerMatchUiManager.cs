using Godot;
using System;
using System.Collections.Generic;

public class TwoPlayerMatchUiManager : Panel
{
	private List<Node> matchSettings;
	public override void _Ready()
	{
		///<summary> Random map = 1, Special abilities = 2, Rocket bounces = 3, Rounds = 4 </summary>
		matchSettings = new List<Node>
		{
			GetNode("LeftPanel").GetNode("Checkbox").GetNode<Checkbox>("Checkbox"),
			GetNode("LeftPanel").GetNode("Checkbox2").GetNode<Checkbox>("Checkbox"),
			GetNode("LeftPanel").GetNode("Iteratebox").GetNode<Iteratebox>("Iteratebox"),
			GetNode("LeftPanel").GetNode("Iteratebox2").GetNode<Iteratebox>("Iteratebox"),
			//GetNode("CentrePanel").GetNode<Picker>("MapPicker"),
			//GetNode("RightPanel").GetNode<Picker>("PlayerOnePicker"),
			//GetNode("RightPanel").GetNode<Picker>("PlayerTwoPicker"),
		};
	}

	private void MatchSettingsChanged(int id)
	{
		if (id < 2)
			(matchSettings[id] as Checkbox).IsEnabled = ((matchSettings[id] as Checkbox).IsEnabled ? false : true);
		else
			(matchSettings[id] as Iteratebox).Increment();
	}
}
