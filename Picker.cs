using Godot;
using Crashteroids;
using System;
using System.Threading.Tasks;

public class Picker : Control
{
	public static Texture[] RocketTextures;
	
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
	
	private async void _on_Button_pressed(int _index)
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
		
		await UpdateTexture(_index);
	}
	
	private async Task UpdateTexture(int _index)
	{
		if (_index == 1)
		{
			//<summary> Come off before coming on tween </summary>
			_rocketTween.InterpolateProperty (
				_rocket, //Object
				"rect_position", //Property being tweened
				new Vector2(192, 16), //from
				new Vector2(0, 16), //to
				0.5f, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.In
			);
			_rocketTween.InterpolateProperty (
				_rocket, //Object
				"rect_rotation", //Property being tweened
				0, //from
				-20, //to
				0.5f, //speed
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
				new Vector2(400, 16), //from
				new Vector2(192, 16), //to
				0.5f, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			_rocketTween.InterpolateProperty (
				_rocket, //Object
				"rect_rotation", //Property being tweened
				-20, //from
				0, //to
				0.5f, //speed
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
				new Vector2(192, 16), //from
				new Vector2(400, 16), //to
				0.5f, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.In
			);
			_rocketTween.InterpolateProperty (
				_rocket, //Object
				"rect_rotation", //Property being tweened
				0, //from
				20, //to
				0.5f, //speed
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
				new Vector2(0, 16), //from
				new Vector2(192, 16), //to
				0.5f, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			_rocketTween.InterpolateProperty (
				_rocket, //Object
				"rect_rotation", //Property being tweened
				20, //from
				0, //to
				0.5f, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			_rocketTween.Start();
		}
		
		GameConfig.SkinID = Array.IndexOf(RocketTextures, _rocket.Texture);
	}
}
