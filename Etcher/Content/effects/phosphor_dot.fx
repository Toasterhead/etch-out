static const int 	TRIAD_WIDTH		= 2;
static const int 	TRIAD_HEIGHT 	= 2;
static const float	MAX_BRIGHTNESS	= 1.0;
static const float  BOOST_RATIO		= 0.2;

float2 	surfaceDimensions;
float 	brightness;

sampler s0;

float4 PhosphorDot(float4 position : SV_Position, float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	int positionX = (int)(coords.x * surfaceDimensions.x);
	int positionY = (int)(coords.y * surfaceDimensions.y);
	
	if (brightness > MAX_BRIGHTNESS)
		brightness = MAX_BRIGHTNESS;
	
	float boost = brightness * BOOST_RATIO;
	
	color = tex2D(s0, coords);

	if (		positionX % TRIAD_WIDTH == 0 && positionY % TRIAD_HEIGHT == 0)
	{
		color.r += boost * (1.0 - color.r);
		color.gb = 0.0;
	}
	else if (	positionX % TRIAD_WIDTH == 1 && positionY % TRIAD_HEIGHT == 0)
	{
		color.g += boost * (1.0 - color.g);
		color.rb = 0.0;
	}
	else if (	positionX % TRIAD_WIDTH == 0 && positionY % TRIAD_HEIGHT == 1)
	{
		color.b += boost * (1.0 - color.b);
		color.rg = 0.0;
	}
	else color *= 1.0;

	return color;
}

technique PostProcess
{
    pass P0 { PixelShader = compile ps_4_0_level_9_3  PhosphorDot(); }
}