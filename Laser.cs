using Godot;
using System;

public class Laser : Node2D
{
	public const int MaxLaserBounces = 15;

	public Line2D laserLine;
	public RayCast2D rayCast;
	
	public override void _Ready()
	{
		laserLine = GetNode<Line2D>("Line2D");
		rayCast = GetNode<RayCast2D>("RayCast2D");
	}

	public override void _PhysicsProcess(float delta)
	{
		laserLine.ClearPoints();
		laserLine.AddPoint(Vector2.Zero);
		laserLine.AddPoint(ToLocal(rayCast.GetCollisionPoint()));
	}
}
