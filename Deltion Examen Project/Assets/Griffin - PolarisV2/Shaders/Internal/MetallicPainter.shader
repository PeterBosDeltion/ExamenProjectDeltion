Shader "Hidden/Griffin/MetallicPainter"
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
		float4 currentColor = tex2D(_MainTex, i.localPos);
		float currentValue = currentColor.r;
		float addictiveValue = maskGray*_Color.r*_Opacity;
		float newValue = saturate(currentValue + addictiveValue);

		return float4(newValue, newValue, newValue, currentColor.a); //keep the alpha channel for smoothness value
	}

	fixed4 fragErase (v2f i) : SV_Target
	{
		float4 maskColor = tex2D(_Mask, i.uv);
		float maskGray = maskColor.r;
		float4 currentColor = tex2D(_MainTex, i.localPos);
		float currentValue = currentColor.r;
		float subtractiveValue = maskGray*_Color.r*_Opacity;
		float newValue = saturate(currentValue - subtractiveValue);

		return float4(newValue, newValue, newValue, currentColor.a); //keep the alpha channel for smoothness value
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
		float newValue = desColor.r;
		return float4(newValue, newValue, newValue, currentColor.a);
	}

	fixed4 fragSet (v2f i) : SV_Target
    {
		float4 currentColor = tex2D(_MainTex, i.localPos);
		float4 maskColor = tex2D(_Mask, i.uv);
		float maskGray = maskColor.r;
		float newValue = lerp(currentColor.r, _Color.r*_Opacity, maskGray);

		return float4(newValue, newValue, newValue, currentColor.a); //keep the alpha channel for smoothness value
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

		Pass
        {
			Name "Set"
			Blend One Zero
			BlendOp Add
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragSet
            ENDCG
        }
    }
}
