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
	private bool dragInitiated = false;
	private Control optionsPanel;
	private ShaderMaterial imageMaterial;
	private Label optionName;
	private Vector2 startPos = Vector2.Zero;

	public override void _Ready()
	{
		optionsPanel = GetNode("OptionsPanel").GetNode<Control>("TextureRect");
		imageMaterial = (ShaderMaterial) optionsPanel.Material;
		optionName = GetNode<Label>("OptionName");
	}

	private void ButtonPressed(int index)
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

		if (dragInitiated && Input.IsMouseButtonPressed(1)) //TODO: Touchscreen support as well.
			WhileDragging();
	}

	private void DragStart()
	{
		startPos = GetViewport().GetMousePosition();
		dragInitiated = true;
	}

	private void WhileDragging()
	{
		Vector2 endPos = GetViewport().GetMousePosition();
		float dragX = startPos.x - endPos.x;
		
		imageMaterial.SetShaderParam("scroll", Mathf.Lerp((float) imageMaterial.GetShaderParam("scroll"), dragX / optionsPanel.RectSize.x, scrollSpeed));
	}

	private void DragEnd()
	{
		dragInitiated = false;
		Vector2 endPos = GetViewport().GetMousePosition(); //TODO: Not sure how this even works on mobile, but okay.
		float dragX = endPos.x - startPos.x;

		///<summary> Less than 50, do nothing at all. More than 50, scroll by one. </summary>
		if (dragX > 50)
		{
			if (current <= 0)
				current = optionsCount - 1;
			else
				current--;
		}
		else if (dragX < -50)
		{
			if (current == optionsCount - 1)
				current = 0;
			else
				current++;
		}		

		UpdateTexture();
	}
}
