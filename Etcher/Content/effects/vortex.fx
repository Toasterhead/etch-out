static const float 	DARKEST 			= 0.0;
static const float 	DARK				= 1.0 / 3.0;
static const float 	BRIGHT				= 2.0 / 3.0;
static const float 	BRIGHTEST 			= 1.0;
static const int 	TIME_DENOMINATOR 	= 30;

float 	radius;
int		timeStamp;
int 	resolutionX;
int		resolutionY;

sampler s0;

float4 Interlace(float4 position : SV_Position, float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float a = coords.x - 0.5;
	float b = coords.y - 0.5;
	float c = sqrt(pow(a, 2) + pow(b, 2));
	
	int positionX = coords.x * resolutionX;
	int positionY = coords.y * resolutionY;
	
	bool ditherPixel = (positionX % 2 == 0 && positionY % 2 == 0) || (positionX % 2 == 1 && positionY % 2 == 1);
	bool darkenPixel = (positionX % 2 == 0 && positionY % 4 == 0) || (positionX % 2 == 1 && positionY % 4 == 2);
	
	float innerRadius = (1.0 - ((float)(timeStamp % TIME_DENOMINATOR) / (float)TIME_DENOMINATOR)) * radius;
	
	if (timeStamp % 6 < 3)
	{
		ditherPixel = !ditherPixel;
		darkenPixel = (positionX % 2 == 0 && positionY % 4 == 1) || (positionX % 2 == 1 && positionY % 4 == 3);
	}
	
	if (c < 0.125 * radius)
	{
		color.rgb = DARKEST;
	}
	else if (c < 0.1875 * radius)
	{
		if (ditherPixel)
			color.rgb = DARK;
		else color.rgb = DARKEST;
	}
	else if (c < 0.25 * radius)
	{
		color.rgb = DARK;
	}
	else if (c < 0.3125 * radius)
	{
		if (ditherPixel)
			color.rgb = BRIGHT;
		else color.rgb = DARK;
	}
	else if (c < 0.375 * radius)
	{
		color.rgb = BRIGHT;
	}
	else if (c < 0.4375 * radius)
	{
		if (ditherPixel)
			color.rgb = BRIGHTEST;
		else color.rgb = BRIGHT;
	}
	else if (c < 0.5 * radius)
	{
		color.rgb = BRIGHTEST;
	}
	else color.rgb = 0.0;
	
	if (c >= innerRadius && c < innerRadius  + (0.1 * radius) && darkenPixel)
		color.rgb -= 1.0 / 3.0;
	
	//Apply base color.
	color.r *= 0.50;
	color.g *= 0.50;
	color.b *= 0.50;
	
	return color;
}

technique PostProcess
{
    pass P0
    {
        PixelShader = compile ps_4_0_level_9_3 Interlace();
    }
}