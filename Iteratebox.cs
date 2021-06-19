using Godot;
using System;

public class Iteratebox : Control
{
	public int Current = 1;
	public int Max = 5;
	
	private TextureButton _button;
	private Label _number;

	public override void _Ready()
	{
		_button = (TextureButton) GetNode("Texture Button");
		_number = (Label) GetNode("Texture Button").GetNode("Number"); 
		
		_number.Text = Current.ToString();
	}

	private void _on_Click()
	{
		if (Current < Max)
			Current++;
		else
			Current = 1;
		
		_number.Text = Current.ToString();
	}
}
