#ifndef __TREE_WIND_HLSLI__
#define __TREE_WIND_HLSLI__

float2 TriangleWave(float2 x)
{
    return 1.0 - 2.0*abs(frac(x) - 0.5);
}

float2 SmoothedTriangleWave(float2 x)
{
    float2 t = TriangleWave(x); // Basic triangle wave (T(0)= 0, T(0.5) = 1, T(1) = 0)
    t = t*t*(3.0 - 2.0*t); // Smooth at 0 and 1 (Hermite interpolation)
    return 2.0*t - 1.0; // Transform into [-1, 1] space
}

float3 TriangleWave3(float3 x)
{
    return 1.0 - 2.0*abs(frac(x) - 0.5);
}

float3 SmoothedTriangleWave3(float3 x)
{
    float3 t = TriangleWave3(x);
    t = t*t*(3.0 - 2.0*t);
    return 2.0*t - 1.0;
}

float4 CalculateWindVector(
    float phase,
    float globalTimer,
    float4 windVector0,
    float4 windVector1,
    float4 windVector2,
    float4 windVector3,
    float freq1,
    float freq2,
    float globalPhaseShift,
    float windVariationEnabled)
{
    // Calculate time-based arguments with frequencies and phase
    float2 arg = float2(freq1, freq2)*float2(globalTimer, globalTimer) +
                 float2(phase, phase) +
                 float2(globalPhaseShift, globalPhaseShift);

    // Get smooth triangle waves for blending
    float2 blend = SmoothedTriangleWave(arg);
    float2 invBlend = float2(1.0, 1.0) - blend;

    // Calculate blending factors for 4 wind vectors (bilinear interpolation)
    float4 factors = float4(
        invBlend.x * invBlend.y,  // Wind 0
        blend.x * invBlend.y,     // Wind 1
        invBlend.x * blend.y,     // Wind 2
        blend.x * blend.y         // Wind 3
    );

    // Blend all 4 wind vectors
    float3 newWind = factors.x*windVector0.xyz +
                     factors.y*windVector1.xyz +
                     factors.z*windVector2.xyz +
                     factors.w*windVector3.xyz;

    // Lerp between base wind (vector 0) and varying wind based on enable flag
    float3 finalWind = (1.0 - windVariationEnabled)*windVector0.xyz +
                       windVariationEnabled*newWind;

    return float4(finalWind, 0.0);
}

float3 ComputeMicromovement(
    float3 modelspaceNormal,
    float3 vertexColor,       // RGB: R=horizontal scale, G=phase, B=vertical scale
    float globalTimer,
    float4 freqAndAmp0,       // XY: low wind freq/amp, ZW: high wind freq/amp
    float4 freqAndAmp1,       // Additional wave basis
    float umHighLowBlend,     // Blend between low and high wind speeds
    float globalPhaseShift,
    float amplitudeScale)     // Overall amplitude multiplier
{
    float umScaleH = vertexColor.r;  // Horizontal movement scale
    float umScaleV = vertexColor.b;  // Vertical movement scale
    float umArgPhase = abs(vertexColor.g + globalPhaseShift); // Phase shift

    // Use consistent normal (important for double-sided rendering)
    float3 normalToUse = sign(modelspaceNormal.z) * modelspaceNormal;

    // Calculate two wave components (low and high wind speed)
    float2 triWaveArg0 = float2(globalTimer, globalTimer)*float2(freqAndAmp0.x, freqAndAmp0.z) + umArgPhase;
    float2 waveSum0 = SmoothedTriangleWave(triWaveArg0)*float2(freqAndAmp0.y, freqAndAmp0.w);

    float2 triWaveArg1 = float2(globalTimer, globalTimer)*float2(freqAndAmp1.x, freqAndAmp1.z) + umArgPhase;
    float2 waveSum1 = SmoothedTriangleWave(triWaveArg1)*float2(freqAndAmp1.y, freqAndAmp1.w);

    float2 waveSum = waveSum0 + waveSum1;

    // Blend between low and high wind speeds
    float sumOfWaves = (1.0 - umHighLowBlend)*waveSum.x + umHighLowBlend*waveSum.y;
    sumOfWaves *= amplitudeScale;

    // Calculate final micromovement
    // Horizontal: move perpendicular to normal
    // Vertical: move along normal direction
    float3 micromovement = sumOfWaves*umScaleH*float3(normalToUse.xy, 0.0) +
                          sumOfWaves*umScaleV*float3(0.0, 0.0, normalToUse.z);

    return micromovement;
}

float ComputeTrunkStiffness(float stiffnessInput, float adjustLow, float adjustHigh)
{
    float stiffness = stiffnessInput*(adjustHigh - adjustLow) + adjustLow;
    return 1.0 - exp(-stiffness); // Exponential falloff for natural feel
}

float ComputePhaseStiffness(float stiffnessInput, float adjustLow, float adjustHigh)
{
    float stiffness = stiffnessInput*(adjustHigh - adjustLow) + adjustLow;
    return 1.0 - exp(-stiffness);
}

float3 ApplySoftClamp(
    float3 windByStiffness,
    float stiffness,
    float windSpeedUnrestricted,
    float windSpeedSoftClampZone)
{
    // Prevent zero length vector
    float l = length(abs(windByStiffness) + float3(0.001, 0.0, 0.0));

    // Calculate in "multiplied by stiffness space"
    float unrestricted = windSpeedUnrestricted * stiffness;
    float softClampZone = windSpeedSoftClampZone * stiffness;

    float k = max(0.0, l - unrestricted) / softClampZone;
    float softClamped = softClampZone*(1.0 - exp(-k)) + unrestricted;

    float scale = (l > unrestricted) ? (softClamped / l) : 1.0;
    return windByStiffness * scale;
}

float3 ComputeBranchBend(
    float3 modelspaceVertPos,
    float4x4 worldMtx,           // World matrix (for transforming wind from world to model space)
    float4 vertexColor,          // Stiffness data in alpha channel
    float globalTimer,
    float4 windVector0,
    float4 windVector1,
    float4 windVector2,
    float4 windVector3,
    float freq1,
    float freq2,
    float globalPhaseShift,
    float pivotHeight,           // Height of bend pivot point
    float trunkStiffnessLow,
    float trunkStiffnessHigh,
    float phaseStiffnessLow,
    float phaseStiffnessHigh,
    float windSpeedUnrestricted,
    float windSpeedSoftClampZone,
    float phaseVariationEnabled)
{
    // Calculate stiffness values
    float trunkStiffness = ComputeTrunkStiffness(vertexColor.a, trunkStiffnessLow, trunkStiffnessHigh);
    float phaseStiffness = ComputePhaseStiffness(vertexColor.a, phaseStiffnessLow, phaseStiffnessHigh);

    float oneMinusTrunkStiffness = 1.0 - trunkStiffness;
    float oneMinusPhaseStiffness = 1.0 - phaseStiffness;

    // Base trunk wind (from primary wind vector)
    float3 trunkWind = windVector0.xyz;

    // Phase variation wind (smoothly varying wind direction)
    float3 phaseVariationWind = CalculateWindVector(
        vertexColor.g,  // phase from vertex color
        globalTimer,
        windVector0,
        windVector1,
        windVector2,
        windVector3,
        freq1,
        freq2,
        globalPhaseShift,
        1.0).xyz;

    // Combine trunk and phase winds based on stiffness
    float3 totalWind = trunkWind*oneMinusTrunkStiffness +
                       (phaseVariationWind - trunkWind)*oneMinusPhaseStiffness*phaseVariationEnabled;

    // Transform wind into model space
    float3 wind = mul((float3x3)worldMtx, totalWind);

    // Bend around the pivot point
    float3 originalOffset = modelspaceVertPos - float3(0.0, 0.0, pivotHeight);
    float originalLen = length(originalOffset);

    // Apply soft clamping to wind
    wind = ApplySoftClamp(wind, oneMinusTrunkStiffness, windSpeedUnrestricted, windSpeedSoftClampZone);

    // Calculate bent position
    float3 bentOffset = originalOffset + wind;
    float bentLen = length(bentOffset);

    // Maintain original distance from pivot (preserves volume)
    float3 branchBendPos = bentOffset*(originalLen / (bentLen + 0.001)) + float3(0.0, 0.0, pivotHeight);

    return branchBendPos;
}

float3 ComputeBranchBendPlusMicromovement(
    float3 modelspaceVertPos,
    float3 modelspaceNormal,
    float4 vertexColor,
    float4x4 worldMtx,
    float globalTimer,
    // Branch bend params
    float4 windVector0,
    float4 windVector1,
    float4 windVector2,
    float4 windVector3,
    float freq1,
    float freq2,
    float globalPhaseShift,
    float pivotHeight,
    float trunkStiffnessLow,
    float trunkStiffnessHigh,
    float phaseStiffnessLow,
    float phaseStiffnessHigh,
    float windSpeedUnrestricted,
    float windSpeedSoftClampZone,
    float phaseVariationEnabled,
    // Micromovement params
    float4 freqAndAmp0,
    float4 freqAndAmp1,
    float umHighLowBlend,
    float umAmplitudeScale)
{
    // Compute branch bending
    float3 branchBendPos = ComputeBranchBend(
        modelspaceVertPos,
        worldMtx,
        vertexColor,
        globalTimer,
        windVector0,
        windVector1,
        windVector2,
        windVector3,
        freq1,
        freq2,
        globalPhaseShift,
        pivotHeight,
        trunkStiffnessLow,
        trunkStiffnessHigh,
        phaseStiffnessLow,
        phaseStiffnessHigh,
        windSpeedUnrestricted,
        windSpeedSoftClampZone,
        phaseVariationEnabled);

    // Compute micromovement
    float3 micromovement = ComputeMicromovement(
        modelspaceNormal,
        vertexColor.rgb,
        globalTimer,
        freqAndAmp0,
        freqAndAmp1,
        umHighLowBlend,
        globalPhaseShift,
        umAmplitudeScale);

    // Combine both effects
    return branchBendPos + micromovement;
}


float3 SimpleWindMotion(
    float3 position,
    float3 vertexColor,      // RGB used for wind parameters
    float4 windVector,       // Main wind direction and strength
    float4 windOverride,     // Per-object wind modulation
    float globalPhaseShift)
{
    // Increased intensity multiplier for more visible movement
    const float WIND_INTENSITY_MULTIPLIER = 2.5; // Increased from 1.0 for more dramatic motion

    // Extract parameters from vertex color with intensity boost
    float3 f1 = vertexColor.xxz * windVector.xxy * windOverride.xxy * WIND_INTENSITY_MULTIPLIER;

    // Phase calculation (uses vertex Y + global phase)
    float phase = vertexColor.y + globalPhaseShift;
    float phaseRad = abs(phase) * 6.283185; // 2*PI

    // Calculate phase-shifted wind
    float3 f2 = windVector.zzw * windOverride.zzw + phaseRad;
    f2 = sin(f2);

    // Apply wind displacement with enhanced amplitude
    f1 = f2 * f1 + position;

    return f1;
}

#endif // __TREE_WIND_HLSLI__
