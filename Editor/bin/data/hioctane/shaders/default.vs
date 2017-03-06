#version 130

precision highp float;

uniform mat4 projection_matrix;
uniform mat4 modelview_matrix;

in vec3 in_position;

void main(void)
{
	gl_Position = projection_matrix * modelview_matrix * vec4(in_position, 1);
}
