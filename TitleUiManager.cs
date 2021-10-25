using Godot;
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Net.Http;
using System.Xml.Serialization;
using System.Collections.Generic;

public class TitleUiManager : Control
{
	private List<Panel> panels;

	private Label moneyLabel;
	private Label editorMoneyLabel;
	private LineEdit usernameEdit;
	private RichTextLabel usernameLabel;

	private Control musicCheckbox;
	private Control sfxCheckbox;
	private Control helpBtnCheckbox;
	private Control advertisementsCheckbox;

	const int MaxUsernameLength = 10;

	public override void _Ready()
	{
		//TODO: Make all scene tree names PascalCase
		
		panels = new List<Panel>()
		{
			GetNode<Panel>("MainPanel"),
			GetNode<Panel>("SettingsPanel"),
			GetNode<Panel>("GamemodePanel"),
			GetNode<Panel>("MatchsettingsPanel"),
			GetNode<Panel>("EditorPanel"),
			GetNode<Panel>("HelpPanel"),
			GetNode("HelpPanel").GetNode<Panel>("Credits Panel")
		};

		moneyLabel = panels[0].GetNode<Label>("Money Label");
		editorMoneyLabel = panels[4].GetNode<Label>("Money Label");
		usernameEdit = panels[1].GetNode("Right Panel").GetNode<LineEdit>("Username Edit");
		usernameLabel = panels[1].GetNode("Right Panel").GetNode("Username Edit").GetNode<RichTextLabel>("Username Label");

		foreach (Panel panel in panels)
		{
			if (panel != panels[0])
				panel.Visible = false;
		}
	}

	///<summary> Handles anels switching and interaction. </summary>
	private void PanelPressed(int id)
	{
		if (id == 1)
		{
			panels[id].Visible = true;

			panels[id].GetNode<Tween>("Panel Tween").InterpolateProperty(
				panels[id], //Object
				"rect_position", //Property being tweened
				new Vector2(-1024, 0), //from
				new Vector2(0, 0), //to
				1, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			panels[id].GetNode<Tween>("Panel Tween").Start();
		}
		if (id > 1 && id < 4)
		{
			panels[id].Visible = true;

			panels[id].GetNode<Tween>("Panel Tween").InterpolateProperty(
				panels[id], //Object
				"rect_position", //Property being tweened
				new Vector2(-1024, 0), //from
				new Vector2(0, 0), //to
				1, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			panels[id].GetNode<Tween>("Panel Tween").Start();
		}
		else if (id == 4)
		{
			panels[id].Visible = true;

			panels[id].GetNode<Tween>("Panel Tween").InterpolateProperty(
				panels[id], //Object
				"rect_position", //Property being tweened
				new Vector2(0, 600), //from
				new Vector2(0, 0), //to
				1, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			panels[id].GetNode<Tween>("Panel Tween").Start();
		}
		else if (id == 5)
		{
			panels[id].Visible = true;
			panels[id].GetNode<Tween>("Panel Tween").InterpolateProperty(
				panels[id], //Object
				"rect_scale", //Property being tweened
				new Vector2(0, 0), //from
				new Vector2(1, 1), //to
				1, //speed
				Tween.TransitionType.Back,
				Tween.EaseType.Out
			);
			panels[id].GetNode<Tween>("Panel Tween").Start();
		}
		else
		{
			panels[id].Visible = true;
		}
	}

	private void BackPressed(int panelId) =>
		panels[panelId].Visible = false;
}
