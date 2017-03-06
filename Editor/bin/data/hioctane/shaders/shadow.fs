#version 410

// note: this shader is not called because there is no color buffer attached
precision highp float;

out vec4 out_color;

void main(void)
{
	out_color = vec4(1.0);
}

