using Godot;
using System;
using Crashteroids;
using System.Collections.Generic;

public class LaunchToolsManager : Control
{
	[Export] public float PressLength = 0.1f;
	
	private Panel toolsPanel;
	private Tween panelTween;
	private Label emptyWarningLabel;
	private PackedScene itemButton;

	public override void _Ready()
	{
		toolsPanel = GetNode<Panel>("ToolsPanel");
		panelTween = GetNode<Tween>("PanelTween");
		emptyWarningLabel = toolsPanel.GetNode<Label>("EmptyWarningLabel");
		itemButton = ResourceLoader.Load<PackedScene>("res://scenes/ItemButton.tscn");

		toolsPanel.Visible = false;

		//TODO: Stuff can't be added during the match (for now, so just add items to this (and hide the hidden message))
		if (GameConfig.Instance.BoughtItems.Count != 0)
		{
			emptyWarningLabel.Visible = false;

			foreach (int id in GameConfig.Instance.BoughtItems)
			{
				Panel newButton = itemButton.Instance<Panel>();
				toolsPanel.GetNode("ScrollContainer").GetNode("GridContainer").AddChild(newButton);
				newButton.RectMinSize = new Vector2(96, 96); //TODO: Make a set const for this?
				newButton.GetNode<Label>("Description").Text = Enum.GetName(typeof(EditorIds), id);
				try { //experimental!
					newButton.GetNode<TextureRect>("IconTexture").Texture = ResourceLoader.Load<Texture>($"res://resources/tools/{Enum.GetName(typeof(EditorIds), id).ToLower()}.png");
				} catch {/*fix later - maybe a "null / error" image here?*/}
			}
		}
		else
			emptyWarningLabel.Visible = true;
	}
	
	private void LaunchPressed()
	{

	}
	
	private void ToolsPressed()
	{
		if (toolsPanel.Visible)
			toolsPanel.Visible = false;
			//tween and fade in
		else
		{
			toolsPanel.Visible = true;
			
			panelTween.InterpolateProperty(
				toolsPanel, //Object
				"rect_position", //Property being tweened
				new Vector2(256, 600), //from
				new Vector2(256, 296), //to
				1, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			panelTween.Start();
		}
	}
}
