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
		
		launchTools = ResourceLoader.Load<Texture>("res://resources/ui/tools_launch/tools_launch_button.png");
		launchPressed = ResourceLoader.Load<Texture>("res://resources/ui/tools_launch/launched_button_pressed.png");
		toolsPressed = ResourceLoader.Load<Texture>("res://resources/ui/tools_launch/tools_button_pressed.png");
		
		buttonTimer = GetNode<Timer>("ButtonTimer");

		buttonRect.Texture = launchTools;
	}
	
	private void LaunchPressed()
	{
		buttonRect.Texture = launchPressed;
		//buttonTimer.Start();
	}
	
	private void ToolsPressed()
	{
		buttonRect.Texture = toolsPressed;
		//buttonTimer.Start();
	}

	private void ButtonTimerTimeout()
	{
		buttonRect.Texture = launchTools;
	}
}
