using Godot;
using System;

public class Checkbox : Control
{
	public bool IsEnabled
	{
		get { return enabled; }
		set
		{
			///<summary> Set the correct texture of the checkbox - in case that enabled wasn't set by another script </summary>
			enabled = value;
			if (enabled)
				image.TextureNormal = checkboxFull;
			else
				image.TextureNormal = checkboxEmpty;
		}
	}

	private TextureButton image;
	private Texture checkboxEmpty;
	private Texture checkboxFull;
	private bool enabled;

	public override void _Ready()
	{
		image = (TextureButton)GetNode("Texture Button");
		checkboxEmpty = ResourceLoader.Load("res://resources/image/checkbox_empty.png") as Texture;
		checkboxFull = ResourceLoader.Load("res://resources/image/checkbox_full.png") as Texture;
	}
}
