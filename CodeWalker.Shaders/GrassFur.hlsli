#include "Quaternion.hlsli"
#include "Common.hlsli"

cbuffer VSSceneVars : register(b0)
{
    float4x4 ViewProj;
    float4 WindVector;
}
cbuffer VSEntityVars : register(b2)
{
    float4 CamRel;
    float4 Orientation;
    uint HasSkeleton;
    uint HasTransforms;
    uint TintPaletteIndex;
    uint Pad1;
    float3 Scale;
    uint Pad2;
}
cbuffer VSModelVars : register(b3)
{
    float4x4 Transform;
}

cbuffer GrassFurMeshVars : register(b4)
{
    uint FurMode;
    uint FurTintMode;
    uint FurMaskMode;
    uint FurLayerCount;
    float FurLayerCountInv;
    float FurLength;
    float FurBumpScale;
    float FurFadeDistMin;
    float FurFadeDistMax;
    float FurFadeShadow;
    float2 FurPad0;
    float4 FurUVScaling;
    float4 FurThresholds1;
    float4 FurThresholds2;
    float4 FurThresholds3;
    float4 FurThresholds4;
    float4 FurShadows1;
    float4 FurShadows2;
    float4 FurShadows3;
    float4 FurShadows4;
}

cbuffer PSSceneVars : register(b0)
{
    ShaderGlobalLightParams GlobalLights;
    uint EnableShadows;
    uint RenderMode;
    uint RenderModeIndex;
    uint RenderSamplerCoord;
}

Texture2D<float4> AlbedoMap : register(t0);
Texture2D<float4> BumpMap : register(t1);
Texture2D<float4> SpecMap : register(t2);
Texture2D<float4> NoiseMap : register(t3);
Texture2D<float4> TintMap : register(t4);
Texture2D<float4> MaskMap : register(t5);
Texture2D<float4> HeightMap1 : register(t6);
Texture2D<float4> HeightMap2 : register(t7);
Texture2D<float4> HeightMap3 : register(t8);
Texture2D<float4> HeightMap4 : register(t9);
SamplerState TextureSS : register(s0);
SamplerState HeightSS : register(s1);


struct VS_INPUT_PNCTTTX
{
    float4 Position : POSITION;
    float3 Normal : NORMAL;
    float4 Colour0 : COLOR0;
    float2 Texcoord0 : TEXCOORD0;
    float2 Texcoord1 : TEXCOORD1;
    float2 Texcoord2 : TEXCOORD2;
    float4 Tangent0 : TANGENT0;
};
struct VS_INPUT_PNCTTX
{
    float4 Position : POSITION;
    float3 Normal : NORMAL;
    float4 Colour0 : COLOR0;
    float2 Texcoord0 : TEXCOORD0;
    float2 Texcoord1 : TEXCOORD1;
    float4 Tangent0 : TANGENT0;
};
struct VS_INPUT_PNCTTT
{
    float4 Position : POSITION;
    float3 Normal : NORMAL;
    float4 Colour0 : COLOR0;
    float2 Texcoord0 : TEXCOORD0;
    float2 Texcoord1 : TEXCOORD1;
    float2 Texcoord2 : TEXCOORD2;
};

struct VS_OUTPUT
{
    float4 Position : SV_POSITION;
    float3 Normal : NORMAL;
    float4 Colour0 : COLOR0;
    float3 Texcoord0 : TEXCOORD0; // xy = uv, z = instanceID
    float2 Texcoord1 : TEXCOORD1;
    float2 Texcoord2 : TEXCOORD2;
    float4 Tangent0 : TEXCOORD3;
    float3 CamRelPos : TEXCOORD4;
};

struct PS_OUTPUT
{
    float4 Colour : SV_Target0;
};
struct PS_OUTPUT_DEFERRED
{
    float4 Diffuse : SV_Target0;
    float4 Normal : SV_Target1;
    float4 Specular : SV_Target2;
    float4 Irradiance : SV_Target3;
};


float3 FurWorldTransform(float3 pos, float3 norm, uint iid)
{
    float3 wpos;
    if (HasTransforms > 0)
    {
        wpos = mul(float4(pos, 1), (float4x3)Transform);
        norm = mul(norm, (float3x3)Transform);
    }
    else
    {
        wpos = mulvq(pos * Scale, Orientation);
    }

    // offset along normal for fur layer
    float3 wnorm = (HasTransforms > 0) ? norm : mulvq(norm, Orientation);
    wpos += normalize(wnorm) * (FurLength * iid);

    return wpos + CamRel.xyz;
}

float3 FurNormalTransform(float3 norm)
{
    if (HasTransforms > 0)
    {
        return normalize(mul(norm, (float3x3)Transform));
    }
    return normalize(mulvq(norm, Orientation));
}

float4 FurTangentTransform(float4 tang)
{
    float3 t;
    if (HasTransforms > 0)
    {
        t = normalize(mul(tang.xyz, (float3x3)Transform));
    }
    else
    {
        t = normalize(mulvq(tang.xyz, Orientation));
    }
    return float4(t, tang.w);
}


float4 GetFurAlbedo(float3 tc, float2 tc1, float2 tc2, float4 vc)
{
    uint iid = (uint)round(tc.z);
    float2 ctc = tc.xy * FurUVScaling.x;
    float2 htc = tc.xy * FurUVScaling.y;
    float a = 0;
    float m = 0.01;
    float s = 0;

    if      (iid<=0) { a = HeightMap1.Sample(HeightSS, htc).y; m = FurThresholds1.x; s = FurShadows1.x; }
    else if (iid==1) { a = HeightMap1.Sample(HeightSS, htc).w; m = FurThresholds1.y; s = FurShadows1.y; }
    else if (iid==2) { a = HeightMap2.Sample(HeightSS, htc).y; m = FurThresholds1.z; s = FurShadows1.z; }
    else if (iid==3) { a = HeightMap2.Sample(HeightSS, htc).w; m = FurThresholds1.w; s = FurShadows1.w; }
    else if (iid==4) { a = HeightMap3.Sample(HeightSS, htc).y; m = FurThresholds2.x; s = FurShadows2.x; }
    else if (iid==5) { a = HeightMap3.Sample(HeightSS, htc).w; m = FurThresholds2.y; s = FurShadows2.y; }
    else if (iid==6) { a = HeightMap4.Sample(HeightSS, htc).y; m = FurThresholds2.z; s = FurShadows2.z; }
    else if (iid>=7) { a = HeightMap4.Sample(HeightSS, htc).w; m = FurThresholds2.w; s = FurShadows2.w; }

    float mask = vc.a;
    if (FurMaskMode == 1)
    {
        float4 mv = MaskMap.Sample(TextureSS, tc1);
        mask *= mv.x;
    }
    a *= mask;

    float threshold = lerp(0.01, m, saturate(mask * 2));

    if (a <= 0) discard;
    a = smoothstep(0, 1, a / max(threshold * 2, 0.001));

    float3 c = AlbedoMap.Sample(TextureSS, ctc).rgb;

    if (FurTintMode == 1)
    {
        float4 t = TintMap.Sample(TextureSS, tc2 * FurUVScaling.w);
        c.rgb *= t.rgb;
    }

    float lightamt = saturate(lerp(FurFadeShadow, s, mask));

    return float4(c * lightamt, a);
}

float3 GetFurBitangent(float3 norm, float4 tang)
{
    return cross(tang.xyz, norm) * ((tang.w == 0) ? 1 : tang.w);
}

float4 GetFurNormal(float3 tc, float3 vnorm, float3 vtang, float3 vbitang)
{
    vnorm = normalize(vnorm);
    vtang = normalize(vtang);
    vbitang = normalize(vbitang);

    float2 ntc = tc.xy * FurUVScaling.z;
    float4 nmv = BumpMap.Sample(TextureSS, ntc);

    float2 nxy = nmv.xy * 2 - 1;
    float2 bxy = nxy * max(FurBumpScale, 0.001);
    float bxyz = sqrt(abs(1 - dot(nxy, nxy)));
    float3 t1 = vtang * bxy.x;
    float3 t2 = vbitang * bxy.y + t1;
    float3 t3 = vnorm * bxyz + t2;
    float3 n = normalize(t3);
    return float4(n, nmv.w);
}
