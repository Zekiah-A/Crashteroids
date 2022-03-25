using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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
		
		//If they already have bought it, display as bought.
		foreach (var tool in tools)
		{
			GD.Print(tool.Key.ToString().ToLower());
			try
			{
				GD.Print(GameData.BoughtTools.ToString());
				var toolname = tools[ToolTypes.Pen];
				GD.Print(toolname);
			}
			catch (Exception e)
			{
				GD.Print(e);
			}

			if (GameData.BoughtTools.Contains(tool.Key.ToString()))
				tool.Value.Buy();
		}
	}

	private void UpgradePressed(int selected)
	{
		if (!tools[(ToolTypes) selected].Bought)
		{
			if (GameData.Money > tools[(ToolTypes) selected].Price)
			{
				tools[(ToolTypes) selected].Buy();
				GameData.Money -= tools[(ToolTypes) selected].Price;
				//Hack to append the bought tool to the end of the bought tools array
				GameData.BoughtTools = GameData.BoughtTools.ToList().Append(((ToolTypes) selected).ToString()).ToArray();
				
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
