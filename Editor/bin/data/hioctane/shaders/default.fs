#version 130

precision highp float;

uniform vec4 color;
out vec4 out_frag_color;

void main(void)
{
	out_frag_color = color; // vec4(color, 1.0);
}
