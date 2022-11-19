using Godot;
using System;

public partial class Iteratebox : Control
{
	public int Current;
	[Export] public int Min = 1;
	[Export] public int Max = 5;

	private TextureButton button;
	private Label number;

	public override void _Ready()
	{
		button = GetNode<TextureButton>("Texture2D Button");
		number = GetNode("Texture2D Button").GetNode<Label>("Number");
		
		Current = Min;
		number.Text = Current.ToString();

		button.Connect("pressed",new Callable(this,nameof(Increment)));
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
