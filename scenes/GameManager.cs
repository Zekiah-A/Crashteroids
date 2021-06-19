using Godot;
using System;

public class GameManager : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	private void _on_Back_pressed()
	{
		GetTree().ChangeScene("res://scenes/Title.tscn"); //Make match finish screen, pause, etc
	}
}



