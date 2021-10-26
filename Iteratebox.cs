using Godot;
using System;

public class Iteratebox : Control
{
	public int Current;
	[Export] public int Min = 1;
	[Export] public int Max = 5;

	private TextureButton button;
	private Label number;

	public override void _Ready()
	{
		button = GetNode<TextureButton>("Texture Button");
		number = GetNode("Texture Button").GetNode<Label>("Number");
		
		Current = Min;
		number.Text = Current.ToString();
	}

	public void Increment()
	{
		if (Current < Max)
			Current++;
		else
			Current = Min;

		number.Text = Current.ToString();
	}
}
