using Godot;
using System;

public class Checkbox : Control
{
	public bool IsEnabled
	{
		get { return enabled; }
		set
		{
			enabled = value;

			///<summary> Set the correct texture of the checkbox - in case that enabled wasn't set by another script </summary>
			if (enabled)
				image.TextureNormal = checkboxFull;
			else
				image.TextureNormal = checkboxEmpty;
		}
	}

	//[Signal]
	//public delegate void _on_Matchconfig_update(int _configId, bool _isEnabled);

	private TextureButton image;
	private Texture checkboxEmpty;
	private Texture checkboxFull;
	private bool enabled;

	public override void _Ready()
	{
		image = (TextureButton)GetNode("Texture Button");
		checkboxEmpty = ResourceLoader.Load("res://resources/image/checkbox_empty.png") as Texture;
		checkboxFull = ResourceLoader.Load("res://resources/image/checkbox_full.png") as Texture;
		//HACK: To ensure that default value is set if just "Play" pressed.
		//EmitSignal(nameof(_on_Matchconfig_update), ConfigId, IsEnabled);
	}

	private void _on_Click()
	{
		if (IsEnabled)
		{
			image.TextureNormal = checkboxEmpty;
			IsEnabled = false;
		}
		else
		{
			image.TextureNormal = checkboxFull;
			IsEnabled = true;
		}

		//EmitSignal(nameof(_on_Matchconfig_update), ConfigId, IsEnabled);
	}
}
