#version 410

precision highp float;

uniform mat4 model;
uniform mat4 modelView;
uniform mat4 modelViewProjection;

uniform mat4 normalMatrix;

uniform mat4 shadowViewProjection;

uniform vec3 lightDirection;
uniform vec4 color;

in vec3 in_position;
in vec3 in_normal;

out vec3 N,Eye,Reflected,shadowPos;
out float NdotL;

void main(void)
{
	vec3 v = (modelView * vec4(in_position, 1.0)).xyz;
	N = normalize((normalMatrix * vec4(in_normal, 0.0)).xyz);
	NdotL = dot(N, -lightDirection);
	Reflected = normalize(reflect(lightDirection, N));
	Eye = normalize(-v);
	shadowPos = (shadowViewProjection * vec4(in_position, 1.0)).xyz;
	gl_Position = modelViewProjection * vec4(in_position, 1.0);
}

