#ifndef __TERRAIN_COMMON_HLSLI__
#define __TERRAIN_COMMON_HLSLI__

#include "Lighting.hlsli"
#include "ParallaxMapping.hlsli"

struct comboTexel
{
    float3 color;
    float3 normal;
    float wetnessMult;
};

float GetAlpha(float3 rgb, float3 mrgb, float3 mask)
{
    float alphaR = mask.r * rgb.r + (1 - mask.r) * mrgb.r;
    float alphaG = mask.g * rgb.g + (1 - mask.g) * mrgb.g;
    float alphaB = mask.b * rgb.b + (1 - mask.b) * mrgb.b;
    return alphaR * alphaG * alphaB;
}

float3 BlendTerrainColor_4Layer(
    float3 srcRGB,
    Texture2D layer0, Texture2D layer1, Texture2D layer2, Texture2D layer3,
    SamplerState samplerState,
    float2 texCoord0, float2 texCoord1)
{
    srcRGB = saturate(srcRGB);
    float3 msrcRGB = 1 - srcRGB;

    // Calculate blend alphas for all 8 possible layer combinations
    float4 alpha0716;
    float4 alpha2534;

    // Layers 0, 7, 1, 6
    alpha0716.x = GetAlpha(srcRGB, msrcRGB, float3(0, 0, 0)); // Layer 0: 000
    alpha0716.y = GetAlpha(srcRGB, msrcRGB, float3(1, 1, 1)); // Layer 7: 111
    alpha0716.z = GetAlpha(srcRGB, msrcRGB, float3(0, 0, 1)); // Layer 1: 001
    alpha0716.w = GetAlpha(srcRGB, msrcRGB, float3(1, 1, 0)); // Layer 6: 110

    // Layers 2, 5, 3, 4
    alpha2534.x = GetAlpha(srcRGB, msrcRGB, float3(0, 1, 0)); // Layer 2: 010
    alpha2534.y = GetAlpha(srcRGB, msrcRGB, float3(1, 0, 1)); // Layer 5: 101
    alpha2534.z = GetAlpha(srcRGB, msrcRGB, float3(0, 1, 1)); // Layer 3: 011
    alpha2534.w = GetAlpha(srcRGB, msrcRGB, float3(1, 0, 0)); // Layer 4: 100

    // Sample and blend textures
    float3 result = layer0.Sample(samplerState, texCoord0).rgb * alpha0716.x;
    result += layer1.Sample(samplerState, texCoord1).rgb * alpha0716.z;
    result += layer2.Sample(samplerState, texCoord1).rgb * alpha2534.x;
    result += layer3.Sample(samplerState, texCoord1).rgb * alpha2534.z;

    return result;
}

float3 BlendTerrainNormal_4Layer(
    float3 srcRGB,
    Texture2D normal0, Texture2D normal1, Texture2D normal2, Texture2D normal3,
    SamplerState samplerState,
    float2 texCoord0, float2 texCoord1)
{
    srcRGB = saturate(srcRGB);
    float3 msrcRGB = 1 - srcRGB;

    float4 alpha0716;
    float4 alpha2534;

    alpha0716.x = GetAlpha(srcRGB, msrcRGB, float3(0, 0, 0));
    alpha0716.y = GetAlpha(srcRGB, msrcRGB, float3(1, 1, 1));
    alpha0716.z = GetAlpha(srcRGB, msrcRGB, float3(0, 0, 1));
    alpha0716.w = GetAlpha(srcRGB, msrcRGB, float3(1, 1, 0));

    alpha2534.x = GetAlpha(srcRGB, msrcRGB, float3(0, 1, 0));
    alpha2534.y = GetAlpha(srcRGB, msrcRGB, float3(1, 0, 1));
    alpha2534.z = GetAlpha(srcRGB, msrcRGB, float3(0, 1, 1));
    alpha2534.w = GetAlpha(srcRGB, msrcRGB, float3(1, 0, 0));

    // Sample normal maps and convert from [0,1] to [-1,1]
    float3 n0 = normal0.Sample(samplerState, texCoord0).xyz * 2.0 - 1.0;
    float3 n1 = normal1.Sample(samplerState, texCoord1).xyz * 2.0 - 1.0;
    float3 n2 = normal2.Sample(samplerState, texCoord1).xyz * 2.0 - 1.0;
    float3 n3 = normal3.Sample(samplerState, texCoord1).xyz * 2.0 - 1.0;

    // Blend normals
    float3 result = n0 * alpha0716.x;
    result += n1 * alpha0716.z;
    result += n2 * alpha2534.x;
    result += n3 * alpha2534.z;

    return normalize(result);
}

float3 BlendTerrainColorWithHeight(
    float3 srcRGB,
    Texture2D layer0, Texture2D layer1, Texture2D layer2, Texture2D layer3,
    Texture2D height0, Texture2D height1, Texture2D height2, Texture2D height3,
    SamplerState samplerState,
    float2 texCoord0, float2 texCoord1,
    float3 viewDirTangent,
    float parallaxScale,
    float viewDistance,
    float fadeStart,
    float fadeEnd)
{
    srcRGB = saturate(srcRGB);
    float3 msrcRGB = 1 - srcRGB;

    // Calculate blend alphas
    float4 alpha0716;
    float4 alpha2534;

    alpha0716.x = GetAlpha(srcRGB, msrcRGB, float3(0, 0, 0));
    alpha0716.z = GetAlpha(srcRGB, msrcRGB, float3(0, 0, 1));
    alpha2534.x = GetAlpha(srcRGB, msrcRGB, float3(0, 1, 0));
    alpha2534.z = GetAlpha(srcRGB, msrcRGB, float3(0, 1, 1));

    // Sample height maps and calculate parallax-adjusted texture coordinates
    float h0 = height0.Sample(samplerState, texCoord0).a;
    float h1 = height1.Sample(samplerState, texCoord1).a;
    float h2 = height2.Sample(samplerState, texCoord1).a;
    float h3 = height3.Sample(samplerState, texCoord1).a;

    // Apply parallax to each layer's texture coordinates
    float2 tc0 = CalculateTerrainParallax(viewDirTangent, h0, parallaxScale, 0.0, texCoord0, viewDistance, fadeStart, fadeEnd);
    float2 tc1 = CalculateTerrainParallax(viewDirTangent, h1, parallaxScale, 0.0, texCoord1, viewDistance, fadeStart, fadeEnd);
    float2 tc2 = CalculateTerrainParallax(viewDirTangent, h2, parallaxScale, 0.0, texCoord1, viewDistance, fadeStart, fadeEnd);
    float2 tc3 = CalculateTerrainParallax(viewDirTangent, h3, parallaxScale, 0.0, texCoord1, viewDistance, fadeStart, fadeEnd);

    // Sample textures with parallax-adjusted coordinates
    float3 result = layer0.Sample(samplerState, tc0).rgb * alpha0716.x;
    result += layer1.Sample(samplerState, tc1).rgb * alpha0716.z;
    result += layer2.Sample(samplerState, tc2).rgb * alpha2534.x;
    result += layer3.Sample(samplerState, tc3).rgb * alpha2534.z;

    return result;
}

float CalculateTerrainSpecular(
    float baseSpecular,
    float specularMapValue,
    float wetness,
    float normalZ,
    float wetnessMultiplier)
{
    // Base specular from texture
    float specular = baseSpecular * specularMapValue * specularMapValue;

    // Increase specular on flat surfaces when wet (puddle effect)
    float flatnessFactor = saturate((normalZ - 0.35) * 1.538462);
    float wetnessBoost = flatnessFactor * wetness * wetnessMultiplier;
    specular += wetnessBoost;

    return specular;
}

float3 ProcessTerrainLighting(
    float3 worldPos,
    float3 worldNormal,
    float3 vertColor,
    float3 surfaceToEyeDir,
    float specularIntensity,
    float specularExponent,
    uniform ShaderGlobalLightParams globalLights)
{
    SurfaceProperties surfaceInfo = (SurfaceProperties)0;

    // Setup surface properties
    surfaceInfo.surface_worldNormal = worldNormal;
    surfaceInfo.surface_baseColor = float4(1.0, 1.0, 1.0, 1.0);
    surfaceInfo.surface_diffuseColor = float4(vertColor, 1.0);
    surfaceInfo.surface_specularIntensity = specularIntensity;
    surfaceInfo.surface_specularExponent = specularExponent;
    surfaceInfo.surface_fresnel = 0.98;
    surfaceInfo.surface_reflectionColor = float3(0.0, 0.0, 0.0);
    surfaceInfo.surface_emissiveIntensity = 0.0;

    // Self-shadowing based on normal vs light direction
    surfaceInfo.surface_selfShadow = dot(worldNormal.xyz, -globalLights.LightDir.xyz) < 0 ? 1.0 : 0.0;

    StandardLightingProperties standardProps = DeriveLightingPropertiesForCommonSurface(surfaceInfo);
    standardProps.diffuseColor = ProcessDiffuseColor(standardProps.diffuseColor);

    surfaceProperties surface;
    directionalLightProperties light;
    materialProperties material;

    populateForwardLightingStructs(
        surface,
        material,
        light,
        worldPos.xyz,
        surfaceInfo.surface_worldNormal.xyz,
        standardProps);

    // Set light properties from global lights
    light.direction = normalize(globalLights.LightDir.xyz);
    light.color = globalLights.LightDirColour.rgb;
    light.ambientColor = globalLights.LightDirAmbColour.rgb;

    // Set view direction
    surface.viewDirection = normalize(surfaceToEyeDir);

    return calculateForwardLighting(
        8, // numLights
        true,
        surface,
        material,
        light,
        true, // directional
        false, // directionalShadow (applied externally)
        false, // directionalShadowHighQuality
        float2(0.0, 0.0)).rgb;
}

float3 BlendLayerColorTint(
    Texture2D layerTexture,
    Texture2D tintTexture,
    SamplerState samplerState,
    float2 layerTexCoord,
    float2 tintTexCoord,
    float blendValue,
    bool desaturateTint)
{
    float3 layerColor = layerTexture.Sample(samplerState, layerTexCoord).rgb;
    float3 tintColor = tintTexture.Sample(samplerState, tintTexCoord).rgb;

    float3 lerpDestination;

    if (desaturateTint)
    {
        float3 tintColorGrayscale = (tintColor.r + tintColor.g + tintColor.b) / 3.0;
        lerpDestination = layerColor * lerp(tintColorGrayscale, tintColor, blendValue);
    }
    else
    {
        lerpDestination = (layerColor.r + layerColor.g + layerColor.b) / 3.0;
        lerpDestination *= tintColor;
    }

    return lerp(layerColor, lerpDestination, blendValue);
}

#endif // __TERRAIN_COMMON_HLSLI__
