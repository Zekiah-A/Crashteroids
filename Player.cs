using Godot;
using System;

public partial class Player : CharacterBody2D
{ 
	private const float Speed = 100;

	public Vector2 MovementDirection;
	public Sprite2D PlayerSprite;
	public bool Launched = false;
	public bool MyTurn = false;
	private bool selected;

	public override void _Ready()
	{
		PlayerSprite = GetNode<Sprite2D>("Display");
	}

	public override void _Input(InputEvent @event)
	{
		if (Launched || !MyTurn || !selected) return;
		var inputPosition = @event switch
		{
			InputEventScreenTouch screenTouch => screenTouch.Position,
			InputEventMouse mouse => mouse.Position,
			_ => Vector2.Zero
		};

		if (inputPosition == Vector2.Zero) return;
		MovementDirection = ToLocal(inputPosition);
		PlayerSprite.Rotation = GetAngleTo(inputPosition) + 1.5708f;
	}

	public override void _Process(double delta)
	{
		if (!Launched || !MyTurn) return;
		var collision = MoveAndCollide(MovementDirection.Normalized() * Speed * (float) delta);
		if (collision == null) return;
		var hit = (Node2D) collision.GetCollider();
		//if (hit.IsInGroup("Player"))
		//	GameManager.GameMatch.Crash((Player) hit, this);
		MovementDirection = MovementDirection.Bounce(collision.GetNormal());
		PlayerSprite.Rotation = MovementDirection.Angle() + 1.5708f; //1.5708 radians is 90 degrees
	}
	private void PlayerAreaInput(object viewport, InputEvent inputEvent, int shapeIndex)
	{
		if (Launched || !MyTurn || inputEvent.GetType() != typeof(InputEventMouseButton) && inputEvent.GetType() != typeof(InputEventScreenTouch)) 
			return;
		if (inputEvent is InputEventMouseButton mouseButton && mouseButton.Pressed || inputEvent is InputEventScreenTouch screenTouch && screenTouch.Pressed)
			selected =! selected; //It will be selected = true in the future, and then disabled by tapping anywhere on the screen when we switch to drag to rotate
		//if (selected)
			/*GetNode<Tween>("PlayerTween").InterpolateProperty(
					GetNode<Sprite2D>("Display"),
					"scale",
					new Vector2(1, 1),
					new Vector2(1.5f, 1.5f),
					0.2f,
					Tween.TransitionType.Sine,
					Tween.EaseType.In
			);*/
		//else
			/*GetNode<Tween>("PlayerTween").InterpolateProperty(
				GetNode<Sprite2D>("Display"),
				"scale",
				new Vector2(1.5f, 1.5f),
				new Vector2(1, 1),
				0.2f,
				Tween.TransitionType.Sine,
				Tween.EaseType.Out,
				0.1f //Delay
			);
		GetNode<Tween>("PlayerTween").Start();*/
	}

	private void InvalidAreaEntered(object body)
	{
		//if (body is StaticBody2D)
		//	invalid = true;
	}
	private void InvalidAreaExited(object body)
	{
		//if (body is StaticBody2D)
		//	invalid = false;
	}

	///<summary> Used for circular clamp, code "borrowed" from unity Mathf @https://github.com/Unity-Technologies/UnityCsReference/ </summary>
	private Vector2 ClampMagnitude(Vector2 vector, float maxLength)
	{
		var sqrMagnitude = vector.x * vector.x + vector.y * vector.y;
		if (!(sqrMagnitude > maxLength * maxLength)) return vector;
		var mag = (float)Math.Sqrt(sqrMagnitude);
		var normalizedX = vector.x / mag;
		var normalizedY = vector.y / mag;
		return new Vector2(normalizedX * maxLength, normalizedY * maxLength);
	}
}
/*
	[Export] public bool StartFlipped = false;
	[Export] public float Speed = 1000;
	[Export] public float RotateSpeed = 0.2f;
	[Export] public int MaxDragDistance = 30;
	[Export] public int Id;

	public bool IsCurrent = false;
	public bool IsDead = false;

	private Sprite2D player;
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
		player = GetNode<Sprite2D>("P1_Display");
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
	public override void _PhysicsProcess(float delta)
	{
		if (IsCurrent && !IsDead) //TODO: Is always false for some reason, bug in the match code?
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
			float normalizedX = vector.x / mag;
			float normalizedY = vector.y / mag;
			return new Vector2(normalizedX * maxLength,
				normalizedY * maxLength);
		}
		return vector;
	}
 */
 
