static const float	MAX_BRIGHTNESS	= 1.0;
static const float  BOOST_RATIO		= 0.15;

float brightness;

sampler s0;
sampler s1;

float4 StaticEffect(float4 position : SV_Position, float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	if (brightness > MAX_BRIGHTNESS)
		brightness = MAX_BRIGHTNESS;
	
	float boost = brightness * BOOST_RATIO;

	color = tex2D(s0, coords);
	
	float4 	noise 			= tex2D(s1, coords);
	float 	noiseBrightness = (noise.r + noise.g + noise.b) / 3.0;
	noiseBrightness *= 0.15 + (BOOST_RATIO - boost);
	
	color.rgb -= noiseBrightness * color.rgb;

	return color;
}

technique PostProcess
{
    pass P0 { PixelShader = compile ps_4_0_level_9_3 StaticEffect(); }
}