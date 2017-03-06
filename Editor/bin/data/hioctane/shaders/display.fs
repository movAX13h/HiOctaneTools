#version 410

out vec4 out_color;

uniform float time;
uniform vec2 resolution;
uniform sampler2D painting;

uniform vec3 color; // vec3(2.2, 2.4, 2.5)
uniform float saturation; // 3.7
uniform float gamma; // 2.2


void main(void)
{
	vec2 uv = gl_FragCoord.xy / resolution;
	vec3 col = texture2D(painting, uv).xyz;
	
	//col = clamp(col, vec3(0.0), vec3(1.0));
	//col = pow(col, color) * saturation; // farbton & sättigung
	//col = pow(col, vec3(1.0 / gamma)); // gamma	
	
	out_color = vec4(col, 1.0);
}

