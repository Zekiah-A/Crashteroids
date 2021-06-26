using Godot;
using System;

public class Player : KinematicBody2D
{
	[Export] public float Speed = 800;
	[Export] public int Id;
	
	public bool IsCurrent;
	
	private KinematicBody2D _kb; //NOTE: redundant!
	private Sprite _player;
	private RayCast2D _rayCast;
	//private Vector2 _velocity; //= new Vector2(1, 0); //HACK: 1 for testing //Vector2.Zero;
	private Vector2 _touchPosition;
	private int _bounces;
	private bool _debounce = false;
	
	public override void _Ready()
	{
		_kb = (KinematicBody2D) this;
		_player = GetNode<Sprite>("P1_Display");
		_rayCast = GetNode("P1_Display").GetNode<RayCast2D>("RayCast2D");
	}
	
	public override void _Process(float _delta)
	{
		//TODO: Velocity = SINGLE tap pos, after tap, NORMALISE vector ~~set "lock" var - imposed by game manager during your go~~
		if (IsCurrent && _debounce == true)
		{
			var _collision = MoveAndCollide(_touchPosition * Speed * _delta);
			if (_collision != null)
			{
				_touchPosition = _touchPosition.Bounce(_collision.Normal);
				///<summary>I spent hours trying to figure out something this easy.</summary>
				_player.Rotation = _touchPosition.Angle();
				
				_bounces++;
				if (_bounces >= GameConfig.Match.RocketBounces)
				{
					GameManager.GameMatch.SwitchTurn(Id);
					_bounces = 0;
					_debounce = false;
				}
			}
		}
		
	}
	
	public override void _Input(InputEvent _event)
	{
		if (IsCurrent)
		{
			if (_event is InputEventScreenTouch _inputTouch && _debounce == false)
			{
				_touchPosition = new Vector2(
					(_inputTouch.Position.x) - (_kb.Position.x),
					(_inputTouch.Position.y) - (_kb.Position.y)
					).Normalized();
				_player.Rotation = _touchPosition.Angle();
				GD.Print(_event);
				GD.Print(_touchPosition);
				_debounce = true;
			}
			else if (_event is InputEventMouseButton _inputMouse && _debounce == false)
			{
				_touchPosition = new Vector2(
					(_inputMouse.Position.x) - (_kb.Position.x),
					(_inputMouse.Position.y) - (_kb.Position.y)
					).Normalized();
				_player.Rotation = _touchPosition.Angle();
				GD.Print(_event);
				GD.Print(_touchPosition);
				_debounce = true;
			}
		}
	}
}
