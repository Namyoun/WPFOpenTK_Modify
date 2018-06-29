#define ObjectPosition	gl_TexCoord[4]
#define WorldPosition	gl_TexCoord[5]
#define Normal			gl_TexCoord[6]

uniform sampler2D texture1;
//uniform sampler2D texture2;

uniform float LightOffset;
uniform int UseLighting;
uniform int UseSpecular;

float gray( float3 color )
{
	return .39*color.r + .50*color.g + .11*color.b;
}

float4 light( float4 color, float4 normal, float4 position, bool useSpecular, float y)
{
	float3 lightColor = float3(1, 1, 1);
	float3 lightPosition = float3(-3+3*sin(y), 4, 6);
	float3 eyePosition = float3(0, 0, 4);
	float Ka = .2;
	float Kd = 1;
	float Ks = 1;
	float shininess = 25;

	float3 P = position.xyz;
	float3 N = normalize(normal.xyz);
	if (N.z < 0)
		N.z *= -1;

	// Compute ambient component
	float3 ambient = Ka * color.rgb;

	// Compute the diffuse component
	float3 L = normalize(lightPosition - P);
	float diffuseLight = max(dot(L, N), 0f);
	float3 diffuse = Kd * lightColor * color.rgb * diffuseLight;
	float4 nonSpec = float4(ambient + diffuse, color.a);
	if (!useSpecular)
		return nonSpec;

	// Compute the specular component
	float3 V = normalize(eyePosition - P);
	float3 H = normalize(L + V);
	float specularLight = pow(max(dot(N, H), 0f), shininess);
	if (diffuseLight <= 0) 
		specularLight = 0;
	float4 specular = float4(Ks * lightColor, specularLight);
	
	// incorporate specular highlight into opacity (clear glass has opaque highlights) 
	// sum components together
	float opacity = color.a + specularLight;
	float3 c = nonSpec * (1-specularLight) + specular.rgb * specularLight;
	//float3 c = nonSpec.rgb*color.a + specular.rgb*specularLight; //(for colored lights)
	
	return float4(c, opacity);
}

void main()
{
	// use normal coloring
	//gl_FragColor = gl_Color;

	// color by texture
	gl_FragColor = texture2D(texture1, gl_TexCoord[0]);
	
	// light
	if (UseLighting)
		gl_FragColor = light(gl_FragColor, Normal, WorldPosition, UseSpecular, LightOffset);
	
	// set color to opaque to prevent transparency
	gl_FragColor.a = max(gl_FragColor.a, gl_Color.a);
}