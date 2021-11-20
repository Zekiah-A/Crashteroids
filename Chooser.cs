using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Chooser : Node
{
	[Export] private int current;
	[Export] private int optionsCount = 3;
	[Export] private float scrollSpeed = 0.075f;
	[Export] private string[] optionsNames;
	private ShaderMaterial imageMaterial;
	private Label optionName;

	public override void _Ready()
	{
		imageMaterial = (ShaderMaterial) GetNode("OptionsPanel").GetNode<CanvasItem>("TextureRect").Material;
		optionName = GetNode<Label>("OptionName");
	}

	private async void ButtonPressed(int index)
	{
		///<note> Forward button = 1 </note>
		if (index == 1)
		{
			if (current == optionsCount - 1)
				current = 0;
			else
				current++;
		}
		else
		{
			if (current <= 0)
				current = optionsCount - 1;
			else
				current--;
		}
	}

	public override void _PhysicsProcess(float delta) => UpdateTexture(); //could also just be on process, not that taxing?

	private void UpdateTexture()
	{
		float scroll = (float) current / (float) optionsCount;
		imageMaterial.SetShaderParam("scroll", Mathf.Lerp((float) imageMaterial.GetShaderParam("scroll"), scroll, scrollSpeed));
		optionName.Text = $"{current + 1}. {optionsNames[current]}";
		//GD.Print(imageMaterial.GetShaderParam("scroll"));
	}
}
