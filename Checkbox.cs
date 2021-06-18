using Godot;
using System;

public class Checkbox : Control
{
	public bool IsEnabled = false;
	private TextureButton _image;
	private Texture _checkboxEmpty;
	private Texture _checkboxFull;

	public override void _Ready()
	{
		_image = (TextureButton) GetNode("Texture Button");
		_checkboxEmpty = ResourceLoader.Load("res://checkbox_empty.png") as Texture;
		_checkboxFull = ResourceLoader.Load("res://checkbox_full.png") as Texture;
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
	}
}
