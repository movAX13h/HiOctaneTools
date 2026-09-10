attribute vec2 in_pos;

uniform vec2 translation; 
uniform vec2 resolution; 
uniform vec2 offset;
uniform vec2 geometryScale;

varying vec2 vPos;

void main() 
{ 
	// Interpolate control-local logical coordinates for fills and texture UVs.
	vPos = in_pos.xy;
	gl_Position = vec4((in_pos.xy * geometryScale + translation + offset - resolution*0.5) / (resolution*0.5), 0.0, 1.0);
}
