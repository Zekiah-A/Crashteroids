using Godot;
using System;

public class IngameNameLabel : Control
{
	public Node2D TargetNode;
	public bool AutoLocate = true;
	
	public override void _Ready()
	{
		
	}

	public override void _Process(float delta)
	{
		if (TargetNode == null) 
			return;
		
		if (AutoLocate)
		{
			RectPosition = new Vector2(TargetNode.GetGlobalTransformWithCanvas().origin.x - (RectSize.x / 2), TargetNode.GetGlobalTransformWithCanvas().origin.y - 84);
		}
	}
}
