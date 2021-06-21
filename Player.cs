using Godot;
using System;

public class Player : KinematicBody2D
{
	public float Speed = 100;
	
	private KinematicBody2D _kb; //rm?
	private Sprite _player;
	private RayCast2D _rayCast;
	
	private Vector2 _velocity = new Vector2(1, 0); //HACK: 1 for testing //Vector2.Zero;
	
	public override void _Ready()
	{
		_kb = (KinematicBody2D) this;
		_player = GetNode<Sprite>("P1_Display");
		_rayCast = GetNode("P1_Display").GetNode<RayCast2D>("RayCast2D");
	}
	
	public override void _Process(float _delta)
	{
		//TODO: Velocity = SINGLE tap pos, after tap, set "lock" var - imposed by game manager during your go
		
		var _collision = MoveAndCollide(_velocity * Speed * _delta);
		if (_collision != null)
		{
			_velocity = _velocity.Bounce(_collision.Normal);
			Rotation += _velocity.Angle(); //maybe rot degrees bad???  //maybe =+??/
		} 
	}
/*
\     /
 \   /
   o
----------
*/


/*
var laser_coll_point = laser2d.get_collision_point()
var laser_coll_normal = laser2d.get_collision_normal()
bounce_one.global_position = laser_coll_point
var forward = laser_coll_point - laser2d.global_position
var reflection = -forward.reflect(laser_coll_normal)
bounce_one.global_rotation = reflection.angle()
*/
}
