#include "GrassFur.hlsli"

PS_OUTPUT main(VS_OUTPUT input)
{
    float4 albo = GetFurAlbedo(input.Texcoord0, input.Texcoord1, input.Texcoord2, input.Colour0);

    // simple lighting
    float3 norm = normalize(input.Normal);
    float lf = saturate(dot(norm, GlobalLights.LightDir));
    float3 c = GlobalLighting(albo.rgb, norm, input.Colour0, lf, GlobalLights);

    PS_OUTPUT output;
    output.Colour = float4(c, albo.a);
    return output;
}
