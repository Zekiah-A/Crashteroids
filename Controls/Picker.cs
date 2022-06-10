using Godot;
/*using Crashteroids;*/
using System;
using System.Threading.Tasks;

public class Picker : Control
{
	public static Texture[] RocketTextures;
	private TextureRect rocket;
	private Tween rocketTween;
	private int currentIndex;

	public override void _Ready()
	{
		rocket = GetNode<TextureRect>("Rocket");
		rocketTween = GetNode("Rocket").GetNode<Tween>("Tween");

		RocketTextures = new []
		{
			ResourceLoader.Load("res://Resources/rockets/rocket_retro_1.png") as Texture,
			ResourceLoader.Load("res://Resources/rockets/rocket_retro_2.png") as Texture,
			ResourceLoader.Load("res://Resources/rockets/rocket_dart_1.png") as Texture
		};
	}

	private async void _on_Button_pressed(int index)
	{
		///<note> Forward button = 1 </note>
		if (index == 1)
		{
			if (currentIndex == RocketTextures.Length - 1)
				currentIndex = 0;
			else
				currentIndex++;
		}
		else
		{
			if (currentIndex <= 0)
				currentIndex = RocketTextures.Length - 1;
			else
				currentIndex--;
		}

		//await ToSignal(rocketTween, "tween_completed");
		await UpdateTexture(index);
	}

	private async Task UpdateTexture(int index)
	{
		if (index == 1)
		{
			//<summary> Come off before coming on tween </summary>
			rocketTween.InterpolateProperty(
				rocket,
				"rect_position",
				new Vector2(192, 16),
				new Vector2(0, 16),
				0.5f,
				Tween.TransitionType.Cubic,
				Tween.EaseType.In
			);
			rocketTween.InterpolateProperty(
				rocket,
				"rect_rotation",
				0,
				-20,
				0.5f,
				Tween.TransitionType.Cubic,
				Tween.EaseType.In
			);
			rocketTween.Start();
			///<summary> Wait for the rocket to move out of view and change texture</summary>
			await ToSignal(rocketTween, "tween_completed");
			rocket.Texture = RocketTextures[currentIndex];

			//<summary> "New" rocket comes on the screen </summary>
			rocketTween.InterpolateProperty(
				rocket,
				"rect_position",
				new Vector2(400, 16),
				new Vector2(192, 16),
				0.5f,
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			rocketTween.InterpolateProperty(
				rocket,
				"rect_rotation",
				-20, 
				0,
				0.5f,
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			rocketTween.Start();
		}
		else
		{
			//<summary> Come off before coming on tween </summary>
			rocketTween.InterpolateProperty(
				rocket,
				"rect_position",
				new Vector2(192, 16),
				new Vector2(400, 16),
				0.5f,
				Tween.TransitionType.Cubic,
				Tween.EaseType.In
			);
			rocketTween.InterpolateProperty(
				rocket, 
				"rect_rotation",
				0,
				20,
				0.5f, 
				Tween.TransitionType.Cubic,
				Tween.EaseType.In
			);
			rocketTween.Start();
			///<summary> Wait for the rocket to move out of view and change texture</summary>
			await ToSignal(rocketTween, "tween_completed");
			rocket.Texture = RocketTextures[currentIndex];

			//<summary> "New" rocket comes on the screen </summary>
			rocketTween.InterpolateProperty(
				rocket,
				"rect_position",
				new Vector2(0, 16),
				new Vector2(192, 16),
				0.5f,
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			rocketTween.InterpolateProperty(
				rocket,
				"rect_rotation",
				20,
				0,
				0.5f,
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			rocketTween.Start();
		}
	}
}
