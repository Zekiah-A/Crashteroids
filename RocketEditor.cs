using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public class RocketEditor : Panel
{
	//add on initialisation

	//disctionary / list <int, panel>
	public override void _Ready()
	{

	}

	private void _on_Item_click(int _selected)
	{
		GD.Print($"Inventory item panel {_selected} selected.");
	}
}
