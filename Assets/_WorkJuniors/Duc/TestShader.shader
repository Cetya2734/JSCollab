Shader "Custom/LethalCompanyStyleURP"
{
    Properties
    {
        _BaseMap ("Albedo (RGB)", 2D) = "white" {}
        _ShadowThreshold1 ("Shadow Layer 1 Threshold", Range(0,1)) = 0.8
        _ShadowThreshold2 ("Shadow Layer 2 Threshold", Range(0,1)) = 0.5
        _ShadowThreshold3 ("Shadow Layer 3 Threshold", Range(0,1)) = 0.2
        _PixelSize ("Dither Pixel Size", Float) = 16.0
        _OutlineThickness ("Outline Thickness", Float) = 0.002
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "Queue"="Geometry" "RenderType"="Opaque" }

        Pass // Main Cel-Shaded + Pixel Dither Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
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
            float _ShadowThreshold1;
            float _ShadowThreshold2;
            float _ShadowThreshold3;
            float _PixelSize;

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
                half4 col = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);

                // Lighting
                Light mainLight = GetMainLight();
                float3 normal = normalize(IN.normalWS);
                float shadow = dot(normal, normalize(mainLight.direction));

                // Multi-Layered Shadows
                float shadowLayer = 1.0;
                if (shadow < _ShadowThreshold1) shadowLayer = 0.75;
                if (shadow < _ShadowThreshold2) shadowLayer = 0.5;
                if (shadow < _ShadowThreshold3) shadowLayer = 0.25;

                // Pixel Halftone Effect
                float2 pixelatedUV = floor(IN.positionCS.xy / _PixelSize) * _PixelSize;
                float ditherPattern = frac(sin(dot(pixelatedUV, float2(12.9898, 78.233))) * 43758.5453);
                shadowLayer = lerp(shadowLayer, ditherPattern, 0.3);

                // Apply shading
                col.rgb *= shadowLayer;
                return col;
            }

            ENDHLSL
        }

        Pass // Outline Pass
        {
            Name "Outline"
            Tags { "LightMode"="SRPDefaultUnlit" }

            Cull Front // Render only backfaces for outline
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            float _OutlineThickness;
            float4 _OutlineColor;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                float3 offset = normalWS * _OutlineThickness;

                OUT.positionCS = TransformWorldToHClip(positionInputs.positionWS + offset);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return _OutlineColor;
            }

            ENDHLSL
        }
    }
}
