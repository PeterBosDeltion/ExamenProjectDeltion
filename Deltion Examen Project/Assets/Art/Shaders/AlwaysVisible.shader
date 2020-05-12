﻿Shader "Kaas/AlwaysVisible"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SeeThroughColor("Always visible color", Color) = (0,0,0,0)
        _Transparency("Transparency", float) = 1
    }

        CGINCLUDE
            #define UNITY_SETUP_BRDF_INPUT MetallicSetup
        ENDCG

    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Cull Off
            ZWrite Off
            ZTest Always

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            float4 _SeeThroughColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _SeeThroughColor;
            }

            ENDCG
        }       

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Transparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.a = _Transparency;
                return col;
            }
            ENDCG
        }
    }
}