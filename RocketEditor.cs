using Godot;
using System;
using Crashteroids;
using System.Collections;
using System.Collections.Generic;

public class RocketEditor : Panel
{
	public override void _Ready()
	{
		UpdateEditorItems();
	}

	public virtual async void UpdateEditorItems() //EDIT: WHY IS THIS a separarte function?, COMBINE with the itemclick function
	{
		foreach (Node node in GetChildren())
		{
			if (node is EditorItem item)
			{
				if (!GameConfig.Instance.BoughtItems.Contains(item.Id))
				{
					if (item.Bought)
						GameConfig.Instance.BoughtItems.Add(item.Id);
					else
						GameConfig.Instance.BoughtItems.Remove(item.Id);

					if (item.Equipped)
						GameConfig.Instance.EquippedItems.Add(item.Id);
					else
						GameConfig.Instance.EquippedItems.Remove(item.Id);
				}

				GD.Print(
					$"Item {item.Id} ({Enum.GetName(typeof(EditorIds), item.Id)}) added, with bought: {item.Bought} and enabled: {item.Equipped} | in BoughtItems: {GameConfig.Instance.BoughtItems.Contains(item.Id)} and in EquippedItems: {GameConfig.Instance.EquippedItems.Contains(item.Id)}");
			}
		}
		if (GameSaveData.Load())
			GameSaveData.Save();
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
					if (item.Bought == true)
					{
						if (item.Equipped)
							item.Equipped = false;
						else
							item.Equipped = true;
					}
					else
					{
						item.Bought = true;
						GameConfig.Instance.Money -= item.Price;
					}
				}
			}
		}

		//ineffifient, especially this foreach!
		UpdateEditorItems();
	}
}
