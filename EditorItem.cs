using Godot;
using System;
using Crashteroids;

public class EditorItem : Node
{
	[Export] public int Id = 0;
	[Export] public int Price = 0;

	[Export]
	public bool Bought
	{
		get { return bought; }
		set
		{
			bought = value;
			BoughtChanged();
		}
	}

	private bool bought;
	private StyleBoxFlat defaultStyle;
	private StyleBoxFlat boughtStyle;
	/*private StyleBoxFlat equippedStyle;*/
	private Label description;

	public override void _Ready()
	{
		defaultStyle = ResourceLoader.Load("res://styles/editoritem_available.tres") as StyleBoxFlat;
		boughtStyle = ResourceLoader.Load("res://styles/editoritem_bought.tres") as StyleBoxFlat;
		/*equippedStyle = ResourceLoader.Load("res://styles/editoritem_equipped.tres") as StyleBoxFlat;*/

		description = GetNode<Label>("Description");
		//TODO: Utils.AddCommasToNumbers function
		description.Text = $"{Enum.GetName(typeof(EditorIds), Id)} - Buy £{Price}";
	}

	public void BoughtChanged()
	{
		if (bought)
		{
			description.Text = "Bought";
			description.AddStyleboxOverride("normal", boughtStyle);
			//play bought animation (maybe make a fun gloss shader for these as well when item is available)
			
		}
	}
/*
	public void EquippedChanged()
	{
		if (equipped)
		{
			description.Text = "Equipped";
			description.AddStyleboxOverride("normal", equippedStyle);
		}
		else
		{
			description.Text = "Bought";
			description.AddStyleboxOverride("normal", boughtStyle);
		}
	}
*/
}
