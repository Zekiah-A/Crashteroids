using Godot;
using System;
using System.Collections.Generic;

public class ShopUiManager : Panel
{
	//TODO: Make public in config?
	private Dictionary<ToolTypes, Tool> upgrades;

	public override void _Ready()
	{
		upgrades = new Dictionary<ToolTypes, Tool>()
		{
			{ ToolTypes.Pen, GetNode("Main Panel").GetNode("Items Panel").GetNode("1") as Tool },
			{ ToolTypes.Potractor, GetNode("Main Panel").GetNode("Items Panel").GetNode("2") as Tool },
			{ ToolTypes.Ruler, GetNode("Main Panel").GetNode("Items Panel").GetNode("3") as Tool },
			{ ToolTypes.Laser, GetNode("Main Panel").GetNode("Items Panel").GetNode("4") as Tool }
		};
	}

	private void UpgradePressed(int selected)
	{
		(upgrades[(ToolTypes)selected] as Tool).Buy();
		//TODO: Save bought to config & Deduct money.
	}
}

//TODO: Move to it's own file?
enum ToolTypes
{
	Pen,
	Potractor,
	Ruler,
	Laser
}
