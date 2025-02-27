#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;

// Sampler for the texture
sampler2D TextureSampler : register(s0);

// Parameters for the vignette effect
float2 screenCenter;
float radius;
float intensity;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TexCoord : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

    output.Position = mul(input.Position, WorldViewProjection);
    output.Color = input.Color;
    output.TexCoord = input.TexCoord;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    // Sample the texture
    float4 color = tex2D(TextureSampler, input.TexCoord);

    // Calculate the distance from the edges of the screen
    float2 uv = input.TexCoord;
    float2 positionFromCenter = abs(uv - screenCenter);

    // Apply rectangular vignette effect by blending horizontal and vertical distances
    float vignetteX = smoothstep(radius, radius - intensity, positionFromCenter.x);
    float vignetteY = smoothstep(radius, radius - intensity, positionFromCenter.y);
    float vignette = vignetteX * vignetteY;

    // Apply the vignette effect
    color.rgb *= vignette;

    return color;
}

technique Vignette
{
    pass P0
    {
        //VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};