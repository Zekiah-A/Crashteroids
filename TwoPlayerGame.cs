using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class TwoPlayerGame : Node
{
	public List<Node2D> Players = new();
	
	private int currentTurn;
	private RichTextLabel playerTurnLabel = null!;
	private Control ingameNameLabel = null!;
	private bool cameraBlocked;
	
	public override void _Ready()
	{
		GD.Print("1");
		var random = new Random();

		//Initialise Map
		if (TwoPlayerGameData.RandomMap)
		{
			TwoPlayerGameData.Map = random.Next(3);
		}

		var mapScene = TwoPlayerGameData.Map switch
		{
			0 => GD.Load<PackedScene>("res://Scenes/DefaultMap.tscn"),
			1 => GD.Load<PackedScene>("res://Scenes/DefaultMap.tscn"),
			2 => GD.Load<PackedScene>("res://Scenes/DefaultMap.tscn"),
			_ => throw new ArgumentOutOfRangeException()
		};
		
		AddChild(mapScene.Instantiate());
		
		//Spawn special abilities around map
		if (TwoPlayerGameData.SpecialAbilities)
		{
			
		}
		
		//Add players to scene
		for (var i = 0; i < Players.Count; i++)
		{
			var playerScene = GD.Load<PackedScene>("res://Scenes/Player.tscn");
			var player = playerScene.Instantiate();
			AddChild(player);
			Players.Add((Node2D) player);
		}
		
		Players[0].Position = new Vector2(48, 300);
		((Player) Players[0]).MovementDirection = Vector2.Right;
		((Player) Players[0]).PlayerSprite.Rotation = 1.5708f;
		Players[1].Position = new Vector2(996, 300);
		((Player) Players[1]).MovementDirection = Vector2.Left;
		((Player) Players[1]).PlayerSprite.Rotation = -1.5708f;

		playerTurnLabel = GetNode<RichTextLabel>("CanvasLayer/UI/PlayerTurn/Title");
		playerTurnLabel.Text = FormatPlayerTurnLabel();
		
		var ingameNameScene = GD.Load<PackedScene>("res://Scenes/IngameNameLabel.tscn");
		ingameNameLabel = (Control) ingameNameScene.Instantiate();
		GetNode<CanvasLayer>("CanvasLayer").AddChild(ingameNameLabel);

		PlayIntroAnimation();
	}

	private void PlayIntroAnimation()
	{
		cameraBlocked = true;
		var tween = CreateTween();
		var camera = GetNode<Camera2D>("Camera2D");
		tween.TweenProperty(camera, "zoom", new Vector2(0.3f, 0.3f), 8)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.Out);

		for (var i = 0; i < Players.Count; i++)
		{
			ingameNameLabel.GetNode<Label>("Label").Text = i == 0 ? $"{Config.Load<string>("name")} (Player {i + 1})" : $"Guest (Player {i + 1})";
			if (string.IsNullOrEmpty(Config.Load<string>("name")))
			{
				ingameNameLabel.GetNode<Label>("Label").Text = $"Player {i + 1}";
			}
			
			((IngameNameLabel) ingameNameLabel).TargetNode = Players[i];
			tween.Chain().TweenProperty(camera, "position", Players[i].Position, 4)
				.SetTrans(Tween.TransitionType.Back)
				.SetEase(Tween.EaseType.Out);
			
			tween.Play();
		}

		tween.Chain().TweenProperty(camera, "position", GetViewport().GetVisibleRect().Size / 2, 2)
			.SetTrans(Tween.TransitionType.Quad)
			.SetEase(Tween.EaseType.Out);

		tween.Chain().TweenProperty(camera, "zoom", Vector2.One, 2)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.In);

		tween.Play();
		
		ingameNameLabel.GetNode<Label>("Label").Text = FormatIngameNameLabel();
		((IngameNameLabel) ingameNameLabel).TargetNode = Players[0];
		((IngameNameLabel) ingameNameLabel).Clampless = false;
		((Player) Players[0]).MyTurn = true;
		cameraBlocked = false;
	}

	private InputEventScreenTouch? secondaryTouch;
	private float previousDragDistance;
	private bool draggingMouse;
	public override void _Input(InputEvent inputEvent)
	{
		if (cameraBlocked) return;
		switch (inputEvent)
		{
			case InputEventMouseButton mouseButton:
			{
				draggingMouse = mouseButton.Pressed;

				var zoom = mouseButton.ButtonIndex switch
				{
					MouseButton.WheelUp => new Vector2(-0.2f, -0.2f),
					MouseButton.WheelDown => new Vector2(0.2f, 0.2f),
					_ => Vector2.Zero
				};

				var tween = CreateTween();
				var camera = GetNode<Camera2D>("Camera2D");
				tween.TweenProperty
					(
						camera,
						"zoom",
						new Vector2(Mathf.Clamp(camera.Zoom.X + zoom.X, 0.15f, 1),
							Mathf.Clamp(GetNode<Camera2D>("Camera2D").Zoom.Y + zoom.Y, 0.15f, 1)),
						0.1f
					)
					.SetTrans(Tween.TransitionType.Sine)
					.SetEase(Tween.EaseType.Out);
				tween.Play();
				break;
			}
			case InputEventMouseMotion mouseMotion:
			{
				if (draggingMouse)
					GetNode<Camera2D>("Camera2D").Position -= mouseMotion.Relative;
				break;
			}
			case InputEventScreenTouch screenTouch:
				secondaryTouch = screenTouch.Index == 1 ? screenTouch.Pressed ? screenTouch : null : null;
				GD.Print($"secondarytouch: {secondaryTouch} index: {screenTouch.Index}");
				break;
			case InputEventScreenDrag screenDrag:
			{
				if (secondaryTouch != null)
				{
					var dragDistance = Mathf.Abs(secondaryTouch.Position.DistanceTo(screenDrag.Position));
					var zoom = dragDistance < previousDragDistance ? 0.01f : -0.01f;
					GetNode<Camera2D>("Camera2D").Zoom = new Vector2(
						Mathf.Clamp(GetNode<Camera2D>("Camera2D").Zoom.X + zoom, 0.15f, 1),
						Mathf.Clamp(GetNode<Camera2D>("Camera2D").Zoom.Y + zoom, 0.15f, 1)
					);
					previousDragDistance = dragDistance;
				}
			
				GetNode<Camera2D>("Camera2D").Position -= screenDrag.Relative;
				break;
			}
		}

		GetNode<Camera2D>("Camera2D").Position = new Vector2(
			Mathf.Clamp(GetNode<Camera2D>("Camera2D").Position.X, 0, 1024),
			Mathf.Clamp(GetNode<Camera2D>("Camera2D").Position.Y, 0, 600)
		);
	}

	//If we have a username, it will be (p1) 'username (Player 1)', (p2) 'guest (Player 2)', if there is no username, it will be (p1) 'Player 1' (p2) 'Player 2'
	private string FormatIngameNameLabel()
	{
		if (string.IsNullOrEmpty(Config.Load<string>("name")))
		{
			return $"Player {currentTurn + 1}";
		}
		
		return currentTurn== 0 ? $"{Config.Load<string>("name")} (Player {currentTurn + 1})" : $"Guest (Player {currentTurn + 1})";
	}

	//If we have a username, it will be (p1) 'username', (p2) 'guest', if there is no username, it will be (p1) 'Player 1' (p2) 'Player 2'
	private string FormatPlayerTurnLabel()
	{
		if (string.IsNullOrEmpty(Config.Load<string>("name")))
		{
			return $"[center][wave amp=5 freq=2]Player {currentTurn + 1}'s turn.[/wave][/center]";
		}
		
		return currentTurn == 0 ? $"[center][wave amp=5 freq=2]{Config.Load<string>("name")}'s turn.[/wave][/center]" : "[center][wave amp=5 freq=2]Guest's turn.[/wave][/center]";
	}
}
