#version 410
precision highp float;

uniform vec4 color;
uniform float time;

in vec3 Pos;

out vec4 out_color;

const float pi = 3.1415927; const int NUM_OCTAVES = 4; 

float hash(vec2 p)
{
	return fract(1e4 * sin(17.0 * p.x + p.y * 0.1) * (0.1 + abs(sin(p.y * 13.0 + p.x))));
}

float noise(float x) 
{
	float i = floor(x); float f = fract(x); float u = f * f * (3.0 - 2.0 * f);
	return mix(hash(vec2(i)), hash(vec2(i + 1.0)), u);
}

float noise(vec2 x)
{
	vec2 i = floor(x); 
	vec2 f = fract(x);
	float a = hash(i); 
	float b = hash(i + vec2(1.0, 0.0)); 
	float c = hash(i + vec2(0.0, 1.0)); 
	float d = hash(i + vec2(1.0, 1.0)); 
	vec2 u = f * f * (3.0 - 2.0 * f); 
	return mix(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
}

float NOISE(vec2 x)
{
	float v = 0.0; 
	float a = 0.5; 
	vec2 shift = vec2(100 + 0.04*time);
	for (int i = 0; i < NUM_OCTAVES; ++i) 
	{
		v += a*noise(x); 
		x = x * 2.0 + shift; a *= 0.5;
	} 
	return v;
}

float square(float x) { return x * x; }

vec3 nebula(vec3 dir) 
{
	float purple = abs(dir.x); 
	float yellow = noise(dir.y);
	vec3 streakyHue = vec3(purple + yellow, yellow * 0.7, purple); 
	vec3 puffyHue = vec3(0.8, 0.1, 1.0); 
	float streaky = min(1.0, 8.0 * pow(NOISE(dir.yz*square(dir.x) * 13.0 + dir.xy * square(dir.z) * 7.0 + vec2(150.0, 2.0)), 10.0)); 
	float puffy = square(NOISE(dir.xz * 4.0 + vec2(30, 10)) * dir.y);
	//return pow(clamp(puffyHue * puffy * (1.0 - streaky) + streaky * streakyHue, 0.0, 1.0), 1.0 / 2.2);
	return clamp(puffyHue * puffy * (1.0 - streaky) + streaky * streakyHue, 0.0, 1.0);
}

void main(void)
{
	vec3 col = clamp(1.4*nebula(normalize(Pos)), vec3(0.0), vec3(1.0));
	out_color = vec4(col, 1.0);
}

