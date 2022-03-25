shader_type canvas_item;

uniform float alpha;

void fragment() {
	COLOR = vec4(texture(TEXTURE, UV) * alpha);
}