Shader "07 Jellyfish/Jellyfish legs"
{
    Properties {
		_Color1 ("Color 1", Color) = (0,0,0,1)
		_Color2 ("Color 2", Color) = (1,1,1,1)
		_Color3 ("Color 3", Color) = (1,1,1,1)

		_Tiling ("Tiling", Range(1, 500)) = 10
		_Width ("Width", float) = 0
		_WarpScale ("Warp Scale", Range(0, 1)) = 0
		_WarpTiling ("Warp Tiling", Range(1, 10)) = 1
        _Speed ("Speed", float) = 0
        _NoiseTex ("Noise Tex", 2D) = "white" {}
		_MaskTex ("Mask tex", 2D) = "white" {}
		_Alpha ("Alpha", float) = 0
	}

	SubShader
	{

		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100
		Zwrite Off
		Blend SrcAlpha OneMinusSrcAlpha 

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"


			fixed4 _Color1, _Color2, _Color3;
			int _Tiling;
			// float _Direction;
			float _WarpScale;
			float _WarpTiling;
            float _Speed, _Width, _Alpha;

            sampler2D _NoiseTex, _MaskTex;
            float4 _NoiseTex_ST, _MaskTex_ST;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _NoiseTex);
				o.uv1 = TRANSFORM_TEX(v.uv1, _MaskTex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				const float PI = 3.14159;

                float4 noise = tex2D(_NoiseTex, i.uv);

				float4 mask = tex2D(_MaskTex, i.uv1);


                float2 uv = i.uv;

                // uv.x *= noise.r;
                // uv.y += _Time;

                // i.uv.x += _Time*_Speed;

				float2 pos = i.uv;
                pos.x *= noise.r;
                pos.y *= noise.r;

                pos.y += _Time*_Speed;

				// pos.x = lerp(i.uv.x, i.uv.y, _Direction);
				// pos.y = lerp(i.uv.y, 1 - i.uv.x, _Direction);

				pos.x += sin(pos.y * _WarpTiling * PI * 2) * _WarpScale;
				pos.x *= _Tiling;

				fixed value = floor(frac(pos.x) + _Width);
				fixed4 a = lerp(_Color1, _Color2, value);
				fixed4 col = _Color3;
				col.a = a.r * _Alpha * mask.r;

				// fixed4 mask = tex2D(_MaskTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}