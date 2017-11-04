Shader "ShaderEditor/EditorShaderCache"
{
	Properties 
	{
_BaseTex("_BaseTex", 2D) = "black" {}
_MaskTex("_MaskTex", 2D) = "black" {}
_NormalTex("_NormalTex", 2D) = "black" {}

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Geometry"
"IgnoreProjector"="False"
"RenderType"="Opaque"

		}

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf Standard  vertex:vert
#pragma target 2.0


sampler2D _BaseTex;
sampler2D _MaskTex;
sampler2D _NormalTex;

			
			inline half4 LightingBlinnPhongEditor_PrePass (SurfaceOutputStandard s, half4 light)
			{
half4 c;
c.rgb = (s.Albedo.rgb * light.rgb) * s.Alpha;
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (SurfaceOutputStandard s, half3 lightDir, half atten)
			{
				half NdotL = dot (s.Normal, lightDir);
				half4 c;
				c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten);
				c.a = s.Alpha;
				return LightingBlinnPhongEditor_PrePass( s, c );
			}
			
			struct Input {
				float2 uv_BaseTex;
float2 uv_NormalTex;
float2 uv_MaskTex;

			};

			void vert (inout appdata_full v, out Input o) {
				UNITY_INITIALIZE_OUTPUT(Input,o)
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


			}
			
			void surf (Input IN, inout SurfaceOutputStandard o) {
float4 Sampled2D0=tex2D(_BaseTex,IN.uv_BaseTex.xy);
float4 Sampled2D2=tex2D(_NormalTex,IN.uv_NormalTex.xy);
float4 Sampled2D1=tex2D(_MaskTex,IN.uv_MaskTex.xy);
float4 Split0=Sampled2D1;
float4 Master0_2_NoInput = float4(0,0,0,0);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_8_NoInput = float4(1,1,1,1);
float4 Master0_30_NoInput = float4(0,0,0,0);
float4 Master0_20_NoInput = float4(1,1,1,1);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Sampled2D0;
o.Normal = Sampled2D2;
o.Smoothness = float4( Split0.x, Split0.x, Split0.x, Split0.x);

			}
		ENDCG
	}
	Fallback "Diffuse"
}
