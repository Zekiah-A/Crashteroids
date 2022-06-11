using Godot;
using System;
using System.Collections.Generic;

public class TwoPlayerGame : Node
{
	public List<Node2D> Players = new List<Node2D>();
	
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
	}
}
