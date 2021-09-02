using Godot;
using System;

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

	[Export]
	public bool Equipped
	{
		get { return equipped; }
		set
		{
			equipped = value;
			EquippedChanged();
		}
	}

	private bool bought;
	private bool equipped;
	private StyleBoxFlat defaultStyle;
	private StyleBoxFlat boughtStyle;
	private StyleBoxFlat equippedStyle;
	private Label description;

	public override void _Ready()
	{
		//defaultStyle = GetNode<>
		//boughtStyle = GetNode<>
		//equippedStyle = GetNode<>
		description = GetNode<Label>("Description");
		//TODO: Utils.AddCommasToNumbers function
		description.Text = $"{Enum.GetName(typeof(EditorIds), Id)} - Buy £{Price}";
	}

	public void BoughtChanged()
	{
		if (bought)
			description.Text = "Bought";
		else
			description.Text = "Not bought";
	}

	public void EquippedChanged()
	{
		if (equipped)
			description.Text = "Equipped";
		else
			description.Text = "Bought";
	}
}
