#ifndef __PARALLAX_MAPPING_HLSLI__
#define __PARALLAX_MAPPING_HLSLI__

// Parallax mapping modes
#define PARALLAX_MODE_STANDARD  0  // Simple offset parallax
#define PARALLAX_MODE_STEEP     1  // Steep parallax occlusion mapping (higher quality)

float2 CalculateParallaxStandard(
    float3 viewDirTangent,
    float heightValue,
    float parallaxScale,
    float parallaxBias,
    float2 texCoord)
{
    // Normalize view direction in tangent space
    float3 tanView = normalize(viewDirTangent);

    // Simple parallax offset
    // height is typically 0-1, we scale and bias it
    float h = heightValue * parallaxScale + parallaxBias;

    // Divide by Z for proper perspective-correct parallax
    // This is the correct parallax mapping formula
    float2 offset = h * (tanView.xy / (tanView.z + 0.01)); // Add small value to prevent division by zero

    return texCoord + offset;
}

float4 CalculateParallaxSteep(
    float3 viewDirTangent,
    float4 initialHeightSample,
    float2 texCoord,
    Texture2D heightTexture,
    SamplerState heightSampler,
    float parallaxScale,
    int numSteps,
    out float2 outTexCoord)
{
    // Normalize view direction in tangent space
    float3 tanView = normalize(viewDirTangent);

    // Calculate step size and delta per iteration
    float stepSize = 1.0 / float(numSteps);
    float2 texDelta = (-tanView.xy) * parallaxScale / float(numSteps);

    // Start at the surface
    float currentHeight = 1.0;
    float2 currentTexCoord = texCoord;
    float4 currentSample = initialHeightSample;

    // Step through the height field
    for (int i = 0; i < numSteps; i++)
    {
        // Check if we're below the surface
        if (currentSample.a < currentHeight)
        {
            // Step down
            currentHeight -= stepSize;
            currentTexCoord += texDelta;

            // Sample new height
            currentSample = heightTexture.Sample(heightSampler, currentTexCoord);
        }
    }

    outTexCoord = currentTexCoord;
    return currentSample;
}

float4 CalculateParallaxRelief(
    float3 viewDirTangent,
    float4 initialHeightSample,
    float2 texCoord,
    Texture2D heightTexture,
    SamplerState heightSampler,
    float parallaxScale,
    int numSteps,
    int numRefinementSteps,
    out float2 outTexCoord)
{
    // First pass: steep parallax
    float2 steepTexCoord;
    float4 steepSample = CalculateParallaxSteep(
        viewDirTangent,
        initialHeightSample,
        texCoord,
        heightTexture,
        heightSampler,
        parallaxScale,
        numSteps,
        steepTexCoord);

    // Second pass: binary search refinement
    float3 tanView = normalize(viewDirTangent);
    float2 texDelta = (-tanView.xy) * parallaxScale / float(numSteps);

    float currentHeight = (numSteps - 1) * (1.0 / float(numSteps));
    float2 currentTexCoord = steepTexCoord - texDelta;

    // Binary search for better precision
    float searchStep = 1.0 / float(numSteps);
    for (int i = 0; i < numRefinementSteps; i++)
    {
        searchStep *= 0.5;
        float4 refineSample = heightTexture.Sample(heightSampler, currentTexCoord);

        if (refineSample.a < currentHeight)
        {
            currentHeight -= searchStep;
            currentTexCoord += texDelta * searchStep;
        }
        else
        {
            currentHeight += searchStep;
            currentTexCoord -= texDelta * searchStep;
        }
    }

    outTexCoord = currentTexCoord;
    return heightTexture.Sample(heightSampler, currentTexCoord);
}

float4 CalculateParallax(
    float3 viewDirTangent,
    float4 bumpMapSample,
    float2 diffuseTexCoord,
    float2 bumpTexCoord,
    Texture2D heightTexture,
    SamplerState heightSampler,
    float parallaxScale,
    float parallaxBias,
    int parallaxMode,      // 0=standard, 1=steep
    int numSteps,          // For steep parallax (typically 8-16)
    bool clampTexCoords,   // Clamp to 0-1 to prevent wrapping artifacts
    bool bumpShift,        // Also offset the normal map coordinates
    out float2 outDiffuseTexCoord)
{
    float4 outBumpMapSample = bumpMapSample;

    if (parallaxMode == PARALLAX_MODE_STEEP)
    {
        // Steep parallax occlusion mapping
        outBumpMapSample = CalculateParallaxSteep(
            viewDirTangent,
            bumpMapSample,
            diffuseTexCoord,
            heightTexture,
            heightSampler,
            parallaxScale,
            numSteps,
            outDiffuseTexCoord);
    }
    else
    {
        // Standard parallax mapping
        float2 height = float2(bumpMapSample.a, bumpMapSample.a);
        height = height * parallaxScale + parallaxBias;

        float3 tanView = normalize(viewDirTangent);
        height *= tanView.xy;

        outDiffuseTexCoord = diffuseTexCoord + height;

        // Optional: also offset bump map coordinates
        if (bumpShift)
        {
            float2 newBumpCoord = bumpTexCoord + height;
            if (clampTexCoords)
            {
                newBumpCoord = saturate(newBumpCoord);
            }
            outBumpMapSample = heightTexture.Sample(heightSampler, newBumpCoord);
        }
    }

    // Optional: clamp texture coordinates
    if (clampTexCoords)
    {
        outDiffuseTexCoord = saturate(outDiffuseTexCoord);
    }

    return outBumpMapSample;
}

float2 CalculateTerrainParallax(
    float3 viewDirTangent,
    float heightValue,
    float heightScale,
    float heightBias,
    float2 texCoord,
    float viewDistance,
    float fadeStart,
    float fadeEnd)
{
    // Fade out parallax at distance to avoid artifacts
    float fadeFactor = 1.0 - saturate((viewDistance - fadeStart) / (fadeEnd - fadeStart));

    if (fadeFactor <= 0.0)
    {
        return texCoord;
    }

    // Calculate parallax offset
    float3 tanView = normalize(viewDirTangent);

    // Apply per-layer height scale and bias
    float adjustedHeight = heightValue * heightScale + heightBias;
    adjustedHeight *= fadeFactor; // Fade with distance

    float2 offset = adjustedHeight * tanView.xy;

    return texCoord + offset;
}

float3 ApplyWaterHeight(
    float3 worldPos,
    float2 worldBase,
    float gridSize,
    int gridElements,
    Texture2D<float> heightTexture,
    float fadeDistance,
    out float3 outNormal)
{
    float3 displacedPos = worldPos;
    outNormal = float3(0, 0, 1); // Default up normal

    // Calculate texture coordinates from world position
    float2 heightClamp = gridElements * gridSize / 2.0 - abs(worldBase + gridElements * gridSize / 2.0 - worldPos.xy);
    heightClamp = saturate(heightClamp / fadeDistance);
    float heightScale = min(heightClamp.x, heightClamp.y);

    if (heightScale > 0.0)
    {
        float2 heightTexCoord = (worldPos.xy - worldBase) / (gridElements * gridSize);
        int2 heightTexInt = (int2)(heightTexCoord * gridElements);

        // Sample height
        float height = heightTexture.Load(int3(heightTexInt, 0)).r * heightScale;
        displacedPos.z += height;

        // Calculate normal from neighboring heights
        float4 heights;
        heights.x = heightTexture.Load(int3(heightTexInt + int2(-1, 0), 0)).r;
        heights.y = heightTexture.Load(int3(heightTexInt + int2( 1, 0), 0)).r;
        heights.z = heightTexture.Load(int3(heightTexInt + int2( 0,-1), 0)).r;
        heights.w = heightTexture.Load(int3(heightTexInt + int2( 0, 1), 0)).r;

        heights *= heightScale;

        // Calculate slope from height differences
        float2 slope = float2(heights.x - heights.y, heights.z - heights.w);
        outNormal = normalize(float3(slope, 1.0));
    }

    return displacedPos;
}

#endif // __PARALLAX_MAPPING_HLSLI__
