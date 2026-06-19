Shader "FGR/RainReflect"
{
    Properties
    {
        _WetMaskTex ("Wet Noise Mask", 2D) = "gray" {}
        _WetMaskScale ("Wet Mask Scale", Float) = 0.006
        _WetMaskOffset ("Wet Mask Offset", Vector) = (0, 0, 0, 0)
        _WetThreshold ("Wet Threshold", Range(0, 1)) = 0.45
        _WetSoftness ("Wet Softness", Range(0.001, 0.5)) = 0.08
        _ReflectionAlpha ("Reflection Alpha", Range(0, 1)) = 0.35
    }

    SubShader
    {
        Tags { 
            "RenderType"="Transparent"
            "Queue" = "Transparent-50"
            "RenderPipeline"="UniversalPipeline"
        }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest LEqual
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_WetMaskTex);
            SAMPLER(sampler_WetMaskTex);

            TEXTURE2D(_PlanarReflectionTex);
            SAMPLER(sampler_PlanarReflectionTex);

            CBUFFER_START(UnityPerMaterial)
                float _WetMaskScale;
                float4 _WetMaskOffset;
                float _WetThreshold;
                float _WetSoftness;
                float _ReflectionAlpha;
            CBUFFER_END

            float4x4 _PlanarReflectionVP;

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;

                VertexPositionInputs pos = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = pos.positionCS;
                output.positionWS = pos.positionWS;
                output.screenPos = ComputeScreenPos(pos.positionCS);

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float4 reflClip = mul(_PlanarReflectionVP, float4(input.positionWS, 1));
                float2 reflUV = reflClip.xy / reflClip.w;
                reflUV = reflUV * 0.5 + 0.5;

                half3 reflected = SAMPLE_TEXTURE2D(
                    _PlanarReflectionTex,
                    sampler_PlanarReflectionTex,
                    reflUV
                ).rgb;

                float inside =
                    step(0.0, reflUV.x) * step(reflUV.x, 1.0) *
                    step(0.0, reflUV.y) * step(reflUV.y, 1.0);

                float2 wetUV = input.positionWS.xz * _WetMaskScale + _WetMaskOffset.xy;
                float wetNoise = SAMPLE_TEXTURE2D(_WetMaskTex, sampler_WetMaskTex, wetUV).r;
                float wetMask = smoothstep(_WetThreshold, _WetThreshold + _WetSoftness, wetNoise);

                float alpha = wetMask * _ReflectionAlpha * inside;
                return half4(reflected, alpha);
            }

            ENDHLSL
        }
    }
}
