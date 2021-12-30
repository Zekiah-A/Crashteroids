using Godot;
using System;
using System.Text;

public class Tool : Panel
{
	public bool Bought;
	[Export] public int Price;
	
	private StyleBoxFlat defaultStyle;
	private StyleBoxFlat boughtStyle;
	private Label description;

	public override void _Ready()
	{
		defaultStyle = ResourceLoader.Load("res://styles/editoritem_available.tres") as StyleBoxFlat;
		boughtStyle = ResourceLoader.Load("res://styles/editoritem_bought.tres") as StyleBoxFlat;

		description = GetNode<Label>("Description");
	}

	public async void Buy()
	{
		Bought = true;
		description.Text = $"Bought {description.Text.Split(' ')[0]}";
		description.AddStyleboxOverride("normal", boughtStyle);

		///<summary> Tool bought animation (done via godot tweening). </summary>
		this.GetNode<Tween>("Tween").InterpolateProperty(
			this,
			"rect_scale",
			new Vector2(1, 1),
			new Vector2(1.2f, 1.2f),
			0.2f,
			Tween.TransitionType.Quad,
			Tween.EaseType.Out
		);
		this.GetNode<Tween>("Tween").Start();
		await ToSignal(this.GetNode<Tween>("Tween"), "tween_completed");
		this.GetNode<Tween>("Tween").InterpolateProperty(
			this,
			"rect_scale",
			new Vector2(1.2f, 1.2f),
			new Vector2(1, 1),
			0.5f,
			Tween.TransitionType.Back,
			Tween.EaseType.Out
		);
		this.GetNode<Tween>("Tween").Start();
	}
}
