#include "BasicPS.hlsli"
#include "ParallaxMapping.hlsli"


PS_OUTPUT main(VS_OUTPUT input)
{
    float4 c = float4(0.5, 0.5, 0.5, 1);
    if (RenderMode == 0) c = float4(1, 1, 1, 1);
    if (EnableTexture > 0)
    {
        float2 texc = input.Texcoord0;
        if (RenderMode >= 5)
        {
            if (RenderSamplerCoord == 2)
                texc = input.Texcoord1;
            else if (RenderSamplerCoord == 3)
                texc = input.Texcoord2;
        }

        // Apply parallax mapping in deferred renderer (same as forward renderer)
        if (EnableParallax && EnableNormalMap && RenderMode == 0)
        {
            // Calculate view direction in tangent space for parallax
            float3 viewDirWorld = normalize(input.CamRelPos);
            float3 viewDirTangent;
            viewDirTangent.x = dot(viewDirWorld, input.Tangent.xyz);
            viewDirTangent.y = dot(viewDirWorld, input.Bitangent.xyz);
            viewDirTangent.z = dot(viewDirWorld, input.Normal.xyz);

            // Fade out parallax at grazing angles to reduce noise/artifacts
            float viewDotNormal = saturate(abs(viewDirTangent.z));
            float parallaxFade = smoothstep(0.0, 0.3, viewDotNormal);

            if (parallaxFade > 0.01)
            {
                // Sample height map to get height data (R channel)
                float4 initialHeight = Heightmap.Sample(TextureSS, texc);

                // Scale parallax intensity by fade factor
                float scaledParallaxScale = parallaxScale * parallaxFade;

                // Apply parallax offset
                float2 parallaxTexCoord;
                if (parallaxNumSteps > 0)
                {
                    // Steep parallax (higher quality)
                    float4 unused = CalculateParallaxSteep(
                        viewDirTangent,
                        initialHeight,
                        texc,
                        Heightmap,
                        TextureSS,
                        scaledParallaxScale,
                        parallaxNumSteps,
                        parallaxTexCoord);
                }
                else
                {
                    // Standard parallax (faster)
                    // Use R channel for height (GTA V pxm format)
                    parallaxTexCoord = CalculateParallaxStandard(
                        viewDirTangent,
                        initialHeight.r,
                        scaledParallaxScale,
                        parallaxBias,
                        texc);
                }

                texc = parallaxTexCoord;
            }
        }

        c = Colourmap.Sample(TextureSS, texc);

        if (EnableTexture > 1) //2+ enables diffuse2
        {
            float4 c2 = Colourmap2.Sample(TextureSS, input.Texcoord1);
            c = c2.a * c2 + (1 - c2.a) * c;
        }
        if (EnableTint == 2)
        {
            //weapon tint
            float tx = (round(c.a * 255.009995) - 32.0) * 0.007813; //okay R* this is just silly
            float ty = 0.03125 * 0.5; // //1;//what to use for Y value? cb12[2].w in R* shader
            float4 c3 = TintPalette.Sample(TextureSS, float2(tx, ty));
            c.rgb *= c3.rgb;
            c.a = 1;
        }

        if (IsDistMap) c = float4(c.rgb * 2, (c.r + c.g + c.b) - 1);
        if (IsDecal == 4) c.a = c.r;
        if ((IsDecal == 0) && (c.a <= 0.33)) discard;
        if ((IsDecal == 1) && (c.a <= 0.0)) discard;
        if ((IsDecal >= 3) && (c.a <= 0.0)) discard;
        if (IsDecal == 0) c.a = 1;
        if (IsDecal == 2)
        {
            float4 mask = TextureAlphaMask * c;
            c.a = saturate(mask.r + mask.g + mask.b + mask.a);
            c.rgb = 0;
        }
        c.a = saturate(c.a * AlphaScale);
    }
    if (EnableTint == 1)
    {
        c.rgb *= input.Tint.rgb;
    }
    if ((IsDecal == 1) || (IsDecal >= 3))
    {
        c.a *= input.Colour0.a;
    }

    float3 norm = normalize(input.Normal);

    if (RenderMode == 1) //normals
    {
        c.rgb = norm * 0.5 + 0.5;
    }
    else if (RenderMode == 2) //tangents
    {
        c.rgb = normalize(input.Tangent.rgb) * 0.5 + 0.5;
    }
    else if (RenderMode == 3) //colours
    {
        c.rgb = input.Colour0.rgb;
        if (RenderModeIndex == 2)
            c.rgb = input.Colour1.rgb;
    }
    else if (RenderMode == 4) //texcoords
    {
        c.rgb = float3(input.Texcoord0, 0);
        if (RenderModeIndex == 2) c.rgb = float3(input.Texcoord1, 0);
        if (RenderModeIndex == 3) c.rgb = float3(input.Texcoord2, 0);
    }


    float3 spec = 0;

    if (RenderMode == 0)
    {
        // Calculate parallax-adjusted coordinates for normal/specular maps (same logic as diffuse)
        float2 normalSpecTexCoord = input.Texcoord0;

        if (EnableParallax && EnableNormalMap)
        {
            // Calculate view direction in tangent space for parallax
            float3 viewDirWorld = normalize(input.CamRelPos);
            float3 viewDirTangent;
            viewDirTangent.x = dot(viewDirWorld, input.Tangent.xyz);
            viewDirTangent.y = dot(viewDirWorld, input.Bitangent.xyz);
            viewDirTangent.z = dot(viewDirWorld, input.Normal.xyz);

            // Fade out parallax at grazing angles to reduce noise/artifacts
            float viewDotNormal = saturate(abs(viewDirTangent.z));
            float parallaxFade = smoothstep(0.0, 0.3, viewDotNormal);

            if (parallaxFade > 0.01)
            {
                // Sample height map to get height data (R channel)
                float4 initialHeight = Heightmap.Sample(TextureSS, normalSpecTexCoord);

                // Scale parallax intensity by fade factor
                float scaledParallaxScale = parallaxScale * parallaxFade;

                // Apply parallax offset
                float2 parallaxTexCoord;
                if (parallaxNumSteps > 0)
                {
                    // Steep parallax (higher quality)
                    float4 unused = CalculateParallaxSteep(
                        viewDirTangent,
                        initialHeight,
                        normalSpecTexCoord,
                        Heightmap,
                        TextureSS,
                        scaledParallaxScale,
                        parallaxNumSteps,
                        parallaxTexCoord);
                }
                else
                {
                    // Standard parallax (faster)
                    parallaxTexCoord = CalculateParallaxStandard(
                        viewDirTangent,
                        initialHeight.r,
                        scaledParallaxScale,
                        parallaxBias,
                        normalSpecTexCoord);
                }

                normalSpecTexCoord = parallaxTexCoord;
            }
        }

        float4 nv = Bumpmap.Sample(TextureSS, normalSpecTexCoord);
        float4 sv = Specmap.Sample(TextureSS, normalSpecTexCoord);


        float2 nmv = nv.xy;
        float4 r0 = 0, r1, r2, r3;

        if (EnableNormalMap)
        {
            if (EnableDetailMap)
            {
                //detail normalmapp (use parallax-adjusted coords if enabled)
                r0.xy = normalSpecTexCoord * detailSettings.zw;
                r0.zw = r0.xy * 3.17;
                r0.xy = Detailmap.Sample(TextureSS, r0.xy).xy - 0.5;
                r0.zw = Detailmap.Sample(TextureSS, r0.zw).xy - 0.5;
                r0.xy = r0.xy + r0.zw;
                r0.yz = r0.xy * detailSettings.y; //r0.x = -r0.x*detailSettings.x;
                nmv = r0.yz * sv.w + nv.xy; //add detail to normal, using specmap(!)
            }

            norm = NormalMap(nmv, bumpiness, input.Normal.xyz, input.Tangent.xyz, input.Bitangent.xyz);


        }
        


        if (EnableSpecMap == 0)
        {
            sv = float4(0.1, 0.1, 0.1, 0.1);
        }

        float r1y = norm.z - 0.35;

        float3 globalScalars = float3(0.5, 0.5, 0.5);
        float globalScalars2z = 1; // 0.65; //wet darkness?
        float wetness = 0; // 10.0;

        r0.x = 0; // .5;
        r0.z = 1 - globalScalars2z;
        r0.y = saturate(r1y * 1.538462);
        r0.y = r0.y * wetness;
        r0.y = r0.y * r0.z;
        r1.yz = input.Colour0.xy * globalScalars.zy;
        r0.y = r0.y * r1.y;
        r0.x = r0.x * sv.w + 1.0;
        sv.xy = sv.xy * sv.xy;
        r0.z = sv.w * specularFalloffMult;
        r3.y = r0.z * 0.001953125; // (1/512)
        r0.z = dot(sv.xyz, specMapIntMask);
        r0.z = r0.z * specularIntensityMult;
        r3.x = r0.x * r0.z;
        r0.z = saturate(r0.z * r0.x + 0.4);
        r0.z = 1 - r3.x * 0.5;
        r0.z = r0.z * r0.y;
        r0.y = r0.y * wetnessMultiplier;
        r0.z = 1 - r0.z * 0.5;

        float3 tc = c.rgb * r0.x;
        c.rgb = tc * r0.z; //diffuse factors...


        spec.xy = sqrt(r3.xy);
        spec.z = r0.z;
        
    }


    float emiss = (IsEmissive == 1) ? 1.0 : 0.0;

    c.a = saturate(c.a);
    
    
    float4 a = c.aaaa;
    if(IsDecal==3) a.xzw = 0; //normal_only
    if(IsDecal==4) a.xyw = 0; //spec_only
    
    
    PS_OUTPUT output;
    output.Diffuse = float4(c.rgb, a.x);
    output.Normal = float4(saturate(norm * 0.5 + 0.5), a.y);
    output.Specular = float4(spec, a.z);
    output.Irradiance = float4(input.Colour0.rg, emiss, a.w);

    return output;
}



