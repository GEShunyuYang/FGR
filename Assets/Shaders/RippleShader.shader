Shader "FGR/RippleShader"
{
    Properties
    {
        _MainTex ("Ripple Mask", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (0.75, 0.9, 1, 0.45)

        _DistortionStrength ("Distortion Strength", Range(0, 0.1)) = 0.025
        _CenterDistortionRadius ("Center Distortion Radius", Range(0, 1)) = 0.35
        _RingWidth ("Ring Width", Range(0.01, 0.5)) = 0.12

        _Alpha ("Alpha", Range(0, 1)) = 0.65
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent-40"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _TintColor;
                float _DistortionStrength;
                float _CenterDistortionRadius;
                float _RingWidth;
                float _Alpha;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 color : COLOR;
                float4 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float4 color : COLOR;
                float randomValue : TEXCOORD2;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;

                VertexPositionInputs pos = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = pos.positionCS;
                output.screenPos = ComputeScreenPos(pos.positionCS);
                output.color = input.color;
                output.uv = TRANSFORM_TEX(input.uv.xy, _MainTex);
                output.randomValue = input.uv.z;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv;
                

                float2 centerVec = uv - 0.5;
                float dist = length(centerVec);
                float2 dir = normalize(centerVec + 0.0001);

                half4 maskTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);

                float rippleCount = floor(saturate(input.randomValue) * 3.0) + 1.0;

                float maxRadius = _CenterDistortionRadius;
                float normalizedDist = saturate(dist / maxRadius);

                float particleMask = 1.0 - smoothstep(0.48, 0.5, dist);

                float phase = normalizedDist * rippleCount;
                float ringDistance = abs(frac(phase) - 0.5);

                float ring = 1.0 - smoothstep(_RingWidth, _RingWidth + 0.025, ringDistance);

                float innerFade = smoothstep(0.05, 0.18, normalizedDist);
                float outerFade = 1.0 - smoothstep(0.92, 1.0, normalizedDist);
                ring *= innerFade * outerFade * particleMask;

                float centerMask = 1.0 - smoothstep(maxRadius * 0.25, maxRadius * 0.7, dist);
                centerMask *= particleMask;

                float distortionMask = saturate(ring * 0.9 + centerMask * 0.25);
                float2 distortion = dir * distortionMask * _DistortionStrength;

                float2 screenUV = input.screenPos.xy / input.screenPos.w;

                half3 originalScene = SAMPLE_TEXTURE2D(
                    _CameraOpaqueTexture,
                    sampler_CameraOpaqueTexture,
                    screenUV
                ).rgb;

                half3 distortedScene = SAMPLE_TEXTURE2D(
                    _CameraOpaqueTexture,
                    sampler_CameraOpaqueTexture,
                    screenUV + distortion
                ).rgb;

                half distortionAlpha = saturate(ring * 0.85 + centerMask * 0.2);
                half rippleAlpha = ring;

                half effectMask = saturate(distortionAlpha + rippleAlpha * 0.5);
                effectMask *= _Alpha * input.color.a;

                half3 color = lerp(originalScene, distortedScene, distortionAlpha);
                //color += _TintColor.rgb * rippleAlpha * 0.35;

                return half4(color, effectMask);
            }

            ENDHLSL
        }
    }
}