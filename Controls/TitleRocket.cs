using Godot;
using System;
using System.Threading.Tasks;

public class TitleRocket : KinematicBody2D
{
	private readonly Random random = new Random();
	private Vector2 movement;

	public override void _Ready() =>
		Rotation = random.Next(-180, 180);
	
	public override void _PhysicsProcess(float delta)
	{
		movement = new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation)).Normalized();

		var collision = MoveAndCollide(movement * 50 * delta);
		if (collision != null)
		{
			Rotation = movement.Bounce(collision.Normal).Angle();
		}

		GetNode<Sprite>("P1_Display").RotationDegrees += 1;
		if(!GetNode<AnimationPlayer>("AnimationPlayer").IsPlaying())
			GetNode<AnimationPlayer>("AnimationPlayer").Play("rescale_anim");
	}
}
