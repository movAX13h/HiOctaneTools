#version 410

#define USE_VERTEX_NORMALS 0
#define USE_COLOR_MAP 0
#define USE_NORMAL_MAP 0
#define USE_SHADOW_MAP 0

precision highp float;

uniform mat4 model;
uniform mat4 modelView;
uniform mat4 modelViewProjection;

uniform vec4 color;

in vec3 in_position;

#ifdef USE_VERTEX_NORMALS
in vec3 in_normal;
out vec3 vn;
#endif

#if USE_COLOR_MAP
in vec2 in_texcoord;
out vec2 UV;
#endif

out vec3 v;
out vec3 pos;

#ifdef USE_SHADOW_MAP
uniform mat4 shadowViewProjection;
out vec3 shadowPos;
#endif

void main(void)
{
	pos = in_position;
	v = vec3(modelView * vec4(in_position, 1.0));

	#if USE_VERTEX_NORMALS
	vn = normalize(model * vec4(in_normal, 0.0)).xyz;
	#endif
	
	//L = -lightDirection;//normalize(lightPosition - v);
	
	#if USE_COLOR_MAP || USE_NORMAL_MAP
	UV = in_texcoord;
	#endif
	
	#ifdef USE_SHADOW_MAP
	shadowPos = (shadowViewProjection * vec4(in_position, 1.0)).xyz;
	#endif
	
	gl_Position = modelViewProjection * vec4(in_position, 1.0);
}

