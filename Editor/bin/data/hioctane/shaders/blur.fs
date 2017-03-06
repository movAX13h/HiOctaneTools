#version 410

out vec4 out_color;

uniform float time;
uniform vec2 resolution;
uniform sampler2D painting;

uniform vec2 intensity;

const int gaussRadius = 11;
const float gaussFilter[gaussRadius] = float[gaussRadius](0.0402,0.0623,0.0877,0.1120,0.1297,0.1362,0.1297,0.1120,0.0877,0.0623,0.0402);

void main(void)
{
	//vec2 intensity = vec2(1.0/resolution);
	
	vec2 uv = gl_FragCoord.xy / resolution;
	vec2 texCoord = uv - float(int(gaussRadius/2)) * intensity;
	vec3 color = vec3(0.0);
	
	for (int i=0; i < gaussRadius; i++) 
	{ 
		color += gaussFilter[i] * texture2D(painting, texCoord).xyz;
		texCoord += intensity;
	}
	
	out_color = vec4(color, 1.0);
}

