Shader "Custom/LethalCompanyStyleURP"
{
    Properties
    {
        _BaseMap ("Albedo (RGB)", 2D) = "white" {}
        _ShadowDither ("Shadow Dither Intensity", Float) = 0.5
        _NoiseStrength ("Subtle Grain", Float) = 0.02
        _ShadowDarkness ("Shadow Darkness", Float) = 1.5
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "Queue"="Geometry" "RenderType"="Opaque" }
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : NORMAL;
                float4 positionCS : SV_POSITION;
            };

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            float _ShadowDither;
            float _NoiseStrength;
            float _ShadowDarkness;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normalOS);

                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.positionWS = positionInputs.positionWS;
                OUT.uv = IN.uv;
                OUT.normalWS = normalInputs.normalWS;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Sample base texture
                half4 col = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);

                // Get Normalized Normal
                float3 normal = normalize(IN.normalWS);

                // Initialize total light color
                float3 totalLightColor = 0;

                // Main Light Calculation
                Light mainLight = GetMainLight();
                float mainLightFactor = saturate(dot(normal, normalize(mainLight.direction)));

                // Apply main light shadow attenuation
                #if defined(_MAIN_LIGHT_SHADOWS)
                    mainLightFactor *= mainLight.shadowAttenuation;
                #endif

                // Add the main light color
                totalLightColor += mainLight.color * mainLightFactor;

                // Additional Lights Contribution (Now includes their colors)
                #if defined(_ADDITIONAL_LIGHTS)
                    uint numLights = GetAdditionalLightsCount();
                    for (uint i = 0; i < numLights; ++i)
                    {
                        Light additionalLight = GetAdditionalLight(i, IN.positionWS);
                        float lightFactor = saturate(dot(normal, normalize(additionalLight.direction)));

                        // Apply shadow attenuation for additional lights
                        lightFactor *= additionalLight.shadowAttenuation;

                        // Add additional light color
                        totalLightColor += additionalLight.color * lightFactor;
                    }
                #endif

                // Shadow Dithering (only applied to shadows)
                float2 screenUV = IN.positionCS.xy / IN.positionCS.w;
                float ditherPattern = frac(sin(dot(screenUV.xy, float2(12.9898, 78.233))) * 43758.5453);
                float shadowFactor = lerp(mainLightFactor, ditherPattern, _ShadowDither);

                // Adjust shadow darkness
                shadowFactor = pow(shadowFactor, _ShadowDarkness);

                // Subtle noise effect
                float noise = frac(sin(dot(IN.positionWS.xy, float2(12.9898, 78.233))) * 43758.5453);
                col.rgb += (noise - 0.5) * _NoiseStrength;

                // Apply total lighting with shadow effect
                col.rgb *= totalLightColor * shadowFactor;

                return col;
            }

            ENDHLSL
        }
    }
}
