#define ObjectPosition	gl_TexCoord[4]
#define WorldPosition	gl_TexCoord[5]
#define Normal			gl_TexCoord[6]

void main()
{
	gl_Position = WorldPosition = ftransform();
    gl_FrontColor = gl_Color;
	ObjectPosition = gl_Vertex;
	Normal.xyz = normalize(gl_NormalMatrix * gl_Normal);
	gl_TexCoord[0] = gl_MultiTexCoord0;
}