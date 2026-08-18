
Texture2D<float4> ParticleTex : register(t0);
SamplerState TextureSS : register(s0);

struct VS_OUTPUT
{
    float4 Position : SV_POSITION;
    float2 Texcoord : TEXCOORD0;
    float4 Colour   : COLOR0;
};

float4 main(VS_OUTPUT input) : SV_TARGET
{
    return ParticleTex.Sample(TextureSS, input.Texcoord) * input.Colour;
}
