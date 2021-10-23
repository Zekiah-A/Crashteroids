using Godot;
using Crashteroids;
using System;

public class Player : KinematicBody2D
{
	[Export] public float Speed = 1000;
	[Export] public float RotateSpeed = 0.2f;
	[Export] public int Id;
	[Export] public int DragClamp = 30;

	public bool IsCurrent;
	public bool IsDead = false;

	private Sprite player;
	private KinematicBody2D kb;
	private RayCast2D rayCast;
	private Line2D dragLine;
	private Vector2 touchPosition;
	private Vector2 mousePosition;
	private int bounces;
	private bool debounce;

	private Random random = new Random();
	private Vector2 hitAngle;

	public override void _Ready()
	{
		kb = (KinematicBody2D)this;
		player = GetNode<Sprite>("P1_Display");
		rayCast = GetNode("P1_Display").GetNode<RayCast2D>("RayCast2D");
		dragLine = GetNode<Line2D>($"DragLine");
	}

	public override void _Process(float delta)
	{
		//TODO: Velocity = SINGLE tap poDoes, after tap, NORMALISE vector ~~set "lock" var - imposed by game manager during your go~~
		if (IsCurrent && debounce == true && !IsDead)
		{
			var collision = MoveAndCollide(touchPosition * Speed * delta);
			if (collision != null)
			{
				var hit = (Godot.Node2D)collision.Collider;
				GD.Print(hit.Name);
				//HACK: Bad code, fix later for non 2player gamemodes
				if (hit.IsInGroup("Player"))
					GameManager.GameMatch.Crash(hit as Player, this);

				touchPosition = touchPosition.Bounce(collision.Normal);
				///<summary>I spent hours trying to figure out something this easy.</summary>
				player.Rotation = touchPosition.Angle();

				bounces++;
				if (bounces >= GameConfig.Match.RocketBounces)
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
			//TODO: Die animation in the direction of impact - make hitangle (referenced in the "explode" function here) the opposite of where the ship was hit from
			player.Rotate(delta * 10);
			GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
			MoveAndCollide(hitAngle * 100 * delta);
		}
	}

	///<summary> Handle inputs on PhysicsProcess, (could also be handled on process?) </summary>
	public override void _PhysicsProcess(float _delta)
	{
		if (IsCurrent && !IsDead)
		{
			if (Input.IsKeyPressed((int) KeyList.Space) && debounce == false) //or GUI launch button pressed (routed through Match.cs, so that the correct instance can be found)
			{
				touchPosition = mousePosition; //TODO: This is MESSY, remove touch position, and combine both
				player.Rotation = touchPosition.Angle();
				debounce = true;
			}

			///<sumamry> Only rotate on finger or mouse drag </summary>
			if (Input.IsMouseButtonPressed(1)) //or touched screen
			{
				dragLine.Visible = true;
				Input.SetCustomMouseCursor(null, Input.CursorShape.Drag);

				mousePosition = new Vector2( 
					GetViewport().GetMousePosition().x- (kb.Position.x), //TODO: input is screen-scale, while KB is only world scale, must find screen to world pos!
					GetViewport().GetMousePosition().y - (kb.Position.y)
				).Normalized();

				Vector2[] linePositions =
				{
					ClampMagnitude(ToLocal(GetViewport().GetMousePosition()), DragClamp),
					Vector2.Zero
				};
				dragLine.Points = linePositions; 

				player.Rotation = Mathf.Lerp(player.Rotation, mousePosition.Angle(), RotateSpeed);
			}
			else
			{
				dragLine.Visible = false;
			}
		}
	}

	public void UpdateSkin() =>
		player.Texture = Picker.RocketTextures[GameConfig.Instance.SkinID];

	public void Explode()
	{
		//TODO: See line 66
		hitAngle = new Vector2(random.Next(-10, 10), random.Next(-10, 10)).Normalized();
		GetNode<Node2D>("Explosion").Visible = true;
		debounce = false;
		IsDead = true;
	}

	///<summary> Used for circular clamp, code "borrowed" from unity Mathf @https://github.com/Unity-Technologies/UnityCsReference/ </summary>
	private Vector2 ClampMagnitude(Vector2 vector, float maxLength) //TODO: Utils.ClampMagnitude function
	{
		float sqrMagnitude = (vector.x * vector.x) + (vector.y * vector.y);
		if (sqrMagnitude > maxLength * maxLength)
		{
			float mag = (float)Math.Sqrt(sqrMagnitude);

			//these intermediate variables force the intermediate result to be
			//of float precision. without this, the intermediate result can be of higher
			//precision, which changes behavior.
			float normalized_x = vector.x / mag;
			float normalized_y = vector.y / mag;
			return new Vector2(normalized_x * maxLength,
				normalized_y * maxLength);
		}
		return vector;
	}
}
