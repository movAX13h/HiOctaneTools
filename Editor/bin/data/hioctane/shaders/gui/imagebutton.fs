#extension GL_EXT_gpu_shader4 : enable

//uniform vec2 translation;
uniform vec2 size;
uniform float alpha;
uniform bool down;
uniform vec3 tint;

uniform sampler2D icon;

varying vec2 vPos;

void main(void)
{
	vec2 uv = (gl_FragCoord.xy - vPos) / size;
	uv.y *= -1.0;
	vec4 col = texture2D(icon, uv);
	col.rgb *= tint;
	if (down) col.rgb *= 0.5;// = vec4(vec3(col.r), col.a);
	gl_FragColor = vec4(col.rgb, min(col.a, alpha)); //vec4(col.rgb, max(0.8, col.a));
}

