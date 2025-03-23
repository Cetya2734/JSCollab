Shader "Custom/URP/VertexRipple"
{
    Properties
    {
        _MainTex("_MainTex", 2D) = "black" {}
        _Speed("_Speed", Range(0,3)) = 0.5
        _Scale("_Scale", Range(0,4)) = 0.193
        _WaveLength("_WaveLength", Float) = 0
    }

    SubShader
    {
        Tags 
        { 
            "RenderPipeline" = "UniversalRenderPipeline"
            "Queue" = "Geometry" 
            "RenderType" = "Opaque"
        }

        Cull Off
        ZWrite On
        ZTest LEqual

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR; // Using vertex color
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float _Speed;
            float _Scale;
            float _WaveLength;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 vertexPosition = IN.positionOS.xyz;
                
                // Ripple Calculation
                float timeHeightOffset = vertexPosition.y * _WaveLength;
                float waveTime = (timeHeightOffset + _Time.y) * _Speed;
                float positionOffset = sin(waveTime) * _Scale * IN.color.r;

                vertexPosition.z += positionOffset; // Apply ripple

                OUT.positionCS = TransformObjectToHClip(vertexPosition);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
            }
            ENDHLSL
        }
    }
}
