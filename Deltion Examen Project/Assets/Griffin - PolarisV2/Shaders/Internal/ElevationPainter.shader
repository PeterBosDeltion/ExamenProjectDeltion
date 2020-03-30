Shader "Hidden/Griffin/ElevationPainter"
{
    Properties
    {
		_MainTex ("MainTex", 2D) = "black" {}
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

	fixed4 fragRaise (v2f i) : SV_Target
    {
		float4 maskColor = tex2D(_Mask, i.uv);
		float4 currentColor = tex2D(_MainTex, i.localPos);
		float4 desColor = currentColor + maskColor.rrrr*_Opacity;
		float gray = desColor.r;

		return saturate(float4(gray, currentColor.g, currentColor.b, currentColor.a));
	}

	
	fixed4 fragLower (v2f i) : SV_Target
	{
		float4 maskColor = tex2D(_Mask, i.uv);
		float4 currentColor = tex2D(_MainTex, i.localPos);
		float4 desColor = currentColor - maskColor.rrrr*_Opacity;
		float gray = desColor.r;

		return saturate(float4(gray, currentColor.g, currentColor.b, currentColor.a));
	}

	fixed4 fragSmooth (v2f i) : SV_Target
	{
		float2 texel = _MainTex_TexelSize.xy;
		float4 avgColor = float4(0,0,0,0);
		float sampleCount = 0;
		for (int x0=-25; x0<=25; ++x0)
		{
			for (int y0=-25; y0<=25; ++y0)
			{
				avgColor += tex2D(_MainTex, i.localPos + float2(x0*texel.x, y0*texel.y));
				sampleCount +=1;
			}
		}
		avgColor = avgColor/sampleCount;

		float4 currentColor = tex2D(_MainTex, i.localPos);
		float4 maskColor = tex2D(_Mask, i.uv);

		float maskValue = maskColor.r;
		float currentGray = currentColor.r;
		float avgGray = avgColor.r;

		float value = lerp(currentGray, avgGray, maskValue*_Opacity);
		return saturate(float4(value, currentColor.g, currentColor.b, currentColor.a));
	}
	ENDCG

    SubShader
    {
        Tags { "RenderType"="Transparent" }
		
        Pass
        {
			Name "Raise"
			Blend One Zero
			BlendOp Add
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragRaise
            ENDCG
        }
		
		Pass
        {
			Name "Lower"
			Blend One Zero
			BlendOp Add
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragLower
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
