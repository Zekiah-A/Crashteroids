using Godot;
/*using Crashteroids;*/
using System;
using System.Threading.Tasks;

public class Picker : Control
{
	public static Texture[] RocketTextures;

	private Button b1;
	private Button b2;
	private TextureRect rocket;
	private Tween rocketTween;
	private int currentIndex;

	public override void _Ready()
	{
		b1 = GetNode<Button>("Button Forward");
		b2 = GetNode<Button>("Button Back");
		rocket = GetNode<TextureRect>("Rocket");
		rocketTween = GetNode("Rocket").GetNode<Tween>("Tween");

		RocketTextures = new Texture[]
		{
			ResourceLoader.Load("res://resources/rockets/rocket_retro_1.png") as Texture,
			ResourceLoader.Load("res://resources/rockets/rocket_retro_2.png") as Texture,
			ResourceLoader.Load("res://resources/rockets/rocket_dart_1.png") as Texture
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

		await UpdateTexture(index);
	}

	private async Task UpdateTexture(int index)
	{
		if (index == 1)
		{
			//<summary> Come off before coming on tween </summary>
			rocketTween.InterpolateProperty(
				rocket, //Object
				"rect_position", //Property being tweened
				new Vector2(192, 16), //from
				new Vector2(0, 16), //to
				0.5f, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.In
			);
			rocketTween.InterpolateProperty(
				rocket, //Object
				"rect_rotation", //Property being tweened
				0, //from
				-20, //to
				0.5f, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.In
			);
			rocketTween.Start();
			///<summary> Wait for the rocket to move out of view and change texture</summary>
			await ToSignal(rocketTween, "tween_completed");
			rocket.Texture = RocketTextures[currentIndex];

			//<summary> "New" rocket comes on the screen </summary>
			rocketTween.InterpolateProperty(
				rocket, //Object
				"rect_position", //Property being tweened
				new Vector2(400, 16), //from
				new Vector2(192, 16), //to
				0.5f, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			rocketTween.InterpolateProperty(
				rocket, //Object
				"rect_rotation", //Property being tweened
				-20, //from
				0, //to
				0.5f, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			rocketTween.Start();
		}
		else
		{
			//<summary> Come off before coming on tween </summary>
			rocketTween.InterpolateProperty(
				rocket, //Object
				"rect_position", //Property being tweened
				new Vector2(192, 16), //from
				new Vector2(400, 16), //to
				0.5f, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.In
			);
			rocketTween.InterpolateProperty(
				rocket, //Object
				"rect_rotation", //Property being tweened
				0, //from
				20, //to
				0.5f, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.In
			);
			rocketTween.Start();
			///<summary> Wait for the rocket to move out of view and change texture</summary>
			await ToSignal(rocketTween, "tween_completed");
			rocket.Texture = RocketTextures[currentIndex];

			//<summary> "New" rocket comes on the screen </summary>
			rocketTween.InterpolateProperty(
				rocket, //Object
				"rect_position", //Property being tweened
				new Vector2(0, 16), //from
				new Vector2(192, 16), //to
				0.5f, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			rocketTween.InterpolateProperty(
				rocket, //Object
				"rect_rotation", //Property being tweened
				20, //from
				0, //to
				0.5f, //speed
				Tween.TransitionType.Cubic,
				Tween.EaseType.Out
			);
			rocketTween.Start();
		}

		//GameConfig.Instance.SkinID = Array.IndexOf(RocketTextures, _rocket.Texture);
	}
}
