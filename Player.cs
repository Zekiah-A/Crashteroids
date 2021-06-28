using Godot;
using Crashteroids;
using System;

public class Player : KinematicBody2D
{
	[Export] public float Speed = 800;
	[Export] public int Id;
	
	public bool IsCurrent;
	public bool IsDead;
	
	private Sprite _player;
	private KinematicBody2D _kb;
	private RayCast2D _rayCast;
	private Vector2 _touchPosition;
	private int _bounces;
	private bool _debounce = false;
	
	private Random _random = new Random();
	private Vector2 _hitAngle;
	
	public override void _Ready()
	{
		_kb = (KinematicBody2D) this;
		_player = GetNode<Sprite>("P1_Display");
		_rayCast = GetNode("P1_Display").GetNode<RayCast2D>("RayCast2D");
	}
	
	public override void _Process(float _delta)
	{
		//TODO: Velocity = SINGLE tap pos, after tap, NORMALISE vector ~~set "lock" var - imposed by game manager during your go~~
		if (IsCurrent && _debounce == true && !IsDead)
		{
			var _collision = MoveAndCollide(_touchPosition * Speed * _delta);
			if (_collision != null)
			{
				var _hit = (Godot.Node2D)_collision.Collider;
				GD.Print(_hit.GetName());
				//HACK: Bad code, fix later for non 2player gamemodes
				if (_hit.GetName() == "P1" || _hit.GetName() == "P2")
				{
					GameManager.GameMatch.Crash();
				}
				
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
		else if (IsDead)
		{ //TODO: Use direction of impact with random
			_player.Rotate(_delta * 10);
			var _collision = MoveAndCollide(_hitAngle * 100 * _delta);
			if (_collision != null)
			{
				var _hit = (Godot.Node2D)_collision.Collider;
				if (_hit.GetName() == "Walls")
					GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
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
				_debounce = true;
			}
			else if (_event is InputEventMouseButton _inputMouse && _debounce == false)
			{
				_touchPosition = new Vector2(
					(_inputMouse.Position.x) - (_kb.Position.x),
					(_inputMouse.Position.y) - (_kb.Position.y)
					).Normalized();
				_player.Rotation = _touchPosition.Angle();
				_debounce = true;
			}
		}
	}
	
	public void UpdateSkin() =>
		_player.Texture = Picker.RocketTextures[GameConfig.SkinID];
	
	public void Explode()
	{ //TODO: Fix janky code
		_hitAngle = new Vector2(_random.Next(-10, 10), _random.Next(-10,10));
		GetNode<Node2D>("Explosion").Visible = true;
		IsDead = true;
	}
}
