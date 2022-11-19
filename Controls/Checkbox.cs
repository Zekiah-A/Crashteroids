using Godot;
using System;

public partial class Checkbox : Control
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
	private Texture2D checkboxEmpty;
	private Texture2D checkboxFull;
	private bool enabled;

	public override void _Ready()
	{
		image = (TextureButton)GetNode("Texture2D Button");
		checkboxEmpty = ResourceLoader.Load("res://Resources/image/checkbox_empty.png") as Texture2D;
		checkboxFull = ResourceLoader.Load("res://Resources/image/checkbox_full.png") as Texture2D;
		image.Connect("pressed",new Callable(this,nameof(Toggle)));
	}

	private void Toggle() => IsEnabled = !IsEnabled;
}
