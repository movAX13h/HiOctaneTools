#version 410

out vec4 out_color;

uniform float time;
uniform vec2 resolution;
uniform sampler2D painting;

uniform float threshold;
uniform vec4 color;

void main(void)
{
	vec2 uv = gl_FragCoord.xy / resolution;
	vec3 d = vec3(1.0 / resolution, 0.0);
	
	vec3 col = texture2D(painting, uv).rgb;
	
	vec3 diff = vec3(0.0);	
	diff += abs(col - texture2D(painting, uv + d.xy).rgb);
	diff += abs(col - texture2D(painting, uv + d.xz).rgb);
	diff += abs(col - texture2D(painting, uv + vec2(d.x, -d.y)).rgb);
	diff += abs(col - texture2D(painting, uv - d.zy).rgb);
	diff += abs(col - texture2D(painting, uv - d.xy).rgb);
	diff += abs(col - texture2D(painting, uv - d.xz).rgb);
	diff += abs(col - texture2D(painting, uv + vec2(-d.x, d.y)).rgb);
	diff += abs(col - texture2D(painting, uv + d.zy).rgb);
	
	float v = smoothstep(0.0, threshold, (diff.r + diff.g + diff.b) / 3.0);

	col = mix(col, color.rgb, min(color.a, v));
	out_color = vec4(col, 1.0);
}

