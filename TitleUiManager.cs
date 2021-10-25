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
			GetNode<Panel>("ShopPanel"),
			GetNode<Panel>("HelpPanel"),
			GetNode("HelpPanel").GetNode<Panel>("Credits Panel")
		};

		moneyLabel = panels[0].GetNode<Label>("Money Label");
		//TODO: Put in editor class, it will handle itself, or make a separate script for the label //editorMoneyLabel = panels[4].GetNode<Label>("Money Label");

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
			///<summary> Object, Property being tweened, From, To, Time, Ease type, Tween type </summary>
			panels[id].Visible = true;

			panels[id].GetNode<Tween>("Panel Tween").InterpolateProperty(
				panels[id],
				"rect_position",
				new Vector2(-1024, 0),
				new Vector2(0, 0),
				1,
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			panels[id].GetNode<Tween>("Panel Tween").Start();
		}
		if (id > 1 && id < 4)
		{
			panels[id].Visible = true;

			panels[id].GetNode<Tween>("Panel Tween").InterpolateProperty(
				panels[id],
				"rect_position",
				new Vector2(-1024, 0),
				new Vector2(0, 0),
				1,
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			panels[id].GetNode<Tween>("Panel Tween").Start();
		}
		else if (id == 4)
		{
			panels[id].Visible = true;

			panels[id].GetNode<Tween>("Panel Tween").InterpolateProperty(
				panels[id],
				"rect_position",
				new Vector2(0, 600),
				new Vector2(0, 0),
				1,
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			panels[id].GetNode<Tween>("Panel Tween").Start();
		}
		else if (id == 5)
		{
			panels[id].Visible = true;
			panels[id].GetNode<Tween>("Panel Tween").InterpolateProperty(
				panels[id],
				"rect_scale",
				new Vector2(0, 0),
				new Vector2(1, 1),
				1,
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
