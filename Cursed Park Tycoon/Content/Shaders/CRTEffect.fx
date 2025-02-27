#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TexCoord : TEXCOORD0; // Add texture coordinate
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TexCoord : TEXCOORD0; // Pass through texture coordinate
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

    output.Position = mul(input.Position, WorldViewProjection);
    output.Color = input.Color;
    output.TexCoord = input.TexCoord; // Pass texture coordinates

    return output;
}

sampler2D Texture : register(s0);
float2 Resolution : register(c0);
float ScanlineIntensity : register(c1);
float ScanlineSize : register(c2);
float Curvature : register(c3);

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 uv = input.TexCoord;
    float2 center = float2(0.5, 0.5);

    // Calculate curvature
    float2 offset = uv - center;
    float r = length(offset);
    float curvature = pow(r, Curvature);
    float2 distortedCoord = center + (offset / max(curvature, 0.001));

    // Sample the texture
    float4 color = tex2D(Texture, distortedCoord);

    // Add scanlines
    float scanline = (fmod(uv.y * Resolution.y, ScanlineSize) < 1.0) ? ScanlineIntensity : 1.0;
    color *= scanline;

    return color;
}

technique CRT
{
    pass P0
    {
        //VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
