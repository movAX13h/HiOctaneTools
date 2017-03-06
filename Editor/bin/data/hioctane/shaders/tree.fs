#version 410

precision highp float;

uniform sampler2D shadowMap;
uniform vec4 color;
//uniform vec3 lightDirection;

in vec3 N,Eye,Reflected,shadowPos;
in float NdotL;

out vec4 out_color;

void main(void)
{
	vec3 ambient = vec3(0.4);
	vec3 Light0_Specular = vec3(1.0);
	float shininess = 50.0;

	vec3 sp = shadowPos;
	sp.z -= 0.001;
	
	float vis = 1.0;
	if (texture2D(shadowMap, sp.xy).z < sp.z) vis = 0.8;
	
	if (NdotL > 0.0)
	{
		vec3 diffuse = NdotL * vec3(0.3, 0.3, 0.3);
		vec3 specular = pow(max(dot(Reflected, Eye), 0.0), shininess) * Light0_Specular;
		out_color = vec4((ambient + diffuse) * color.rgb + specular, color.a);
	}
	else
	{
		out_color = vec4(ambient * color.rgb, color.a);
	}
	
	out_color.rgb *= vis;
}

