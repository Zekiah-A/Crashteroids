using Godot;

public partial class Chooser : Node
{
	[Export] private int current;
	[Export] private int optionsCount = 3;
	[Export] private float scrollSpeed = 0.075f;
	[Export] private string[] optionsNames;
	private bool dragInitiated;
	private Control optionsPanel;
	private ShaderMaterial imageMaterial;
	private Label optionName;
	private Vector2 startPos = Vector2.Zero;

	public override void _Ready()
	{
		optionsPanel = GetNode("OptionsPanel").GetNode<Control >("TextureRect");
		imageMaterial = (ShaderMaterial) optionsPanel.Material;
		optionName = GetNode<Label>("OptionName");
	}

	///<note> Forward button = 1 </note>
	private void ButtonPressed(int index)
	{
		if (index == 1)
		{
			if (current == optionsCount - 1)
				current = optionsCount - 1; //current = 0;
			else
				current++;
		}
		else
		{
			if (current <= 0)
				current = 0; //current = optionsCount - 1;
			else
				current--;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		UpdateTexture(); //could also just be on process, not that taxing?
		if (dragInitiated && Input.IsMouseButtonPressed(MouseButton.Left))
			WhileDragging();
	}

	private void UpdateTexture()
	{
		var scroll = current / optionsCount;
		imageMaterial.SetShaderParameter("scroll", Mathf.Lerp((float) imageMaterial.GetShaderParameter("scroll"), scroll, scrollSpeed));
		optionName.Text = $"{current + 1}. {optionsNames[current]}";
	}

	private void DragStart()
	{
		startPos = GetViewport().GetMousePosition();
		dragInitiated = true;
	}

	private void WhileDragging()
	{
		var endPos = GetViewport().GetMousePosition();
		var dragX = (startPos.x - endPos.x) + (current * optionsPanel.Size.x / optionsCount);
		imageMaterial.SetShaderParameter("scroll",  dragX / optionsPanel.Size.x);
	}

	///<summary> Less than 50, do nothing at all. More than 50, scroll by one. </summary>
	private void DragEnd()
	{
		dragInitiated = false;
		var endPos = GetViewport().GetMousePosition();
		var dragX = endPos.x - startPos.x;

		switch (dragX)
		{
			case > 50 when current <= 0:
				current = 0; //current = optionsCount - 1;
				break;
			case > 50:
				current--;
				break;
			case < -50 when current == optionsCount - 1:
				current = optionsCount - 1; //current = 0;
				break;
			case < -50:
				current++;
				break;
		}

		UpdateTexture();
	}
}
