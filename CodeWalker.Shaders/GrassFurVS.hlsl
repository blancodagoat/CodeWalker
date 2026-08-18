#include "GrassFur.hlsli"

VS_OUTPUT main(VS_INPUT_PNCTTTX input, uint iid : SV_InstanceID)
{
    float3 cpos = FurWorldTransform(input.Position.xyz, input.Normal.xyz, iid);
    float4 spos = mul(float4(cpos, 1), ViewProj);
    spos.z = DepthFunc(spos.zw);
    float3 norm = FurNormalTransform(input.Normal.xyz);
    float4 tan0 = FurTangentTransform(input.Tangent0);

    VS_OUTPUT output;
    output.Position = spos;
    output.Normal = norm;
    output.Colour0 = input.Colour0;
    output.Texcoord0 = float3(input.Texcoord0, (float)iid);
    output.Texcoord1 = input.Texcoord1;
    output.Texcoord2 = input.Texcoord2;
    output.Tangent0 = tan0;
    output.CamRelPos = cpos;
    return output;
}
