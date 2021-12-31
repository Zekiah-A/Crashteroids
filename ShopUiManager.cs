using Godot;
using System;
using System.Collections.Generic;

public class ShopUiManager : Panel
{
	//TODO: Make public in config?
	private Dictionary<ToolTypes, Tool> tools;

	public override void _Ready()
	{
		tools = new Dictionary<ToolTypes, Tool>
		{
			{ ToolTypes.Pen, GetNode("MainPanel").GetNode("ToolsGrid").GetNode("1") as Tool },
			{ ToolTypes.Potractor, GetNode("MainPanel").GetNode("ToolsGrid").GetNode("2") as Tool },
			{ ToolTypes.Ruler, GetNode("MainPanel").GetNode("ToolsGrid").GetNode("3") as Tool },
			{ ToolTypes.Laser, GetNode("MainPanel").GetNode("ToolsGrid").GetNode("4") as Tool }
		};
		
		//TODO: Initialise money, initialise all vars with default values when creating the config so no errors occur due to fields not existing
	}

	private void UpgradePressed(int selected)
	{
		if (!tools[(ToolTypes) selected].Bought)
		{
			if (GameData.Money > tools[(ToolTypes) selected].Price)
			{
				tools[(ToolTypes) selected].Buy();
				GameData.Money -= tools[(ToolTypes) selected].Price;

				GameData.BoughtTools = new List<string>()
				{
					((ToolTypes) selected).ToString()
				};
				
				GD.Print($"Item {tools[(ToolTypes) selected].Name} bought. Old balance: {tools[(ToolTypes) selected].Price + GameData.Money}, current balance: {GameData.Money}, price: {tools[(ToolTypes) selected].Price}");
			}
		}
		
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
