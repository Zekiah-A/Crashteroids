shader_type canvas_item;


uniform float speed = 0.02;


void fragment () {
	COLOR = texture(TEXTURE, vec2(UV.x + TIME * (speed / 100.0), UV.y));
}