Shader "07 Jellyfish/Jellyfish body" {
  Properties {
    [HDR] _Color0 ("Color", Color) = (1,1,1,1)
    [HDR] _Color1 ("Color", Color) = (1,1,1,1)

    _NoiseTex   ("Noise", 2D) = "white" {}
    _DispTex    ("Displacement", 2D) = "white" {}
    _GradTex    ("Others", 2D) = "white" {}

    _Glossiness ("Smoothness", Range(0,1)) = 0.5
    _Metallic ("Metallic", Range(0,1)) = 0.0

    _Displacement (" displacement", Range(0,2)) = 0.1
    _Speed ("speed", float) = 0

    _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
    [PowerSlider(4)] _FresnelExponent ("Fresnel Exponent", Range(0.25, 20)) = 1



        _FadeAdd ("Fade Add", float) = 0
        _FadeMul ("Fade Mul", float) = 0

  }
  SubShader {
    Tags{ "RenderType"="Opaque" "Queue"="Geometry"}
    // Blend SrcAlpha OneMinusSrcAlpha
    LOD 200
    
    CGPROGRAM
    // Physically based Standard lighting model, and enable shadows on all light types
    #pragma surface surf Standard vertex:vert addshadow
    

    // Use shader model 3.0 target, to get nicer looking lighting
    #pragma target 3.0

    sampler2D _NoiseTex, _DispTex, _GradTex;
    float _Displacement, _Speed;
    float _Thick, _FadeAdd, _FadeMul;

    struct Input {
        float2 uv_NoiseTex;
        float2 uv_GradTex;
        float3 worldNormal;
        float3 viewDir;
        INTERNAL_DATA
    };

    void vert(inout appdata_full v, out Input o)
    {
        float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; 
        half4 dX = tex2Dlod(_DispTex, float4(worldPos.x, worldPos.y + _Time.y*_Speed, 0,0));
        UNITY_INITIALIZE_OUTPUT(Input, o);
        v.vertex.xyz += _Displacement * v.normal * dX.r;  
    }

    half _Glossiness;
    half _Metallic;
    fixed4 _Color0, _Color1;
    half3 _Emission;
    float3 _FresnelColor;
    float _FresnelExponent;



    void surf (Input IN, inout SurfaceOutputStandard o) {
        o.Metallic = _Metallic;
        o.Smoothness = _Glossiness;
      // Albedo comes from a texture tinted by color
        
        
        
        fixed4 noise = tex2D(_NoiseTex, IN.uv_NoiseTex);


        float stripe = tex2D(_GradTex, IN.uv_GradTex).g;
        float fade = saturate(IN.uv_NoiseTex.y*_FadeMul + _FadeAdd);

        float4 str = lerp(_Color0, _Color1, saturate(stripe+fade));


            float fresnel = dot(IN.worldNormal, IN.viewDir);
            fresnel = saturate(1 - fresnel);
            fresnel = pow(fresnel, _FresnelExponent);
            float3 fresnelColor = fresnel * _FresnelColor;
            o.Emission = str + fresnelColor;
    }
    ENDCG
  } 
  FallBack "Diffuse"
}