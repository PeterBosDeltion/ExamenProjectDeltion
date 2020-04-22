Shader "Hidden/Griffin/AlbedoPainter"
{
    Properties
    {
		_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "black" {}
		_Mask ("Mask", 2D) = "white" {}
		_Opacity ("Opacity", Float) = 1
    }

	CGINCLUDE
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
		float4 localPos : TEXCOORD1;
    };

	float4 _Color;
    sampler2D _MainTex;
	float4 _MainTex_TexelSize;
	sampler2D _Mask;
	float _Opacity;

	v2f vert (appdata v)
    {
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		o.localPos = v.vertex;
		return o;
    }

	fixed4 fragPaint (v2f i) : SV_Target
    {
		float4 maskColor = tex2D(_Mask, i.uv);
		float maskGray = maskColor.r;
		float4 overlayColor = float4(_Color.rgb, _Color.a*maskGray*_Opacity);
		float4 currentColor = tex2D(_MainTex, i.localPos);

		float4 desColor = overlayColor*overlayColor.a + currentColor*(1-overlayColor.a);
		float alpha = 1 - (1-overlayColor.a)*(1-currentColor.a);

		return float4(desColor.rgb, alpha);
	}

	fixed4 fragErase (v2f i) : SV_Target
	{
		float4 backgroundColor = float4(0,0,0,0);
		float4 maskColor = tex2D(_Mask, i.uv);
		float maskGray = maskColor.r;
		float f = maskGray*_Opacity;
		float4 currentColor = tex2D(_MainTex, i.localPos);
		float4 dir = normalize(backgroundColor-currentColor)*f;
		float4 desColor = saturate(currentColor + dir);

		return desColor;
	}
	
	fixed4 fragSmooth (v2f i) : SV_Target
	{
		float2 texel = _MainTex_TexelSize.xy;
		float4 avgColor = float4(0,0,0,0);
		float sampleCount = 0;
		for (int x0=-3; x0<=3; ++x0)
		{
			for (int y0=-3; y0<=3; ++y0)
			{
				avgColor += tex2D(_MainTex, i.localPos + float2(x0*texel.x, y0*texel.y));
				sampleCount +=1;
			}
		}
		avgColor = avgColor/sampleCount;

		float4 currentColor = tex2D(_MainTex, i.localPos);
		float4 maskColor = tex2D(_Mask, i.uv);

		float maskValue = maskColor.r;
		float4 desColor = lerp(currentColor, avgColor, maskValue*_Opacity);
		return desColor;
	}

	ENDCG

    SubShader
    {
        Tags { "RenderType"="Transparent" }
		
        Pass
        {
			Name "Paint"
			Blend One Zero
			BlendOp Add
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragPaint
            ENDCG
        }

		Pass
        {
			Name "Erase"
			Blend One Zero
			BlendOp Add
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragErase
            ENDCG
        }
		
		Pass
        {
			Name "Smooth"
			Blend One Zero
			BlendOp Add
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragSmooth
            ENDCG
        }
    }
}
