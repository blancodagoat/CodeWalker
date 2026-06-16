#include "Common.hlsli"


struct VS_INPUT
{
    float4 Position : POSITION;
    float2 Texcoord : TEXCOORD0;
};
struct VS_OUTPUT
{
    float4 Position : SV_POSITION;
    float2 Texcoord : TEXCOORD0;
    float4 Colour   : COLOR0;
};

struct ParticleInstance
{
    float3 Position;
    float Rotation;
    float2 Size;
    float2 Pad0;
    float4 UVRect;
    float4 Colour;
};
StructuredBuffer<ParticleInstance> ParticleInstances : register(t0);

cbuffer VSSceneVars : register(b0)
{
    float4x4 ViewProj;
    float4x4 ViewInv;
    float3 CamPos;
    float Pad0;
};


VS_OUTPUT main(VS_INPUT input, uint iid : SV_InstanceID)
{
    VS_OUTPUT output;

    ParticleInstance p = ParticleInstances[iid];

    // rotate the unit quad corner around the view axis
    float2 q = input.Position.xy;
    float c = cos(p.Rotation);
    float s = sin(p.Rotation);
    float2 r = float2(q.x * c - q.y * s, q.x * s + q.y * c);

    // camera-facing billboard: build a view-space offset then transform to world
    float3 voffs = float3(r * p.Size, 0.0);
    float3 woffs = mul(voffs, (float3x3)ViewInv);
    float3 wpos = (p.Position - CamPos) + woffs;

    output.Position = mul(float4(wpos, 1.0), ViewProj);
    output.Texcoord = lerp(p.UVRect.xy, p.UVRect.zw, input.Texcoord);
    output.Colour = p.Colour;

    return output;
}
