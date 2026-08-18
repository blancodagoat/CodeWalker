#include "TreeVS.hlsli"

struct VS_INPUT
{
    float4 Position  : POSITION;
    float3 Normal    : NORMAL;
    float2 Texcoord0 : TEXCOORD0;
    float4 Colour0   : COLOR0;
    float4 Colour1   : COLOR1;
};


VS_OUTPUT main(VS_INPUT input)
{
    VS_OUTPUT output;
    float3 opos = TreeModelTransform(input.Position.xyz, input.Colour0.xyz, input.Normal);
    float4 cpos = ScreenTransform(opos);
    float3 bnorm = NormalTransform(input.Normal);
    float3 btang = 0.5;

    float4 tnt = ColourTint(input.Colour1.b);

    float4 lightspacepos;
    float shadowdepth = ShadowmapSceneDepth(opos, lightspacepos);
    output.LightShadow = lightspacepos;
    output.Shadows = float4(shadowdepth, 0, 0, 0);

    output.Position = cpos;
    output.CamRelPos = opos;
    output.Normal = bnorm;
    output.Texcoord0 = input.Texcoord0;
    output.Texcoord1 = 0.5;
    output.Texcoord2 = 0.5;
    output.Colour0 = input.Colour0;
    output.Colour1 = input.Colour1;
    output.Tint = tnt;
    output.Tangent = float4(btang, 1);
    output.Bitangent = float4(cross(btang, bnorm), 0);
    return output;
}
