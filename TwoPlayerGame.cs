using Godot;
using System;
using System.Collections.Generic;

public partial class TwoPlayerGame : Node
{
	public List<Node2D> Players = new();
	
	private int currentTurn = 0;
	private RichTextLabel playerTurnLabel;
	private Control ingameNameLabel;
	private bool cameraBlocked;
	
	public override void _Ready()
	{
		var random = new Random();
		//Initialise Map
		if (TwoPlayerGameData.RandomMap)
			TwoPlayerGameData.Map = random.Next(3);

		var mapScene = GD.Load<PackedScene>("res://Scenes/DefaultMap.tscn");
		switch (TwoPlayerGameData.Map)
		{
			case 0: mapScene = GD.Load<PackedScene>("res://Scenes/DefaultMap.tscn"); break;
			case 1: mapScene = GD.Load<PackedScene>("res://Scenes/DefaultMap.tscn"); break;
			case 2: mapScene = GD.Load<PackedScene>("res://Scenes/DefaultMap.tscn"); break;
		}
		AddChild(mapScene.Instantiate());
		
		//Spawn special abilities around map
		if (TwoPlayerGameData.SpecialAbilities) { }
		
		//Add players to scene
		for (var i = 0; i < 2; i++)
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

		StartGame();
	}

	public async void StartGame()
	{
		var cameraTween = GetNode<Tween>("CameraTween");

		cameraBlocked = true;
		/*cameraTween.InterpolateProperty(
			GetNode<Camera2D>("Camera2D"),
			"zoom",
			GetNode<Camera2D>("Camera2D").Zoom,
			new Vector2(0.3f, 0.3f),
			8,
			Tween.TransitionType.Sine,
			Tween.EaseType.Out
		);*/
		
		for (var i = 0; i < Players.Count; i++)
		{
			ingameNameLabel.GetNode<Label>("Label").Text = i == 0 ? $"{Config.Load("name")} (Player {i + 1})" : $"Guest (Player {i + 1})";
			if (string.IsNullOrEmpty((string) Config.Load("name")))
			{
				ingameNameLabel.GetNode<Label>("Label").Text = $"Player {i + 1}";
			}
			
			((IngameNameLabel) ingameNameLabel).TargetNode = Players[i];
			/*cameraTween.InterpolateProperty(
				GetNode<Camera2D>("Camera2D"),
				"position",
				GetNode<Camera2D>("Camera2D").Position,
				Players[i].Position,
				4,
				Tween.TransitionType.Back,
				Tween.EaseType.Out
			);
			cameraTween.Start();
			await ToSignal(cameraTween, "tween_completed");*/
		}
		
		/*await ToSignal(cameraTween, "tween_completed");
		cameraTween.InterpolateProperty(
			GetNode<Camera2D>("Camera2D"),
			"position",
			GetNode<Camera2D>("Camera2D").Position,
			new Vector2(512, 300),
			2,
			Tween.TransitionType.Quad,
			Tween.EaseType.Out
		);
		cameraTween.InterpolateProperty(
			GetNode<Camera2D>("Camera2D"),
			"zoom",
			GetNode<Camera2D>("Camera2D").Zoom,
			new Vector2(1, 1),
			2,
			Tween.TransitionType.Sine,
			Tween.EaseType.In
		);
		cameraTween.Start();*/
		await ToSignal(cameraTween, "tween_completed");
		
		ingameNameLabel.GetNode<Label>("Label").Text = FormatIngameNameLabel();
		((IngameNameLabel) ingameNameLabel).TargetNode = Players[0];
		((IngameNameLabel) ingameNameLabel).Clampless = false;
		((Player) Players[0]).MyTurn = true;
		cameraBlocked = false;
	}

	private InputEventScreenTouch secondaryTouch;
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

				/*GetNode<Tween>("CameraTween").InterpolateProperty(
					GetNode<Camera2D>("Camera2D"),
					"zoom",
					GetNode<Camera2D>("Camera2D").Zoom,
					new Vector2(Mathf.Clamp(GetNode<Camera2D>("Camera2D").Zoom.x + zoom.x, 0.15f, 1), 
						Mathf.Clamp(GetNode<Camera2D>("Camera2D").Zoom.y + zoom.y, 0.15f, 1)),
					0.1f,
					Tween.TransitionType.Sine
				);
				GetNode<Tween>("CameraTween").Start();*/
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
						Mathf.Clamp(GetNode<Camera2D>("Camera2D").Zoom.x + zoom, 0.15f, 1),
						Mathf.Clamp(GetNode<Camera2D>("Camera2D").Zoom.y + zoom, 0.15f, 1)
					);
					previousDragDistance = dragDistance;
				}
			
				GetNode<Camera2D>("Camera2D").Position -= screenDrag.Relative;
				break;
			}
		}

		GetNode<Camera2D>("Camera2D").Position = new Vector2(
			Mathf.Clamp(GetNode<Camera2D>("Camera2D").Position.x, 0, 1024),
			Mathf.Clamp(GetNode<Camera2D>("Camera2D").Position.y, 0, 600)
		);
	}

	//If we have a username, it will be (p1) 'username (Player 1)', (p2) 'guest (Player 2)', if there is no username, it will be (p1) 'Player 1' (p2) 'Player 2'
	private string FormatIngameNameLabel()
	{
		if (string.IsNullOrEmpty((string) Config.Load("name")))
		{
			return $"Player {currentTurn + 1}";
		}
		
		return currentTurn== 0 ? $"{Config.Load("name")} (Player {currentTurn + 1})" : $"Guest (Player {currentTurn + 1})";
	}

	//If we have a username, it will be (p1) 'username', (p2) 'guest', if there is no username, it will be (p1) 'Player 1' (p2) 'Player 2'
	private string FormatPlayerTurnLabel()
	{
		if (string.IsNullOrEmpty((string) Config.Load("name")))
		{
			return $"[center][wave amp=5 freq=2]Player {currentTurn + 1}'s turn.[/wave][/center]";
		}
		
		return currentTurn == 0 ? $"[center][wave amp=5 freq=2]{Config.Load("name")}'s turn.[/wave][/center]" : "[center][wave amp=5 freq=2]Guest's turn.[/wave][/center]";
	}
}
