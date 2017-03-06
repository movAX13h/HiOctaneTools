#extension GL_EXT_gpu_shader4 : enable

uniform vec2 size;
uniform float alpha;
uniform vec3 backgroundColor;
//varying vec2 vPos;

void main(void)
{
	//vec2 pos = gl_FragCoord.xy - vPos;
	gl_FragColor = vec4(backgroundColor, alpha);
}

