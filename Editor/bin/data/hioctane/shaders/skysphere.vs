#version 410

precision highp float;

uniform mat4 model;
uniform mat4 modelView;
uniform mat4 modelViewProjection;

in vec3 in_position;

out vec3 Pos;

void main(void)
{
	Pos = in_position;
	gl_Position = modelViewProjection * vec4(in_position, 1.0);
}

