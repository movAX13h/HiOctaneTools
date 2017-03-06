#version 410

#define USE_COLOR_MAP 0
#define USE_SHADOW_MAP 0

precision highp float;

uniform mat4 projectionMatrix;
uniform mat4 modelViewMatrix;

uniform vec4 color;

in vec3 in_position;

#if USE_COLOR_MAP
in vec2 in_texcoord;
out vec2 UV;
#endif

#ifdef USE_SHADOW_MAP
uniform mat4 shadowViewProjection;
out vec3 shadowPos;
#endif

void main(void)
{
	#if USE_COLOR_MAP
	UV = in_texcoord;
	#endif
	
	#if USE_SHADOW_MAP
	shadowPos = (shadowPosView * vec4(in_position, 1.0)).xyz;
	#endif
	
	gl_Position = projectionMatrix * (modelViewMatrix * vec4(0.0, 0.0, 0.0, 1.0) + 
				  vec4(in_position.x, in_position.y, 0.0, 0.0));
}

