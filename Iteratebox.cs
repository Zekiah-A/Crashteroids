using Godot;
using System;

public class Iteratebox : Control
{
	public int Current = 1;
	[Export] public int Min = 0;
	[Export] public int Max = 5;

	private TextureButton button;
	private Label number;

	public override void _Ready()
	{
		button = GetNode<TextureButton>("Texture Button");
		number = GetNode("Texture Button").GetNode<Label>("Number");

		number.Text = Current.ToString();
	}

	private void _on_Click()
	{
		if (Current < Max)
			Current++;
		else
			Current = 1;

		number.Text = Current.ToString();
	}
}
