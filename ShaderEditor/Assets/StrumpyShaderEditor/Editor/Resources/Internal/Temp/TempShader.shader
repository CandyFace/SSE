Shader "ShaderEditor/EditorShaderCache"
{
	Properties 
	{
_Color_1("_Color_1", Color) = (0.2980392,0.5019608,0,1)
_Color_2("_Color_2", Color) = (0.7019608,0.7019608,0.7019608,1)
_Metallic("_Metallic", Float) = 0

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


float4 _Color_1;
float4 _Color_2;
float _Metallic;

			
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
				float4 color : COLOR;

			};

			void vert (inout appdata_full v, out Input o) {
				UNITY_INITIALIZE_OUTPUT(Input,o)
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


			}
			
			void surf (Input IN, inout SurfaceOutputStandard o) {
				o.Smoothness = 0.5;
float4 Add0=_Color_1 + _Color_2;
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_2_NoInput = float4(0,0,0,0);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_9_NoInput = float4(0,0,0,0);
float4 Master0_10_NoInput = float4(0,0,0,0);
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Add0;
o.Metallic = _Metallic.xxxx;

			}
		ENDCG
	}
	Fallback "Diffuse"
}
