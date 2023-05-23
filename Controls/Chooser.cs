using Godot;

public partial class Chooser : Node
{
	[Export] private int current;
	[Export] private int optionsCount = 3;
	[Export] private float scrollSpeed = 0.075f;
	[Export] private string[] optionsNames = null!;
	private bool dragInitiated;
	private Control optionsTexture = null!;
	private ShaderMaterial imageMaterial = null!;
	private Label optionName = null!;
	private Vector2 startPos = Vector2.Zero;

	public override void _Ready()
	{
		optionsTexture = GetNode<Control>("OptionsPanel/TextureRect");
		imageMaterial = (ShaderMaterial) optionsTexture.Material;
		optionName = GetNode<Label>("OptionName");
	}

	private void OnTextureInput(InputEvent @event)
	{
		
		if (@event.IsPressed())
		{
			if (dragInitiated) return;
			dragInitiated = true;
			startPos = GetViewport().GetMousePosition();
		}
		else
		{
			dragInitiated = false;
			var endPos = GetViewport().GetMousePosition();
			var dragX = endPos.X - startPos.X;

			switch (dragX)
			{
				case > 50 when current <= 0:
					current = 0;
					break;
				case > 50:
					current--;
					break;
				case < -50 when current == optionsCount - 1:
					current = optionsCount - 1;
					break;
				case < -50:
					current++;
					break;
			}

			UpdateTexture();
		}
	}

	///<note> Forward button = 1 </note>
	private void ButtonPressed(int index)
	{
		if (index == 1)
		{
			current = current == optionsCount - 1 ? optionsCount - 1 : current + 1;
			return;
		}
		
		current = current <= 0 ? 0 : current - 1;
	}

	public override void _PhysicsProcess(double delta)
	{
		UpdateTexture();

		if (dragInitiated && Input.IsMouseButtonPressed(MouseButton.Left))
		{
			WhileDragging();
		}
	}

	private void UpdateTexture()
	{
		var scroll = (float) current / optionsCount;
		imageMaterial.SetShaderParameter("scroll", Mathf.Lerp((float) imageMaterial.GetShaderParameter("scroll"), scroll, scrollSpeed));
		optionName.Text = $"{current + 1}. {optionsNames[current]}";
	}

	private void WhileDragging()
	{
		var endPos = GetViewport().GetMousePosition();
		var dragX = startPos.X - endPos.X + current * optionsTexture.Size.X / optionsCount;
		imageMaterial.SetShaderParameter("scroll",  dragX / optionsTexture.Size.X);
	}
}
