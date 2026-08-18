#include "Common.hlsli"

cbuffer VSSceneVars : register(b0)
{
    float4x4 WorldViewProj;
}

float4 main(float4 pos : POSITION) : SV_POSITION
{
    float4 cpos = mul(float4(pos.xyz, 1), WorldViewProj);
    cpos.z = DepthFunc(cpos.zw);
    return cpos;
}
