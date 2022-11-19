using Godot;
using System;

public partial class IngameNameLabel : Control
{
	public Node2D TargetNode;
	public bool Clampless = true;
	
	public override void _Process(double delta)
	{
		if (TargetNode == null) 
			return;

		if (Clampless)
		{
			Position = new Vector2(
				Mathf.Lerp(Position.x, TargetNode.GetGlobalTransformWithCanvas().origin.x  - Size.x / 2, 0.6f),
				Mathf.Lerp(Position.y, TargetNode.GetGlobalTransformWithCanvas().origin.y - 84, 0.6f)
			);
		}
		else
		{
			Position = new Vector2(
				Mathf.Clamp(TargetNode.GetGlobalTransformWithCanvas().origin.x  - Size.x / 2, 0, GetViewport().GetVisibleRect().Size.x - Size.x),
				Mathf.Clamp(TargetNode.GetGlobalTransformWithCanvas().origin.y - 84, 0, GetViewport().GetVisibleRect().Size.y - Size.y)
			);
		}

		Size = new Vector2(GetNode<Label>("Label").Text.Length * 8, GetNode<Label>("Label").Size.y);
	}
}
