using Godot;
using System;

public class Picker : Control
{
	public Texture Current;
	public Texture[] RocketTextures;
	
	private Button _1;
	private Button _2;
	private TextureRect _rocket;
	private Tween _rocketTween;
	private int _currentIndex;
	
	public override void _Ready()
	{
		_1 = GetNode<Button>("Button Forward");
		_2 = GetNode<Button>("Button Back");
		_rocket = GetNode<TextureRect>("Rocket");
		_rocketTween = GetNode("Rocket").GetNode<Tween>("Tween");
		
		RocketTextures = new Texture[]
		{
			ResourceLoader.Load("res://resources/rockets/rocket_retro_1.png") as Texture,
			ResourceLoader.Load("res://resources/rockets/rocket_retro_2.png") as Texture,
			ResourceLoader.Load("res://resources/rockets/rocket_dart_1.png") as Texture
		};
	}
	
	private void _on_Button_pressed(int _index)
	{
		
		///<note> Forward button = 1 </note>
		if (_index == 1)
		{
			if (_currentIndex == RocketTextures.Length - 1)
				_currentIndex = 0;
			else
				_currentIndex++;
		}
		else
		{
			if (_currentIndex <= 0)
				_currentIndex = RocketTextures.Length - 1;
			else
				_currentIndex--;
		}
		UpdateTexture(_index);
	}
	
	private async void UpdateTexture(int _index)
	{
		if (_index == 1)
		{
			//<summary> Come off before coming on tween </summary>
			_rocketTween.InterpolateProperty (
				_rocket, //Object
				"rect_position", //Property being tweened
				new Vector2(192, 24), //from
				new Vector2(0, 24), //to
				1, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.In
			);
			_rocketTween.Start();
			///<summary> Wait for the rocket to move out of view and change texture</summary>
			await ToSignal(_rocketTween, "tween_completed");
			_rocket.Texture = RocketTextures[_currentIndex];
			
			//<summary> "New" rocket comes on the screen </summary>
			_rocketTween.InterpolateProperty (
				_rocket, //Object
				"rect_position", //Property being tweened
				new Vector2(415, 24), //from
				new Vector2(192, 24), //to
				1, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			_rocketTween.Start();
		}
		else
		{
			//<summary> Come off before coming on tween </summary>
			_rocketTween.InterpolateProperty (
				_rocket, //Object
				"rect_position", //Property being tweened
				new Vector2(192, 24), //from
				new Vector2(415, 24), //to
				1, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.In
			);
			_rocketTween.Start();
			///<summary> Wait for the rocket to move out of view and change texture</summary>
			await ToSignal(_rocketTween, "tween_completed");
			_rocket.Texture = RocketTextures[_currentIndex];
			
			//<summary> "New" rocket comes on the screen </summary>
			_rocketTween.InterpolateProperty (
				_rocket, //Object
				"rect_position", //Property being tweened
				new Vector2(0, 24), //from
				new Vector2(192, 24), //to
				1, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			_rocketTween.Start();
		}
	}
}
