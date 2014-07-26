Shader "Custom/SelfIllumDiffuseUV2" {
	Properties {
	    _Color ("Main Color", Color) = (1,1,1,1)
	    _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	    _Illum ("Illumin (A)", 2D) = "white" {}
	    _BumpMap ("Normalmap", 2D) = "bump" {}
	    _EmisColor ("Main Emission Color", Color) = (1,1,1,1)
	    _EmissionLM ("Emission (Lightmapper)", Float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 300
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _Illum;
		fixed4 _Color;
		fixed4 _EmisColor;

		struct Input {
			float2 uv_MainTex;
			float2 uv2_Illum;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	   		fixed4 c = tex * _Color;
	   		o.Albedo = c.rgb;
	    	o.Emission = c.rgb * UNITY_SAMPLE_1CHANNEL(_Illum, IN.uv2_Illum) * _EmisColor;
	    	o.Alpha = c.a;
	    	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv2_Illum));
		}
		ENDCG
	} 
	FallBack "Legacy Shaders/Lightmapped/Diffuse"
}
