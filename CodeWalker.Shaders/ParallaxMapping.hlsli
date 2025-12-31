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

    // Improved parallax offset with height-based displacement
    // GTA V uses inverted height, where darker = deeper
    float h = (heightValue * parallaxScale) - (parallaxScale * 0.5);
    h += parallaxBias;

    // Perspective-correct parallax with safer division
    // Use max to prevent extreme values at grazing angles
    float2 offset = h * (tanView.xy / max(abs(tanView.z), 0.2));

    // Clamp offset to prevent excessive distortion
    offset = clamp(offset, -0.1, 0.1);

    return texCoord - offset;
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

    // Clamp numSteps to reasonable range
    numSteps = clamp(numSteps, 4, 32);

    // Calculate step size and delta per iteration
    // Scale by tanView.z to get perspective-correct depth
    float stepSize = 1.0 / float(numSteps);
    float2 texDelta = (tanView.xy / max(abs(tanView.z), 0.1)) * parallaxScale * stepSize;

    // Start at the surface - use R channel for GTA V pxm format
    float currentLayerDepth = 0.0;
    float2 currentTexCoord = texCoord;
    float currentDepthMapValue = saturate(initialHeightSample.r); // Ensure valid range [0,1]

    // Ray march through the height field
    [unroll(32)]
    for (int i = 0; i < numSteps; i++)
    {
        // Check if current point is below the surface
        if (currentLayerDepth < currentDepthMapValue)
        {
            // Move along ray
            currentTexCoord -= texDelta;

            // Sample and validate height value
            float sampledHeight = heightTexture.Sample(heightSampler, currentTexCoord).r;
            currentDepthMapValue = saturate(sampledHeight); // Clamp to [0,1]

            currentLayerDepth += stepSize;
        }
    }

    // Parallax occlusion mapping with offset limiting (interpolation)
    float2 prevTexCoord = currentTexCoord + texDelta;

    // Get depth values before and after collision point for interpolation
    float afterDepth = currentDepthMapValue - currentLayerDepth;
    float beforeDepth = heightTexture.Sample(heightSampler, prevTexCoord).r - (currentLayerDepth - stepSize);

    // Interpolation weight with safety check to prevent division by zero
    float depthDiff = afterDepth - beforeDepth;
    float weight = 0.0;

    // Only interpolate if we have valid depth difference
    if (abs(depthDiff) > 0.0001)
    {
        weight = saturate(afterDepth / depthDiff);
    }

    // Interpolate texture coordinates
    outTexCoord = lerp(currentTexCoord, prevTexCoord, weight);

    // Clamp texture coordinates to prevent extreme offsets
    // Allow some wrapping but prevent going too far out of bounds
    float2 offsetFromOriginal = outTexCoord - texCoord;
    const float maxOffset = 0.15; // Maximum 15% texture coordinate shift
    offsetFromOriginal = clamp(offsetFromOriginal, -maxOffset, maxOffset);
    outTexCoord = texCoord + offsetFromOriginal;

    // Sample at final interpolated position
    float4 finalSample = heightTexture.Sample(heightSampler, outTexCoord);

    return finalSample;
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
    // First pass: steep parallax occlusion mapping
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

    // Second pass: binary search refinement for higher quality
    float3 tanView = normalize(viewDirTangent);
    float stepSize = 1.0 / float(numSteps);
    float2 texDelta = (tanView.xy / max(abs(tanView.z), 0.1)) * parallaxScale * stepSize;

    float2 currentTexCoord = steepTexCoord;
    float currentLayerDepth = steepSample.r;

    // Binary search refinement
    numRefinementSteps = clamp(numRefinementSteps, 0, 8);
    float2 halfDelta = texDelta * 0.5;

    [unroll(8)]
    for (int i = 0; i < numRefinementSteps; i++)
    {
        // Sample at midpoint
        float2 testTexCoord = currentTexCoord + halfDelta;
        float testHeight = heightTexture.Sample(heightSampler, testTexCoord).r;

        // Refine based on height comparison
        if (testHeight > currentLayerDepth)
        {
            currentTexCoord = testTexCoord;
        }

        halfDelta *= 0.5;
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
