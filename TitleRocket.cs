using Godot;
using System;

public class TitleRocket : KinematicBody2D
{
	Random random = new Random();
	private Vector2 rotation;

	public override void _Ready() =>
		Rotation = random.Next(-10, 10);

	public override void _Process(float delta)
	{
		rotation = new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation)).Normalized();

		var collision = MoveAndCollide(rotation * 500 * delta);
		if (collision != null)
		{
			Rotation = rotation.Bounce(collision.Normal).Angle();
			//Rotation = rotation.Angle();
		}

		GetNode<Sprite>("P1_Display").Rotation = Mathf.Lerp(GetNode<Sprite>("P1_Display").Rotation, rotation.Angle(), 0.1f);
	}
}
