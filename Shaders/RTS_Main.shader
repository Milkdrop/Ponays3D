Shader "Custom/RTS_Main"
{
	Properties
	{
		_teamColor("Team Color", Color) = (1,1,1,1)	// For team color
		_highlightColor("Highlight Color", Color) = (1,1,1,1) // For selection

		_mainTex("Diffuse Map", 2D) = "white" {}
		_aoTex("Ambient Occlusion", 2D) = "white" {}
		_teamColorMask("Team Color Mask", 2D) = "black" {}
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _mainTex;
		sampler2D _aoTex;
		sampler2D _teamColorMask;
		fixed4 _teamColor;
		fixed4 _highlightColor;

		struct Input
		{
			float2 uv_mainTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			fixed teamColorMask = tex2D(_teamColorMask, IN.uv_mainTex);
			fixed colorMask = 1 - teamColorMask;
			fixed3 color = tex2D(_mainTex, IN.uv_mainTex);

			o.Albedo = (_teamColor * teamColorMask + color * colorMask) * tex2D(_aoTex, IN.uv_mainTex);
			o.Emission = _highlightColor;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
