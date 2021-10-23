using Godot;
using System;
using Crashteroids;
using System.Collections;
using System.Collections.Generic;

public class RocketEditor : Panel
{
	public override void _Ready()
	{
		//TODO: Can't update editor items on start, because of some stupid exception due to saving and loading at the same time
		//UpdateEditorItems();
	}

	public async void UpdateEditorItems() //EDIT: WHY IS THIS a separarte function?, COMBINE with the itemclick function
	{
		if (GameSaveData.Load())//jankest thing ever, must do for now!
		{
			foreach (Node node in GetChildren())
			{
				if (node is EditorItem item)
				{
					if (!GameConfig.Instance.BoughtItems.Contains(item.Id))
					{
						if (item.Bought)
						{	
							GameConfig.Instance.BoughtItems.Add(item.Id);
						}
						else
							GameConfig.Instance.BoughtItems.Remove(item.Id);
					}

					GD.Print($"Item {item.Id} ({Enum.GetName(typeof(EditorIds), item.Id)}) added, with bought: {item.Bought} | in BoughtItems: {GameConfig.Instance.BoughtItems.Contains(item.Id)}");
				}
			}
			await GameSaveData.Save();
		}
	}

	private void OnItemClick(int selected)
	{
		GD.Print($"Inventory item panel {selected} selected.");

		foreach (Node node in GetChildren())
		{
			if (node is EditorItem item)
			{
				if (item.Id == selected)
				{
					item.Bought = true;
					GameConfig.Instance.Money -= item.Price;
				}
			}
		}

		//TODO: ineffifient and stupid, especially this foreach, just add it!
		UpdateEditorItems();
	}
}
