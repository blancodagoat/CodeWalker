#ifndef __LIGHTING_HLSLI__
#define __LIGHTING_HLSLI__

#include "Common.hlsli"

struct SurfaceProperties
{
    float3 surface_worldNormal;
    float4 surface_baseColor;
    float4 surface_diffuseColor;

    // Specular properties
    float surface_specularIntensity;
    float surface_specularExponent;
    float surface_specularSkin;

    // Additional properties
    float surface_fresnel;
    float surface_emissiveIntensity;
    float surface_selfShadow;

    // Optional reflection color
    float3 surface_reflectionColor;
};

struct StandardLightingProperties
{
    float3 diffuseColor;
    float3 specularColor;
    float specularIntensity;
    float specularExponent;
    float fresnel;
    float emissive;
};

struct surfaceProperties
{
    float3 position;
    float3 normal;
    float3 viewDirection;
};

struct materialProperties
{
    float3 diffuseColor;
    float3 specularColor;
    float specularIntensity;
    float specularPower;
    float fresnel;
};

struct directionalLightProperties
{
    float3 direction;
    float3 color;
    float3 ambientColor;
};

StandardLightingProperties DeriveLightingPropertiesForCommonSurface(SurfaceProperties surfaceInfo)
{
    StandardLightingProperties props;

    props.diffuseColor = surfaceInfo.surface_diffuseColor.rgb;
    props.specularColor = surfaceInfo.surface_baseColor.rgb;
    props.specularIntensity = surfaceInfo.surface_specularIntensity;
    props.specularExponent = surfaceInfo.surface_specularExponent;
    props.fresnel = surfaceInfo.surface_fresnel;
    props.emissive = surfaceInfo.surface_emissiveIntensity;

    return props;
}

float3 ProcessDiffuseColor(float3 diffuseColor)
{
    return saturate(diffuseColor);
}

void populateForwardLightingStructs(
    out surfaceProperties surface,
    out materialProperties material,
    out directionalLightProperties light,
    float3 worldPos,
    float3 worldNormal,
    StandardLightingProperties standardProps)
{
    surface.position = worldPos;
    surface.normal = normalize(worldNormal);
    surface.viewDirection = float3(0, 0, 1); // Will be set by caller if needed

    material.diffuseColor = standardProps.diffuseColor;
    material.specularColor = standardProps.specularColor;
    material.specularIntensity = standardProps.specularIntensity;
    material.specularPower = standardProps.specularExponent;
    material.fresnel = standardProps.fresnel;

    // Light properties will be filled from global light params
    light.direction = float3(0, 0, -1);
    light.color = float3(1, 1, 1);
    light.ambientColor = float3(0.2, 0.2, 0.2);
}

float CalculateFresnel(float3 normal, float3 viewDir, float fresnelPower)
{
    float fresnel = 1.0 - saturate(dot(normal, -viewDir));
    return pow(fresnel, fresnelPower);
}

float3 CalculateSpecular(
    float3 normal,
    float3 lightDir,
    float3 viewDir,
    float3 specularColor,
    float specularIntensity,
    float specularPower,
    float lightIntensity)
{
    float3 halfVector = normalize(lightDir + (-viewDir));
    float NdotH = saturate(dot(normal, halfVector));
    float specular = pow(NdotH, specularPower);

    return specularColor * specularIntensity * specular * lightIntensity;
}

float3 CalculateDirectionalLight(
    surfaceProperties surface,
    materialProperties material,
    directionalLightProperties light,
    float shadowFactor)
{
    // Diffuse term
    float NdotL = saturate(dot(surface.normal, light.direction));
    float3 diffuse = material.diffuseColor * light.color * NdotL * shadowFactor;

    // Specular term
    float3 specular = float3(0, 0, 0);
    if (NdotL > 0)
    {
        specular = CalculateSpecular(
            surface.normal,
            light.direction,
            surface.viewDirection,
            material.specularColor,
            material.specularIntensity,
            material.specularPower,
            shadowFactor);
    }

    return diffuse + specular;
}

float4 calculateForwardLighting(
    int numLights,
    bool useLights,
    surfaceProperties surface,
    materialProperties material,
    directionalLightProperties light,
    bool useDirectional,
    bool directionalShadow,
    bool directionalShadowHighQuality,
    float2 screenPos)
{
    float3 result = float3(0, 0, 0);

    // Directional light
    if (useDirectional)
    {
        float shadowFactor = directionalShadow ? 1.0 : 1.0; // Shadow will be applied externally
        result += CalculateDirectionalLight(surface, material, light, shadowFactor);
    }

    // Ambient light
    result += light.ambientColor * material.diffuseColor;

    // Additional point/spot lights would go here
    // (not implemented for now as they're handled separately in deferred rendering)

    return float4(result, 1.0);
}

float3 EnhancedLighting(
    float3 diffuseColor,
    float3 specularColor,
    float3 normal,
    float3 viewDirection,
    float4 vertexColor,
    float specularIntensity,
    float specularPower,
    float fresnel,
    uniform ShaderGlobalLightParams globalLights,
    float shadowFactor)
{
    float3 lightDir = normalize(globalLights.LightDir.xyz);

    // Diffuse lighting
    float NdotL = saturate(dot(normal, lightDir));
    float3 diffuse = diffuseColor * globalLights.LightDirColour.rgb * NdotL * shadowFactor;

    // Specular lighting
    float3 specular = float3(0, 0, 0);
    if (NdotL > 0 && specularIntensity > 0)
    {
        specular = CalculateSpecular(
            normal,
            lightDir,
            viewDirection,
            specularColor,
            specularIntensity,
            specularPower,
            shadowFactor);
    }

    // Ambient lighting with vertex color factors
    float naturalAmbient = vertexColor.r;
    float artificialAmbient = saturate(vertexColor.g);

    float3 ambientContribution = diffuseColor * globalLights.LightDirAmbColour.rgb;
    ambientContribution += AmbientLight(diffuseColor, normal.z,
        globalLights.LightNaturalAmbUp, globalLights.LightNaturalAmbDown, naturalAmbient);
    ambientContribution += AmbientLight(diffuseColor, normal.z,
        globalLights.LightArtificialAmbUp, globalLights.LightArtificialAmbDown, artificialAmbient);

    return diffuse + specular + ambientContribution;
}

float ApplyWetnessToSpecular(float baseSpecular, float wetness, float wetnessMultiplier)
{
    return baseSpecular * (1.0 + wetness * wetnessMultiplier);
}

float CalculateSelfShadow(float3 normal, float3 lightDir, float selfShadowStrength)
{
    float NdotL = dot(normal, lightDir);
    return saturate(NdotL * selfShadowStrength + (1.0 - selfShadowStrength));
}

#endif // __LIGHTING_HLSLI__
