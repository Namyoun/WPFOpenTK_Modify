#define ObjectPosition	gl_TexCoord[4]
#define WorldPosition	gl_TexCoord[5]
#define Normal			gl_TexCoord[6]

uniform int pattern;

int pattern1(vec3 position)
{
    vec3 remainder;
    float checkerSize = .1;
    float halfChecker = checkerSize / 2.0;
    remainder = mod(position, checkerSize);
    remainder = abs(remainder);
    
    return (remainder.x > halfChecker) ^ 
		(remainder.y > halfChecker) ^ 
		(remainder.z > halfChecker);
}

int pattern1a(vec3 position)
{
    vec3 remainder;
    float3 checkerSize = .2 * (position/2 + 1);
    float3 halfChecker = checkerSize / 2.0;
    remainder = mod(position, checkerSize);
    remainder = abs(remainder);
    
    return (remainder.x > halfChecker.x) ^ 
		(remainder.y > halfChecker.y) ^ 
		(remainder.z > halfChecker.z);
}

int pattern2(vec3 position)
{
	bool circles = mod(length(position), .15) > .075;
	if ((position.x > 0) ^ 
		(position.y > 0) ^ 
		(position.z > 0))
		circles = !circles;
	return circles;
}

int pattern3(vec3 position)
{
	position.x += .5*position.y*position.y*position.y*position.z;
	position.y += .5*position.y*position.x*position.x*position.x;
	bool circles = mod(length(position), .15) > .13;
	if ((position.x > 0) ^ 
		(position.y > 0) ^ 
		(position.z > 0))
		circles = !circles;
	return circles;
}

int pattern4(vec3 position)
{
	bool circles = pattern3(position) ^ pattern2(position*position);
	return circles;
}

int patternChooser(vec3 position)
{
	if (pattern == 0)
		return 1;
	else if (pattern == 1)
		return pattern1(position);
	else if (pattern == 2)
		return pattern2(position);
	else if (pattern == 3)
		return pattern3(position);
	else if (pattern == 4)
		return pattern4(position);
	return 0;
}

void main()
{
	float d = .0005;
	float avg = // 1x, 3x, or 9x antialiasing
		patternChooser(ObjectPosition.xyz + vec3( d, d, d)) + 
		//patternChooser(ObjectPosition.xyz + vec3( d, d,-d)) + 
		//patternChooser(ObjectPosition.xyz + vec3( d,-d, d)) + 
		//patternChooser(ObjectPosition.xyz + vec3( d,-d,-d)) + 
		//patternChooser(ObjectPosition.xyz + vec3(-d, d, d)) + 
		//patternChooser(ObjectPosition.xyz + vec3(-d, d,-d)) + 
		//patternChooser(ObjectPosition.xyz + vec3(-d,-d, d)) + 
		patternChooser(ObjectPosition.xyz + vec3(-d,-d,-d)) + 
		patternChooser(ObjectPosition.xyz + vec3(0,0,0));
	avg /= 3.0;
	vec4 color = abs(vec4(ObjectPosition.xyz, 1));
	gl_FragColor = mix(color, vec4(0,0,0,1), avg);
	
}