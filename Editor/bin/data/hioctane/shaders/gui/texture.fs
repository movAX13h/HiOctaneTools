#extension GL_EXT_gpu_shader4 : enable

uniform vec2 size;
uniform sampler2D texture;
uniform vec2 translation;
uniform float alpha;

varying vec2 vPos;

void main(void)
{
	vec2 uv = (gl_FragCoord.xy - vPos) / size;
	uv.y *= -1.0;
	vec4 col = texture2D(texture, uv);
	gl_FragColor = vec4(col.rgb, min(col.a, alpha)); //vec4(col.rgb, max(0.8, col.a));
}