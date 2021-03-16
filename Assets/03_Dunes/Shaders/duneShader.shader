  Shader "03 Dunes/Sand Shader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _PaintMap("PaintMap", 2D) = "white" {} // texture to paint on
        _SandColor0("Sand", Color) = (0, 0, 0, 1)
        _SandColor1("Sand", Color) = (0, 0, 0, 1)
        _DarkSand0("Dark Sand", Color) = (0, 0, 0, 1)
        _DarkSand1("Dark Sand", Color) = (0, 0, 0, 1)
        _EdgeHardness("Edge Hardness", float) = 0
    }

    SubShader {
      Tags { "RenderType" = "Opaque" }

      CGPROGRAM

      #pragma surface surf Lambert 

      struct Input {
          float2 uv_MainTex;
          float2 uv_PaintMap;
      };

        sampler2D _MainTex;
        sampler2D _PaintMap;
        float4 _SandColor0, _SandColor1, _DarkSand0, _DarkSand1;
        float _EdgeHardness;
        float _Displacement;

      void surf (Input IN, inout SurfaceOutput o) {
          half4 paint = tex2D(_PaintMap, IN.uv_PaintMap);
          paint = saturate(paint * _EdgeHardness);

          half4 t = tex2D(_MainTex, IN.uv_MainTex);

          half4 lightSand = lerp(_SandColor0, _SandColor1, t.r);
          half4 darkSand = lerp(_DarkSand0, _DarkSand1, t.r);

          o.Albedo = lerp(lightSand, darkSand, paint);
          // o.Albedo = paint;

      }
      ENDCG
    } 
    Fallback "Diffuse"
  }