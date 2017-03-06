#extension GL_EXT_gpu_shader4 : enable

//uniform vec2 translation;
uniform vec2 size;
uniform float alpha;

uniform vec3 backgroundColor;
uniform vec3 borderColor;
uniform float borderSize;
uniform float cornerRound;

varying vec2 vPos;

float udRoundBox( vec2 p, vec2 b, float r ) { return length(max(abs(p)-b+r,0.0))-r; }

void main(void)
{
	vec2 pos = gl_FragCoord.xy - vPos;

	vec4 color = vec4(backgroundColor, alpha);
	
	vec2 s = size - borderSize * 2.0;
	float d = udRoundBox(pos - 0.5*size, 0.5*s, cornerRound);
	color.rgb = mix(color.rgb, borderColor, smoothstep(0.001, 1.6, d));
	color.a *= smoothstep(borderSize + 1.6, borderSize + 0.001, d);
	
	gl_FragColor = color;
}

