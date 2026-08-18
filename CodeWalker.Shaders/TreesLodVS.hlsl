#include "Common.hlsli"
#include "Quaternion.hlsli"


cbuffer VSSceneVars : register(b0)
{
    float4x4 ViewProj;
}
cbuffer VSEntityVars : register(b1)
{
    float4 CamRel;
    float4 Orientation;
    uint HasSkeleton;
    uint HasTransforms;
    uint Pad0;
    uint Pad1;
    float3 Scale;
    uint Pad2;
}
cbuffer VSModelVars : register(b2)
{
    float4x4 Transform;
}
cbuffer TreesLodShaderVSGeometryVars : register(b3)
{
    float4 AlphaTest;
    float4 AlphaScale;
    float4 UseTreeNormals;
    float4 treeLod2Normal;
    float4 treeLod2Params;
}
cbuffer VSWindVars : register(b4)
{
    // Wind parameters matching GTA 5 tree wind system
    float4 WindVector;       // XY = wind amplitude, ZW = wind phase (time-varying from CPU)
    float4 umGlobalParams;   // material: X=scaleH, Y=scaleV, Z=stiffnessMultiplier, W=freqV
    float4 WindGlobalParams; // material: X=windScale, Y=collR(unused), Z=collR(unused), W=free
    float  GlobalTimer;      // accumulated wind timer
    float3 WindPad;
}


struct VS_INPUT
{
    float4 Position : POSITION;
    float3 Normal   : NORMAL;
    float2 Texcoord0 : TEXCOORD0;
    float2 Texcoord1 : TEXCOORD1;
    float2 Texcoord2 : TEXCOORD2;
    float2 Texcoord3 : TEXCOORD3;
    float4 Colour0   : COLOR0;
    float4 Colour1   : COLOR1;
};

struct VS_OUTPUT
{
    float4 Position : SV_POSITION;
    float3 Normal   : NORMAL;
    float2 Texcoord : TEXCOORD0;
    float4 Colour   : COLOR0;
    float2 Texcoord1 : TEXCOORD1;
    float2 Texcoord2 : TEXCOORD2;
    float4 Colour1  : COLOR1;
};


//------------------------------------------------------------------------------
// Wind functions from GTA 5 trees_windfuncs.fxh
//------------------------------------------------------------------------------

// Smoothed triangle wave: produces natural-looking oscillation
// T(0)=0, T(0.5)=1, T(1)=0, smoothed at extremes, mapped to [-1, 1]
float2 SmoothedTriangleWave(float2 x)
{
    float2 t = 1.0 - 2.0 * abs(frac(x) - 0.5);  // basic triangle wave
    t = t * t * (3.0 - 2.0 * t);                  // hermite smoothing
    return 2.0 * t - 1.0;                         // remap to [-1, 1]
}

// Compute wind displacement matching GTA 5 tree wind system.
// Combines alpha-card-only bending (ComputeBranchBend_AlphaCardOnly) with
// smoothed triangle wave micromovements from trees_windfuncs.fxh.
//
// Vertex colors control per-vertex wind response (GTA 5 convention):
//   Color0.r = horizontal movement amplitude (0=none, 1=max)
//   Color0.g = phase shift (variation between vertices so branches don't sync)
//   Color0.b = vertical movement amplitude (0=none, 1=max)
//   Color0.a = self-shadowing / depth in tree
//
// Material parameters (umGlobalParams) - per-material wind control:
//   .x = horizontal scale multiplier (default 0.025)
//   .y = vertical scale multiplier (default 0.020)
//   .z = stiffness multiplier - multiplied with global stiffness (default 1.0)
//       In GTA 5 this multiplies branchBendEtc_GlobalAlphaCardStiffness.
//       Lower values = more flexible tree, higher = stiffer.
//   .w = reserved
//
// Material parameters (WindGlobalParams) - per-material wind scale:
//   .x = overall wind scale (GTA 5: free slot, default 1.0)
//   .y = player collision radius (unused here)
//   .z = vehicle collision radius (unused here)
//   .w = free (default 1.0)
float3 ComputeTreeWind(float3 color0, float heightFactor)
{
    // Phase from vertex color green channel (GTA 5 convention)
    // Each vertex gets a unique phase so branches don't all move in sync
    float phase = abs(color0.g);

    // Generate wind variation using 3 smoothed triangle waves (GTA 5 uses 3 basis waves)
    // Frequencies match GTA 5 umTriWave1/2/3Params defaults
    float2 wave1 = SmoothedTriangleWave(float2(0.053, 0.043) * GlobalTimer + phase);
    float2 wave2 = SmoothedTriangleWave(float2(0.046, 0.028) * GlobalTimer + phase);
    float2 wave3 = SmoothedTriangleWave(float2(0.079, 0.010) * GlobalTimer + phase);

    // Sum the 3 basis waves (GTA 5 approach: low wind + high wind components)
    float lowWind  = wave1.x * 0.35 + wave2.x * 0.30 + wave3.x * 0.20;
    float highWind = wave1.y * 0.60 + wave2.y * 0.45 + wave3.y * 0.30;

    // Blend between low and high wind based on wind vector strength
    float windMagnitude = length(WindVector.xy);
    float windBlend = saturate(windMagnitude * 10.0);
    float waveSum = lerp(lowWind, highWind, windBlend);

    // Wind amplitude from vertex colors (GTA 5 convention)
    // k = max(r, b) tethers foliage to branch (from ComputeBranchBend_AlphaCardOnly)
    float k = max(color0.r, color0.b);

    // Material stiffness control (GTA 5: oneMinusStiffness = 1 - saturate(globalStiffness * multiplier))
    // We use a base global stiffness of 0.3 (typical GTA 5 value),
    // multiplied by umGlobalParams.z (per-material stiffness multiplier)
    float globalStiffness = 0.3;
    float oneMinusStiffness = 1.0 - saturate(globalStiffness * umGlobalParams.z);

    // WindGlobalParams.x acts as overall wind scale multiplier
    // (GTA 5 default is 1.0; artists can increase/decrease per-material)
    float windScale = max(WindGlobalParams.x, 0.0);
    // If WindGlobalParams is zero (not set in material), use default scale of 1.0
    windScale = (windScale == 0.0) ? 1.0 : windScale;

    // Wind direction from renderer's wind vector
    float2 windDir = windMagnitude > 0.001 ? normalize(WindVector.xy) : float2(1, 0);

    // Compute displacement - scale factors from umGlobalParams.x/y control overall amplitude
    // Multiplied by 40x to convert from GTA 5's micromovement scale to visible world-space displacement
    // (GTA 5's actual displacement comes from wind vectors with large magnitudes;
    //  we compensate since we only have the small oscillating WindVector)
    float hScale = umGlobalParams.x * 5.0 * windScale;
    float vScale = umGlobalParams.y * 5.0 * windScale;

    float3 displacement;
    displacement.x = waveSum * k * hScale * windDir.x;
    displacement.y = waveSum * k * hScale * windDir.y;
    displacement.z = waveSum * k * vScale * 0.3; // vertical is subtler

    // Scale by stiffness (material control) and height (top of tree moves more)
    displacement *= oneMinusStiffness * heightFactor;

    // Add base wind push (constant lean in wind direction, like GTA 5's trunk wind)
    float basePush = windMagnitude * k * oneMinusStiffness * heightFactor * 0.5 * windScale;
    displacement.x += basePush * windDir.x;
    displacement.y += basePush * windDir.y;

    return displacement;
}


VS_OUTPUT main(VS_INPUT input)
{
    VS_OUTPUT output;

    //first find the base point of the billboard
    float3 ipos = input.Position.xyz;
    float3 tpos = (HasTransforms == 1) ? mul(float4(ipos, 1), Transform).xyz : ipos;
    float3 spos = tpos;
    float3 bpos = mulvq(spos, Orientation);
    float3 opos = CamRel.xyz + bpos;

    float3 dir = normalize(opos);
    float3 bbside = normalize(cross(dir, treeLod2Normal.xyz));
    float2 bbvpos = treeLod2Params.xy*(0.5 - input.Texcoord0)*input.Texcoord2;
    opos += bbside*bbvpos.x;
    opos += treeLod2Normal.xyz*bbvpos.y;

    // Wind displacement using GTA 5 vertex color convention
    // Height factor: vertices at top of billboard move more than bottom
    // Texcoord0.y goes from 0 (top) to 1 (bottom) for billboard UVs
    float heightFactor = saturate(1.0 - input.Texcoord0.y);
    float3 windOffset = ComputeTreeWind(input.Colour0.rgb, heightFactor);
    opos += windOffset;

    float4 pos = float4(opos, 1);
    float4 cpos = mul(pos, ViewProj);
    cpos.z = DepthFunc(cpos.zw);

    float3 bnorm = normalize(-pos.xyz); //normal pointing towards the camera

    output.Position = cpos;
    output.Normal = bnorm;
    output.Texcoord = input.Texcoord1;
    output.Colour = input.Colour0;// float4(input.Texcoord2, 0, 1);// input.Colour1;
    output.Texcoord1 = input.Texcoord0;
    output.Texcoord2 = input.Texcoord2;
    output.Colour1 = input.Colour1;
    return output;
}