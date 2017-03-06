#extension GL_EXT_gpu_shader4 : enable

//uniform vec2 translation;
uniform vec2 size;
uniform float alpha;

uniform vec3 backgroundColor;
uniform vec3 borderColor;
uniform float borderSize;

varying vec2 vPos;

void main(void)
{
	vec2 pos = gl_FragCoord.xy - vPos;

	gl_FragColor = vec4(backgroundColor, alpha);
	if (borderSize > 0.5 && (
		pos.x < borderSize ||
		pos.y < borderSize ||
		pos.x > size.x - borderSize ||
		pos.y > size.y - borderSize)
		) gl_FragColor = vec4(borderColor, alpha);
}

