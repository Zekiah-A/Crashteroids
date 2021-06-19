using Godot;
using System;

public class Checkbox : Control
{
	public bool IsEnabled = false;
	
	[Export] public int ConfigId;
	[Signal] public delegate void _on_Matchconfig_update(int _configId, bool _isEnabled);

	private TextureButton _image;
	private Texture _checkboxEmpty;
	private Texture _checkboxFull;

	public override void _Ready()
	{
		_image = (TextureButton) GetNode("Texture Button");
		_checkboxEmpty = ResourceLoader.Load("res://resources/image/checkbox_empty.png") as Texture;
		_checkboxFull = ResourceLoader.Load("res://resources/image/checkbox_full.png") as Texture;
		//HACK: To ensure that default value is set if just "Play" pressed.
		EmitSignal(nameof(_on_Matchconfig_update), ConfigId, IsEnabled);
	}

	private void _on_Click()
	{
		if (IsEnabled)
		{
			_image.SetNormalTexture(_checkboxEmpty);
			IsEnabled = false;
		}
		else
		{
			_image.SetNormalTexture(_checkboxFull);
			IsEnabled = true;
		}
		
		EmitSignal(nameof(_on_Matchconfig_update), ConfigId, IsEnabled);
	}
}
