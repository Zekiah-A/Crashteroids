using Godot;
using System;
using System.Collections.Generic;

public class ShopUiManager : Panel
{
	//TODO: Make public in config?
	private Dictionary<ToolTypes, Tool> tools;

	public override void _Ready()
	{
		tools = new Dictionary<ToolTypes, Tool>()
		{
			{ ToolTypes.Pen, GetNode("MainPanel").GetNode("ToolsGrid").GetNode("1") as Tool },
			{ ToolTypes.Potractor, GetNode("MainPanel").GetNode("ToolsGrid").GetNode("2") as Tool },
			{ ToolTypes.Ruler, GetNode("MainPanel").GetNode("ToolsGrid").GetNode("3") as Tool },
			{ ToolTypes.Laser, GetNode("MainPanel").GetNode("ToolsGrid").GetNode("4") as Tool }
		};
	}

	private void UpgradePressed(int selected)
	{
		(tools[(ToolTypes)selected] as Tool).Buy();
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
