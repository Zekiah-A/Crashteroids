using Godot;
using System;

public class Iteratebox : Control
{
	public int Current = 1;
	public int Max = 5;
	
	[Export] public int ConfigId;
	[Signal] public delegate void _on_Matchconfig_update(int _configId, int _newValue);
	
	private TextureButton _button;
	private Label _number;
	
	public override void _Ready()
	{
		_button = (TextureButton) GetNode("Texture Button");
		_number = (Label) GetNode("Texture Button").GetNode("Number"); 
		
		_number.Text = Current.ToString();
		//HACK: To ensure that default value is set if just "Play" pressed.
		EmitSignal(nameof(_on_Matchconfig_update), ConfigId, Current);
	}

	private void _on_Click()
	{
		if (Current < Max)
			Current++;
		else
			Current = 1;
		
		_number.Text = Current.ToString();
		
		EmitSignal(nameof(_on_Matchconfig_update), ConfigId, Current);
	}
}
