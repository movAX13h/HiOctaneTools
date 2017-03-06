attribute vec2 in_pos;

uniform vec2 translation; 
uniform vec2 resolution; 
uniform vec2 offset;

varying vec2 vPos;

void main() 
{ 
	vPos = translation + offset;
	gl_Position = vec4((in_pos.xy + vPos - resolution*0.5) / (resolution*0.5), 0.0, 1.0); 
}