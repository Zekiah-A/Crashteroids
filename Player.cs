using Godot;
using System;

public class Player : KinematicBody2D
{
	[Export] public bool StartFlipped = false;
	[Export] public float Speed = 1000;
	[Export] public float RotateSpeed = 0.2f;
	[Export] public int MaxDragDistance = 30;
	[Export] public int Id;

	public bool IsCurrent = false;
	public bool IsDead = false;

	private Sprite player;
#warning "[rayCast/Player.cs] This field should have a better name."
	private RayCast2D rayCast;
	private Line2D dragLine;
	private Vector2 hitAngle;
	private Vector2 clickPosition;
	private Area2D invalidArea;
	private Gradient dragLineWhite;
	private Gradient dragLineRed;

	private Random random = new Random();
	private int bounces;
	private bool invalid;
#warning "[rayCast/Player.cs] This field should have a better name."
	private bool debounce;
	private bool dragging;

	public override void _Ready()
	{
		player = GetNode<Sprite>("P1_Display");
	 	rayCast = GetNode("P1_Display").GetNode<RayCast2D>("RayCast2D");
	 	dragLine = GetNode<Line2D>($"DragLine");
	 	invalidArea = GetNode("P1_Display").GetNode<Area2D>("InvalidArea");
	  	dragLineWhite = (Gradient) ResourceLoader.Load("res://styles/dragline_white.tres");
		dragLineRed = (Gradient) ResourceLoader.Load("res://styles/dragline_red.tres");

#warning "[Player.cs] Fix weird bug when starting flipped."
		//if (StartFlipped)
		//	Scale = new Vector2(4, -4);
	}

	public override void _Process(float delta)
	{
		if (IsCurrent && debounce && !IsDead)
		{
			var collision = MoveAndCollide(clickPosition * Speed * delta);
			if (collision != null)
			{
				var hit = (Godot.Node2D) collision.Collider;
				if (hit.IsInGroup("Player"))
					GameManager.GameMatch.Crash((Player) hit, this);
				clickPosition = clickPosition.Bounce(collision.Normal);
				player.Rotation = clickPosition.Angle();

				bounces++;
#warning "[Player.cs] bounces >= 1 should be bounces >= matchconfigmaxbouncesvar."
				if (bounces >= 1)
				{
					GameManager.GameMatch.SwitchTurn(Id);
					bounces = 0;
					debounce = false;
				}
				GameManager.GameMatch.TotalBounces[Id] += 1;
			}
		}
		else if (IsDead)
		{
			player.Rotate(delta * 10);
			GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
			MoveAndCollide(hitAngle * 200 * delta);
		}
	}

	///<summary> Handle inputs on PhysicsProcess, (could also be handled on process?) </summary>
	public override void _PhysicsProcess(float _delta)
	{
		if (IsCurrent && !IsDead)
		{
			if (Input.IsKeyPressed((int) KeyList.Space) && debounce == false && !invalid) //or GUI launch button pressed (routed through Match.cs, so that the correct instance can be found)
			{
				player.Rotation = clickPosition.Angle();
				debounce = true;
			}

			///<sumamry> Only rotate on finger or mouse drag </summary>
			if (Input.IsMouseButtonPressed(1)) //or touched screen
			{
				dragLine.Visible = true;
				Input.SetCustomMouseCursor(null, Input.CursorShape.Drag);

				clickPosition = new Vector2( 
					GetViewport().GetMousePosition().x - (Position.x), //TODO: input is screen-scale, while KB is only world scale, must find screen to world pos!
					GetViewport().GetMousePosition().y - (Position.y)
				).Normalized();
				//clickPosition = ToLocal(GetViewport().GetMousePosition()).Normalized();

				Vector2[] linePositions =
				{
					ClampMagnitude(ToLocal(GetViewport().GetMousePosition()), MaxDragDistance),
					Vector2.Zero
				};
				dragLine.Points = linePositions; 

				player.Rotation = Mathf.Lerp(player.Rotation, clickPosition.Angle(), RotateSpeed);
				
				if (invalid)
					dragLine.Gradient = dragLineRed;
				else
					dragLine.Gradient = dragLineWhite;
			}
			else
			{
				dragLine.Visible = false;
			}
		}
	}
	
	private void InvalidAreaEntered(object body)
	{
		if (body is StaticBody2D)
			invalid = true;
	}
	private void InvalidAreaExited(object body)
	{
		if (body is StaticBody2D)
			invalid = false;
	}

	public void Explode()
	{
		hitAngle = new Vector2(random.Next(-10, 10), random.Next(-10, 10)).Normalized();
		GetNode<Node2D>("Explosion").Visible = true;
		debounce = false;
		IsDead = true;
	}

	///<summary> Used for circular clamp, code "borrowed" from unity Mathf @https://github.com/Unity-Technologies/UnityCsReference/ </summary>
	private Vector2 ClampMagnitude(Vector2 vector, float maxLength)
	{
		float sqrMagnitude = (vector.x * vector.x) + (vector.y * vector.y);
		if (sqrMagnitude > maxLength * maxLength)
		{
			float mag = (float)Math.Sqrt(sqrMagnitude);
			float normalized_x = vector.x / mag;
			float normalized_y = vector.y / mag;
			return new Vector2(normalized_x * maxLength,
				normalized_y * maxLength);
		}
		return vector;
	}
}
