using Godot;

public partial class IngameNameLabel : Control
{
	public Node2D? TargetNode;
	public bool Clampless = true;
	
	public override void _Process(double delta)
	{
		if (TargetNode == null)
		{
			return;
		}

		if (Clampless)
		{
			Position = new Vector2(
				Mathf.Lerp(Position.X, TargetNode.GetGlobalTransformWithCanvas().Origin.X  - Size.X / 2, 0.6f),
				Mathf.Lerp(Position.Y, TargetNode.GetGlobalTransformWithCanvas().Origin.Y - 84, 0.6f)
			);
		}
		else
		{
			Position = new Vector2(
				Mathf.Clamp(TargetNode.GetGlobalTransformWithCanvas().Origin.X  - Size.X / 2, 0, GetViewport().GetVisibleRect().Size.X - Size.X),
				Mathf.Clamp(TargetNode.GetGlobalTransformWithCanvas().Origin.Y - 84, 0, GetViewport().GetVisibleRect().Size.Y - Size.Y)
			);
		}

		Size = new Vector2(GetNode<Label>("Label").Text.Length * 8, GetNode<Label>("Label").Size.Y);
	}
}
