using Godot;
using System;

public class GameManager : Node2D
{
	private void _on_Back_pressed() =>
		GetTree().ChangeScene("res://scenes/Title.tscn"); //TODO: Make match finish screen, pause, etc
}
