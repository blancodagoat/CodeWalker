//
// TreeVS.hlsli - Tree vertex shader with GTA 5 wind system
//
// Uses smoothed triangle waves from trees_windfuncs.fxh and vertex color
// conventions from trees_common.fxh for accurate tree wind animation.
// Outputs same VS_OUTPUT as BasicVS so BasicPS can be reused.
//

#include "Quaternion.hlsli"
#include "Shadowmap.hlsli"

// Same layout as BasicShader for PS compatibility
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
    uint IsInstanced;
}
cbuffer VSModelVars : register(b3)
{
    float4x4 Transform;
}
cbuffer VSGeomVars : register(b4)
{
    uint EnableTint;
    float TintYVal;
    uint IsDecal;
    uint EnableWind;
    float4 WindOverrideParams;
    float4 globalAnimUV0;
    float4 globalAnimUV1;
}

// Tree-specific wind parameters from material
cbuffer VSTreeWindVars : register(b9)
{
    float4 umGlobalParams;   // material: X=scaleH, Y=scaleV, Z=stiffnessMultiplier, W=freqV
    float4 WindGlobalParams; // material: X=windScale, Y-Z=collision(unused), W=free
    float  GlobalTimer;      // accumulated wind timer
    float3 TreeWindPad;
}

// Same VS_OUTPUT as BasicVS for BasicPS compatibility
struct VS_OUTPUT
{
    float4 Position  : SV_POSITION;
    float3 Normal    : NORMAL;
    float2 Texcoord0 : TEXCOORD0;
    float2 Texcoord1 : TEXCOORD1;
    float2 Texcoord2 : TEXCOORD2;
    float4 Shadows   : TEXCOORD3;
    float4 LightShadow : TEXCOORD4;
    float4 Colour0   : COLOR0;
    float4 Colour1   : COLOR1;
    float4 Tint      : COLOR2;
    float4 Tangent   : TEXCOORD5;
    float4 Bitangent : TEXCOORD6;
    float3 CamRelPos : TEXCOORD7;
};

Texture2D<float4> TintPalette : register(t0);
SamplerState TextureSS : register(s0);


//------------------------------------------------------------------------------
// Wind functions from GTA 5 trees_windfuncs.fxh
//------------------------------------------------------------------------------

// Smoothed triangle wave: natural oscillation [-1, 1]
// Exact implementation from GTA 5 trees_windfuncs.fxh
float2 SmoothedTriangleWave(float2 x)
{
    float2 t = 1.0 - 2.0 * abs(frac(x) - 0.5);  // basic triangle wave
    t = t * t * (3.0 - 2.0 * t);                  // hermite smoothing
    return 2.0 * t - 1.0;                         // remap to [-1, 1]
}

// Compute world-space wind vector with phase variation.
// Matches GTA 5 CalculateWindVector() from trees_windfuncs.fxh.
// Uses smoothed triangle waves to blend wind directions for natural variation.
//
// In GTA 5, 4 pre-computed wind vectors are blended. Since CodeWalker has a single
// WindVector, we generate variation by rotating it at different phases.
float3 CalculateWindVectorForTree(float phase)
{
    // Two blend frequencies (GTA 5: branchBend_Freq1, branchBend_Freq2)
    float2 arg = float2(0.9, 1.1) * GlobalTimer + float2(phase, phase);
    float2 blend = SmoothedTriangleWave(arg);
    float2 invBlend = 1.0 - blend;

    // Generate 4 wind vector variants by rotating the base wind
    float3 wind0 = WindVector.xyz;
    float3 wind1 = float3(-wind0.y, wind0.x, wind0.z) * 0.8;  // rotated 90 deg, slightly weaker
    float3 wind2 = float3(-wind0.x, -wind0.y, wind0.z) * 0.6; // rotated 180 deg
    float3 wind3 = float3(wind0.y, -wind0.x, wind0.z) * 0.7;  // rotated 270 deg

    // Bilinear blend between 4 variants (exact GTA 5 approach)
    float4 factors = float4(invBlend.x * invBlend.y, blend.x * invBlend.y,
                            invBlend.x * blend.y,    blend.x * blend.y);
    float3 newWind = factors.x * wind0 + factors.y * wind1 +
                     factors.z * wind2 + factors.w * wind3;

    return newWind;
}

// Apply soft clamp to wind displacement (GTA 5 ApplySoftClamp)
// Prevents excessive bending while allowing smooth motion
float3 ApplySoftClamp(float3 windByStiffness, float stiffness)
{
    float l = length(abs(windByStiffness) + float3(0.001, 0.0, 0.0));
    float unrestricted = 1.0 * stiffness;
    float softClampZone = 0.5 * stiffness;
    float k = max(0.0, l - unrestricted) / max(softClampZone, 0.001);
    float softClamped = softClampZone * (1.0 - exp(-k)) + unrestricted;
    float scale = (l > unrestricted) ? softClamped / l : 1.0;
    return windByStiffness * scale;
}

// Compute tree wind bending - matches GTA 5 ComputeBranchBend_AlphaCardOnly
// from trees_windfuncs.fxh.
//
// Vertex colors (GTA 5 convention):
//   Color0.r = horizontal movement amplitude (0=none, 1=max)
//   Color0.g = phase shift (per-vertex, so branches don't move in sync)
//   Color0.b = vertical movement amplitude (0=none, 1=max)
//
// umGlobalParams (per-material):
//   .x = horizontal scale (default 0.025)
//   .y = vertical scale (default 0.020)
//   .z = stiffness multiplier (default 1.0)
//
// WindGlobalParams (per-material):
//   .x = overall wind scale (default 1.0)
//
// Returns the bent model-space position (NOT a displacement vector).
float3 ComputeTreeBend(float3 modelspacevertpos, float3 color0)
{
    // Material stiffness (GTA 5: oneMinusStiffness = 1 - saturate(globalStiffness * multiplier))
    float globalStiffness = 0.3;
    float oneMinusStiffness = 1.0 - saturate(globalStiffness * umGlobalParams.z);

    // WindGlobalParams.x = overall wind scale
    float windScale = max(WindGlobalParams.x, 0.0);
    windScale = (windScale == 0.0) ? 1.0 : windScale;

    // Calculate wind vector using vertex phase (GTA 5: CalculateWindVector)
    float3 alphaCardWind = CalculateWindVectorForTree(color0.g);

    // Tether to branch: k = max(r, b) (GTA 5 convention)
    float k = max(color0.r, color0.b);

    // Scale wind by vertex color, stiffness, and material params
    float3 blendedWind = k * alphaCardWind * oneMinusStiffness * windScale;

    // Transform wind from world space to model space using inverse orientation
    // (GTA 5: float3 wind = mul(worldMtx, float4(windWorldSpace, 0.0)))
    float4 invOri = float4(-Orientation.xyz, Orientation.w); // conjugate = inverse for unit quat
    float3 wind = mulvq(blendedWind, invOri);

    // Apply material scale
    wind.xy *= umGlobalParams.x * 5.0;
    wind.z  *= umGlobalParams.y * 5.0;

    // Soft clamp to prevent extreme bending (GTA 5: ApplySoftClamp)
    wind = ApplySoftClamp(wind, oneMinusStiffness);

    // Arc-length preserving bend around pivot at origin
    // (GTA 5: bentOffset*(originalLen/bentLen) maintains distance from pivot)
    float3 originalOffset = modelspacevertpos; // pivot is at (0,0,0) for alpha-card-only
    float originalLen = length(originalOffset);

    float3 bentOffset = originalOffset + wind;
    float bentLen = length(bentOffset);

    // Maintain original distance from pivot - this creates proper bending
    // instead of stretching/twisting
    float3 branchBendPos = bentOffset * (originalLen / max(bentLen, 0.001));

    return branchBendPos;
}

// Add 3-wave micromovements on top of branch bending (GTA 5: ComputeMicromovement)
// These are small oscillations that make leaves/foliage flutter
float3 ComputeMicromovement(float3 modelspacenormal, float3 color0)
{
    float umScaleH = color0.r;
    float umScaleV = color0.b;
    float phase = abs(color0.g);

    // 3 smoothed triangle waves (GTA 5 umTriWave defaults)
    float2 wave1 = SmoothedTriangleWave(float2(0.053, 0.043) * GlobalTimer + phase);
    float2 wave2 = SmoothedTriangleWave(float2(0.046, 0.028) * GlobalTimer + phase);
    float2 wave3 = SmoothedTriangleWave(float2(0.079, 0.010) * GlobalTimer + phase);

    float waveSum = wave1.x * 0.020 + wave2.x * 0.019 + wave3.x * 0.012;

    // Use normal to determine movement direction (GTA 5 convention)
    float3 normalToUse = sign(modelspacenormal.z + 0.001) * modelspacenormal;
    float3 micromovement = waveSum * umScaleH * float3(normalToUse.xy, 0.0)
                         + waveSum * umScaleV * float3(0.0, 0.0, normalToUse.z);

    return micromovement;
}


//------------------------------------------------------------------------------
// Transform functions
//------------------------------------------------------------------------------

float3 TreeModelTransform(float3 ipos, float3 vc0, float3 inorm)
{
    float3 tpos = (HasTransforms == 1) ? mul(float4(ipos, 1), Transform).xyz : ipos;
    float3 tnorm = (HasTransforms == 1) ? mul(inorm, (float3x3)Transform) : inorm;
    float3 spos = tpos * Scale;

    if (EnableWind)
    {
        // Branch bending with arc-length preservation (GTA 5 approach)
        spos = ComputeTreeBend(spos, vc0);

        // Add micromovements on top (small leaf flutter)
        spos += ComputeMicromovement(tnorm, vc0);
    }

    float3 bpos = mulvq(spos, Orientation);
    return CamRel.xyz + bpos;
}

// Overload without normal for simpler vertex types
float3 TreeModelTransform(float3 ipos, float3 vc0)
{
    return TreeModelTransform(ipos, vc0, float3(0, 0, 1));
}

float4 ScreenTransform(float3 opos)
{
    float4 pos = float4(opos, 1);
    float4 cpos = mul(pos, ViewProj);
    cpos.z = DepthFunc(cpos.zw);
    return cpos;
}

float3 NormalTransform(float3 inorm)
{
    float3 tnorm = (HasTransforms == 1) ? mul(inorm, (float3x3)Transform) : inorm;
    float3 bnorm = normalize(mulvq(tnorm, Orientation));
    return bnorm;
}

float4 ColourTint(float tx)
{
    float4 tnt = 1;
    if (EnableTint > 0)
    {
        tnt = TintPalette.SampleLevel(TextureSS, float2(tx, TintYVal), 0);
    }
    return tnt;
}
