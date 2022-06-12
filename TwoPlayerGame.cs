using Godot;
using System;
using System.Collections.Generic;

public class TwoPlayerGame : Node
{
	public List<Node2D> Players = new List<Node2D>();
	
	private int currentTurn = 0;
	private RichTextLabel playerTurnLabel;
	private Control ingameNameLabel;
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
		AddChild(mapScene.Instance());
		
		//Spawn special abilities around map
		if (TwoPlayerGameData.SpecialAbilities) { }
		
		//Add players to scene
		for (int i = 0; i < 2; i++)
		{
			var playerScene = GD.Load<PackedScene>("res://Scenes/Player.tscn");
			var player = playerScene.Instance();
			AddChild(player);
			Players.Add((Node2D) player);
		}
		Players[0].Position = new Vector2(48, 300);
		((Player) Players[0]).MovementDirection = Vector2.Right;
		((Player) Players[0]).PlayerSprite.Rotation = 1.5708f;
		Players[1].Position = new Vector2(976, 300);
		((Player) Players[1]).MovementDirection = Vector2.Left;
		((Player) Players[1]).PlayerSprite.Rotation = -1.5708f;

		playerTurnLabel = GetNode<RichTextLabel>("CanvasLayer/UI/PlayerTurn/Title");
		playerTurnLabel.BbcodeText = FormatPlayerTurnLabel();
		
		var ingameNameScene = GD.Load<PackedScene>("res://Scenes/IngameNameLabel.tscn");
		ingameNameLabel = (Control) ingameNameScene.Instance();
		GetNode<CanvasLayer>("CanvasLayer").AddChild(ingameNameLabel);

		StartGame();
	}

	public async void StartGame()
	{
		var cameraTween = GetNode<Tween>("CameraTween");

		cameraTween.InterpolateProperty(
			GetNode<Camera2D>("Camera2D"),
			"zoom",
			GetNode<Camera2D>("Camera2D").Zoom,
			new Vector2(0.3f, 0.3f),
			8,
			Tween.TransitionType.Sine,
			Tween.EaseType.Out
		);
		
		for (int i = 0; i < Players.Count; i++)
		{
			if (string.IsNullOrEmpty((string) Config.Load("name")))
				ingameNameLabel.GetNode<Label>("Label").Text = $"Player {i + 1}";
			else
			{
				if (i == 0)
					ingameNameLabel.GetNode<Label>("Label").Text = $"{Config.Load("name")} (Player {i + 1})";
				else
					ingameNameLabel.GetNode<Label>("Label").Text = $"Guest (Player {i + 1})";
			}
			((IngameNameLabel) ingameNameLabel).TargetNode = Players[i];
			cameraTween.InterpolateProperty(
				GetNode<Camera2D>("Camera2D"),
				"position",
				GetNode<Camera2D>("Camera2D").Position,
				Players[i].Position,
				4,
				Tween.TransitionType.Back,
				Tween.EaseType.Out
			);
			cameraTween.Start();
			await ToSignal(cameraTween, "tween_completed");
		}
		
		await ToSignal(cameraTween, "tween_completed");
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
		cameraTween.Start();
		await ToSignal(cameraTween, "tween_completed");
		
		((Player) Players[0]).MyTurn = true;
	}

	//If we have a username, it will be (p1) 'username', (p2) 'guest', if there is no username, it will be (p1) 'Player 1' (p2) 'Player 2'
	private string FormatPlayerTurnLabel()
	{
		if (string.IsNullOrEmpty((string) Config.Load("name")))
			return $"[center][wave amp=5 freq=2]Player {currentTurn + 1}'s turn.[/wave][/center]";
		if (currentTurn == 0)
			return $"[center][wave amp=5 freq=2]{Config.Load("name")}'s turn.[/wave][/center]";
		return "[center][wave amp=5 freq=2]Guest's turn.[/wave][/center]";
	}
}
