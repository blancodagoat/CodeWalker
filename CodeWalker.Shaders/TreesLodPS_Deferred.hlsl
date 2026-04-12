#include "TreesLodPS.hlsli"


PS_OUTPUT main(VS_OUTPUT input)
{
    //return float4(1,0,0,1);//red

    float4 c = 0; // float4(input.Colour.rgb, 1);
    //return c;

    if (EnableTexture == 1)
    {
        //c = Colourmap.SampleLevel(TextureSS, input.Texcoord, 0);
        c = Colourmap.Sample(TextureSS, input.Texcoord);
        if (c.a <= 0.25) discard;
        c.a = 1;
            // c = float4(input.Colour.rgb, 1);
    }

    float3 norm = input.Normal;


    float3 spec = 0;

    c.a = saturate(c.a);

    float3 dbgRgb;
    if (GetVertexDebugColour(RenderMode, RenderModeIndex,
                             norm, float3(0, 0, 0),
                             input.Colour, input.Colour1,
                             input.Texcoord, input.Texcoord1, input.Texcoord2,
                             dbgRgb))
    {
        c = float4(dbgRgb, 1);
    }

    PS_OUTPUT output;
    output.Diffuse = c;
    output.Normal = float4(saturate(norm * 0.5 + 0.5), c.a);
    output.Specular = float4(spec, c.a);
    output.Irradiance = float4(1, 0, 0, c.a);

    return output;

}