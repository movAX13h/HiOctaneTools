#version 410
precision highp float;

uniform sampler2D shadowMap;
uniform vec4 color;

in vec3 N,Eye,Reflected,shadowPos;
in float NdotL;

out vec4 out_color;

float rand(vec3 co)
{
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5357 * co.z);
}

void main(void)
{
	float specFalloff = 30.0;
	vec3 specCol = vec3(0.1);
	vec3 ambientCol = vec3(0.6);
	float r = rand(gl_FragCoord.xyz);

	vec3 sp = shadowPos;
	sp.z -= 0.001;
	
	float vis = 1.0;
	if (texture2D(shadowMap, sp.xy).z < sp.z) vis = 0.8;
	
	if (NdotL > 0.0)
	{
		vec3 diffuse = NdotL * vec3(0.3);
		vec3 specular = pow(max(dot(Reflected, Eye), 0.0), specFalloff + 20.0*r) * specCol;
		out_color = vec4((ambientCol + diffuse) * color.rgb + specular, color.a);
	}
	else
	{
		out_color = vec4(ambientCol * color.rgb, color.a);
	}
	
	out_color.rgb *= vis;
}

