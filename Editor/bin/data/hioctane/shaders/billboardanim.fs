#version 410

#define USE_COLOR_MAP 0
#define USE_SHADOW_MAP 0

precision highp float;

#if USE_COLOR_MAP
uniform sampler2D colorMap;
in vec2 UV;
#endif

#if USE_SHADOW_MAP
uniform sampler2D shadowMap;
in vec3 shadowPos;
#endif

out vec4 out_color;

void main(void)
{
	vec4 ambient = vec4(0.4, 0.4, 0.4, 1.0);
	vec4 texel = vec4(1.0, 0.0, 0.0, 1.0);	

	#if USE_COLOR_MAP
	texel = texture2D(colorMap, UV);
	#endif

	out_color = texel;

	#if USE_SHADOW_MAP
	vec3 sp = shadowPos;
	sp.z -= 0.001;
	if (texture2D(shadowMap, sp.xy).z < sp.z) out_color.rgb *= 0.8;
	#endif
}

