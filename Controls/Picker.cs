using Godot;

public partial class Picker : Control
{
	public static Texture2D?[] RocketTextures = null!;
	private TextureRect rocket = null!;
	private int currentIndex;
	private Control buttonLeft = null!;
	private Control buttonRight = null!;

	public override void _Ready()
	{
		rocket = GetNode<TextureRect>("Rocket");
		buttonLeft = GetNode<Control>("ButtonBack");
		buttonRight = GetNode<Control>("ButtonForward");
		
		RocketTextures = new []
		{
			ResourceLoader.Load("res://Resources/Rockets/rocket_retro_1.png") as Texture2D,
			ResourceLoader.Load("res://Resources/Rockets/rocket_retro_2.png") as Texture2D,
			ResourceLoader.Load("res://Resources/Rockets/rocket_dart_1.png") as Texture2D
		};
	}

	///<note> Forward button = 1 </note>
	private void _on_Button_pressed(int index) //TODO: Fix method casing
	{
		if (index == 1)
		{
			currentIndex = currentIndex == RocketTextures.Length - 1 ? 0 : ++currentIndex;
		}
		else
		{
			currentIndex = currentIndex <= 0 ? RocketTextures.Length - 1 : --currentIndex ;
		}

		UpdateTexture(index);
	}

	private void UpdateTexture(int index)
	{
		var tween = CreateTween()
			.SetEase(Tween.EaseType.In)
			.SetTrans(Tween.TransitionType.Cubic)
			.SetParallel();
		var parentCentre = Size / 2 - rocket.Size / 2;

		if (index == 1)
		{
			// Come off before coming on tween & wait for the rocket to move out of view to change texture
			rocket.Position = parentCentre;
			rocket.Rotation = 0;
			tween.TweenProperty(rocket, "position", buttonLeft.Position, 0.5f);
			tween.TweenProperty(rocket, "rotation", -0.3491, 0.5f);
			tween.TweenCallback(Callable.From(() =>
			{
				rocket.Texture = RocketTextures[currentIndex];
				rocket.Position = buttonRight.Position;
				rocket.Rotation = -0.3491f;
			})).SetDelay(0.5f);
			// "New" rocket comes on the screen
			tween.Chain().SetEase(Tween.EaseType.Out);
			tween.TweenProperty(rocket, "position", parentCentre, 0.5f);
			tween.TweenProperty(rocket, "rotation", 0, 0.5f);
			tween.Play();
		}
		else
		{
			// Come off before coming on tween & wait for the rocket to move out of view to change texture
			rocket.Position = parentCentre;
			rocket.Rotation = 0;
			tween.TweenProperty(rocket, "position", buttonRight.Position, 0.5f);
			tween.TweenProperty(rocket, "rotation", 0.3491, 0.5f);
			tween.TweenCallback(Callable.From(() =>
			{
				rocket.Texture = RocketTextures[currentIndex];
				rocket.Position = buttonLeft.Position;
				rocket.Rotation = -0.3491f;
			})).SetDelay(0.5f);
			// "New" rocket comes on the screen
			tween.Chain().SetEase(Tween.EaseType.Out);
			tween.TweenProperty(rocket, "position", parentCentre, 0.5f);
			tween.TweenProperty(rocket, "rotation", 0, 0.5f);
			tween.Play();
		}
	}
}
