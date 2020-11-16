Shader "custom/paintDiffuse" {
 
    Properties{
        _MainTex("Main Texture", 2D) = "white" {}
        _HiddenTex("Hidden Texture", 2D) = "white" {}
        _SandColor("Sand", Color) = (0, 0, 0, 1)
        _DarkSand("Dark Sand", Color) = (0, 0, 0, 1)
        _PaintMap("PaintMap", 2D) = "white" {} // texture to paint on
    }
 
        SubShader{
        Tags{ "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
 
        Pass{
 
        Lighting Off
 
        CGPROGRAM
 
#pragma vertex vert
#pragma fragment frag
 
#include "UnityCG.cginc"
#include "AutoLight.cginc"
 
    struct v2f {
        float4 pos : SV_POSITION;
        float2 uv0 : TEXCOORD0;
        float2 uv1 : TEXCOORD1;
 
    };
 
    struct appdata {
        float4 vertex : POSITION;
        float2 texcoord : TEXCOORD0;
        float2 texcoord1 : TEXCOORD1;
 
    };
 
    sampler2D _PaintMap;
    sampler2D _MainTex, _HiddenTex;
    float4 _MainTex_ST;
    float4 _SandColor, _DarkSand;
    v2f vert(appdata v) {
        v2f o;
 
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv0 = TRANSFORM_TEX(v.texcoord, _MainTex);
 
        o.uv1 = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;// lightmap uvs
 
        return o;
    }
 
    half4 frag(v2f o) : COLOR{
        // half4 mainCol = tex2D(_MainTex, o.uv0); // main texture
        // half4 hiddenCol = tex2D(_HiddenTex, o.uv0);
        half4 paint = (tex2D(_PaintMap, o.uv1)); // painted on texture


        // mainCol *= paint; // add paint to main;
        // return (mainCol*paint) + (hiddenCol*(1-paint));
        return lerp(_DarkSand, _SandColor, paint);
    }
        ENDCG
    }
    }
}

