Shader "FGR/IndreactGrassShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags 
        { "RenderType" = "TransparentCutout"
          "Queue" = "AlphaTest"
          "RenderPipeline" = "UniversalPipeline"}

        LOD 100

        Pass
        {
            Cull Off
            ZWrite On

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5

            #include "UnityCG.cginc"

            StructuredBuffer<float4x4> _Matrices;

            float4 _BaseColor;
            float4 _PlayerPosRadius;
            float _BendStrength;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                uint instanceID : SV_InstanceID;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Cutoff;

            v2f vert (appdata v)
            {
                float4x4 m = _Matrices[v.instanceID];

                float3 localPos = v.vertex;

                float3 worldPos = mul(m, float4(localPos, 1)).xyz;

                float3 playerPos = _PlayerPosRadius.xyz;
                float radius = _PlayerPosRadius.w;

                float dist = distance(worldPos.xz, playerPos.xz);
                float bend = saturate(1.0 - dist / radius);

                float3 away = normalize(float3(worldPos.x - playerPos.x, 0, worldPos.z - playerPos.z) + 0.0001);
                worldPos += away * bend * _BendStrength * localPos.y;

                v2f o;
                o.vertex = UnityWorldToClipPos(worldPos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                clip(col.a - _Cutoff);
                return col;
            }
            ENDCG
        }
    }
}
