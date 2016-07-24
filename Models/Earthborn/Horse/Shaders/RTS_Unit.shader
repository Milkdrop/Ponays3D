Shader "Custom/RTS_Unit"
{
	Properties
	{
		_mask1("Mask 1 Color", Color) = (1,1,1,1)	// For team color
		_mask2("Mask 2 Color", Color) = (1,1,1,1)	// For detail color
		_highlightColor("highlightColor", Color) = (1,1,1,1) // For selection

		_colorTex("Diffuse Map", 2D) = "white" {}
		_mainNormal("Normal Map", 2D) = "white" {}

		// Red channel in mainTex is AO, Green is first color mask, Blue is second color mask
		_mainTex("Masks", 2D) = "white" {}
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
		sampler2D _colorTex;
		sampler2D _mainNormal;
		fixed4 _mask1;
		fixed4 _mask2;
		fixed4 _highlightColor;

		struct Input
		{
			float2 uv_mainTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			fixed3 tex = tex2D(_mainTex, IN.uv_mainTex);
			fixed colorMask = 1 - min(tex.g + tex.b, 1);
			fixed3 color = tex2D(_colorTex, IN.uv_mainTex);

			o.Albedo = tex.r * (_mask1 * tex.g + _mask2 * tex.b + color * colorMask);
			o.Emission = _highlightColor;
			o.Normal = UnpackNormal(tex2D(_mainNormal, IN.uv_mainTex));
		}
		ENDCG
	}
	FallBack "Diffuse"
}
