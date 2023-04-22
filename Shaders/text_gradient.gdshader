shader_type canvas_item;
render_mode unshaded;
 
uniform float stretch;
uniform bool horizontal;
uniform float r;
uniform float g;
uniform float b;
uniform float height;

void fragment()
{
	float texture_alpha = texture(TEXTURE, UV).a;

	if (texture_alpha > 0.0)
	{
		if (horizontal)
		{
			COLOR = vec4(
				UV.x * r * stretch,
				UV.x * g * stretch,
				UV.x * b * stretch,
				texture_alpha
			);
		}
		else
		{
			COLOR = vec4(
				UV.y * r * stretch,
				UV.y * g * stretch,
				UV.y * b * stretch,
				texture_alpha
			);
		}
	}
	else
	{ 
		COLOR = texture(TEXTURE, UV);
	}
}