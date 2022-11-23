shader_type canvas_item;

uniform float scroll = 0;


void fragment () {
	COLOR = texture(TEXTURE, vec2(UV.x + scroll, UV.y));
}