using Godot;
using System;

public class IngameNameLabel : Control
{
	public Node2D TargetNode;
	public bool Clampless = true;
	
	public override void _Process(float delta)
	{
		if (TargetNode == null) 
			return;

		if (Clampless)
		{
			RectPosition = new Vector2(
				Mathf.Lerp(RectPosition.x, TargetNode.GetGlobalTransformWithCanvas().origin.x  - RectSize.x / 2, 0.6f),
				Mathf.Lerp(RectPosition.y, TargetNode.GetGlobalTransformWithCanvas().origin.y - 84, 0.6f)
			);
		}
		else
		{
			RectPosition = new Vector2(
				Mathf.Clamp(TargetNode.GetGlobalTransformWithCanvas().origin.x  - RectSize.x / 2, 0, GetViewport().GetVisibleRect().Size.x - RectSize.x),
				Mathf.Clamp(TargetNode.GetGlobalTransformWithCanvas().origin.y - 84, 0, GetViewport().GetVisibleRect().Size.y - RectSize.y)
			);
		}

		RectSize = new Vector2(GetNode<Label>("Label").Text.Length * 7, GetNode<Label>("Label").RectSize.y);
	}
}
