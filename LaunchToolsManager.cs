using Godot;
using System;

public class LaunchToolsManager : Control
{
	private TextureRect buttonRect;
	private Texture launchTools;
	private Texture launchPressed;
	private Texture toolsPressed;
	private Timer buttonTimer;
	
	public override void _Ready()
	{
		buttonRect = GetNode("Panel").GetNode<TextureRect>("TextureRect");
		
		launchTools = ResourceLoader.Load<Texture>();
		launchPressed = ResourceLoader.Load<Texture>();
		toolsPressed = ResourceLoader.Load<Texture>();
		
		buttonTimer = GetNode<Timer>("ButtonTimer");
	}
	
	private void LaunchPressed()
	{
		
	}
	
	private void ToolsPressed()
	{
		
	}
}
