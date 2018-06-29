#define ObjectPosition	gl_TexCoord[4]
#define WorldPosition	gl_TexCoord[5]
#define Normal			gl_TexCoord[6]

uniform float ScaleASD;
uniform vec4 colorX;
uniform vec4 colorY;
uniform vec4 colorZ;

uniform float phase;
uniform float frequency;
uniform float wrippleScale;

void main()
{
	// color
	vec4 color =
		gl_Vertex.x * colorX +
		gl_Vertex.y * colorY +
		gl_Vertex.z * colorZ;
	gl_FrontColor.rgb = color.rgb + .75;
	
	// apply wripple
	gl_Vertex.x += cos(gl_Vertex.y*frequency + phase) * wrippleScale;
	gl_Vertex.y += sin(gl_Vertex.x*frequency - phase) * wrippleScale;
	gl_Vertex.z += cos(gl_Vertex.y*frequency - phase) * wrippleScale;

	// pass on the values
	ObjectPosition = gl_Vertex;
	Normal.xyz = normalize(gl_NormalMatrix * gl_Normal);
	gl_TexCoord[0] = gl_MultiTexCoord0;
	
	// position
	gl_Position = WorldPosition = ftransform();
}