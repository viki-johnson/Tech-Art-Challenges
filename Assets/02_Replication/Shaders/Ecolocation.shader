//based on https://github.com/Broxxar/NoMansScanner

Shader "02 Replication/Ecolocation"
{
    Properties
    {
        _Black("Black", Color) = (1, 1, 1, 0)
        _Edge("Edge Softness", float) = 0
		_Radius(" -- ", float) = 0
    }
    
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct VertIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 ray : TEXCOORD1;
			};

			struct VertOut
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uv_depth : TEXCOORD1;
				float4 interpolatedRay : TEXCOORD2;
			};

			float4 _MainTex_TexelSize;
			float4 _CameraWS;

			VertOut vert(VertIn v)
			{
				VertOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv.xy;
				o.uv_depth = v.uv.xy;

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1 - o.uv.y;
				#endif				

				o.interpolatedRay = v.ray;

				return o;
			}

            sampler2D _MainTex, _CameraDepthTexture;

            float4 _WorldSpaceScannerPos[100];
            float _ScanDistanceArr[100];
            int _ArrayLength;

            float4 _Black;
			float _Edge, _Radius;

			half4 frag (VertOut i) : SV_Target
            {

                half4 col = tex2D(_MainTex, i.uv);

                float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv_depth));
                float linearDepth = Linear01Depth(rawDepth);
                float4 wsDir = linearDepth * i.interpolatedRay;
                float3 wsPos = _WorldSpaceCameraPos + wsDir;
                half4 scannerCol = _Black;

                float mask = 0;

                for(int i=0; i<_ArrayLength; i++)
                {
                    float3 dist = distance(wsPos, _WorldSpaceScannerPos[i]);
                    float3 sphere = 1 - saturate(dist/_ScanDistanceArr[i]);
                    sphere = saturate(sphere* _Edge);

                    mask += sphere.r;
                }

                mask = saturate(mask);
                return lerp(scannerCol, col, mask);

            }
			
			ENDCG
		}
	}
}