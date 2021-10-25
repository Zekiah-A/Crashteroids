using Godot;
using System;

public class Tool : Panel
{
/*
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
*/
	private StyleBoxFlat defaultStyle;
	private StyleBoxFlat boughtStyle;
	private Label description;

	public override void _Ready()
	{
		defaultStyle = ResourceLoader.Load("res://styles/editoritem_available.tres") as StyleBoxFlat;
		boughtStyle = ResourceLoader.Load("res://styles/editoritem_bought.tres") as StyleBoxFlat;

		description = GetNode<Label>("Description");
    }

	public void Buy()
	{
		//if (bought)
		//{
			description.Text = "Bought";
			description.AddStyleboxOverride("normal", boughtStyle);
			GetNode<AnimationPlayer>("AnimationPlayer").Play("bought_anim");
		//}
	}
}
