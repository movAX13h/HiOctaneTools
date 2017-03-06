#version 410

#define USE_VERTEX_NORMALS 0
#define USE_COLOR_MAP 0
#define USE_NORMAL_MAP 0
#define USE_SHADOW_MAP 0

precision highp float;

uniform mat4 model;

uniform vec3 lightDirection;
uniform vec4 color;
uniform float opacity;
uniform float grayscale;
uniform float wireframe;

#if USE_COLOR_MAP
uniform sampler2D colorMap;
#endif

#if USE_NORMAL_MAP
uniform sampler2D normalMap;
#endif

#if USE_SHADOW_MAP
uniform sampler2D shadowMap;
in vec3 shadowPos;
#endif

#if USE_COLOR_MAP || USE_NORMAL_MAP
in vec2 UV;
#endif

#if USE_VERTEX_NORMALS
in vec3 vn;
#endif

in vec3 v;
in vec3 pos;


out vec4 out_color;

void main(void)
{

	vec3 ambient = vec3(0.4);
	vec3 lightSpecularColor = vec3(1.0);
	float specFalloff = 30.0;
	
	vec3 normal = vec3(0.0, 1.0, 0.0);

	#if USE_VERTEX_NORMALS
	normal = vn;
	#endif

	#if USE_NORMAL_MAP
	//normal = 2.0 * texture2D(normalMap, UV).rgb - 1.0;
	#endif
	
	vec3 N = normalize(model * vec4(normal, 0.0)).xyz;
	vec3 texel = color.rgb;
	
	#if USE_COLOR_MAP
	texel = texture2D(colorMap, UV).rgb;
	texel = mix(texel, color.rgb, color.a);
	#endif

	if (grayscale > 0.01)
	{
		float m = (texel.r + texel.g + texel.b) * 0.33333333;
		texel = mix(texel, vec3(m), grayscale);
	}

	//out_color.rgb = texel; out_color.a = 1.0; return;

	float NdotL = dot(N, -lightDirection);
	
	if (NdotL > 0.0)
	{
		vec3 Reflected = normalize(reflect(lightDirection, N));
		vec3 Eye = normalize(-v);
	
		vec3 diffuse = NdotL * vec3(1.0);
		vec3 specular = pow(max(dot(Reflected, Eye), 0.0), specFalloff) * lightSpecularColor;
		out_color = vec4((ambient + diffuse + specular) * texel, opacity);
	}
	else
	{
		out_color = vec4(ambient * texel, opacity);
	}

	out_color = clamp(out_color, vec4(0.0), vec4(1.0));

	if (wireframe > 0)
	{
		float d = smoothstep(30.0, 20.0, length(v));

		float onLine = d * wireframe * max(
							smoothstep(0.04, 0.0, abs(fract(pos.x+0.5)-0.5)),
							smoothstep(0.04, 0.0, abs(fract(pos.z+0.5)-0.5)) );

		out_color = mix(out_color, vec4(1.0, 1.0, 1.0, 1.0), max(0, onLine - 0.5));
	}

	#if USE_SHADOW_MAP
	vec3 sp = shadowPos;
	sp.z -= 0.001;
	if (texture2D(shadowMap, sp.xy).z < sp.z) out_color.rgb *= 0.8;
	#endif

}

