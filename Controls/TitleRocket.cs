using Godot;
using System;
using System.Threading.Tasks;

public partial class TitleRocket : CharacterBody2D
{
	private readonly Random random = new Random();
	private Vector2 movement;

	public override void _Ready() =>
		Rotation = random.Next(-3, 3);
	
	public override void _PhysicsProcess(double delta)
	{
		movement = new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation)).Normalized();

		var collision = MoveAndCollide(movement * 50 * (float) delta);
		if (collision is not null)
		{
			Rotation = movement.Bounce(collision.GetNormal()).Angle();
		}

		GetNode<Sprite2D>("P1_Display").Rotation += 0.01f;
		if(!GetNode<AnimationPlayer>("AnimationPlayer").IsPlaying())
			GetNode<AnimationPlayer>("AnimationPlayer").Play("rescale_anim");
	}
}
