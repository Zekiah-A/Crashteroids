using Godot;
using System;
using System.Collections.Generic;

public class ShopUiManager : Panel
{
	//TODO: Make public in config?
	private List<Tools> upgrades;

	public override void _Ready()
	{
		
	}

	private void OnItemClick(int selected)
	{

	}
}

//TODO: Move to it's own file!
enum Tools
{
	Pen,
	Potractor,
	Ruler,
	Laser
}