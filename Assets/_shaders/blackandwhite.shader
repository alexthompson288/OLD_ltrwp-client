Shader "BlackAndWhiteUnlit"
{
	Properties 
	{
_Texture("_Texture", 2D) = "black" {}
_saturation("_saturation", Float) = 0

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Transparent"
"IgnoreProjector"="False"
"RenderType"="Transparent"

		}

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 2.0


sampler2D _Texture;
float _saturation;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Custom;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot ( lightDir, s.Normal ));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff;
				res.w = spec * Luminance (_LightColor0.rgb);
				res *= atten * 2.0;

				return LightingBlinnPhongEditor_PrePass( s, res );
			}
			
			struct Input {
				float2 uv_Texture;

			};

			void vert (inout appdata_full v, out Input o) {
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Sampled2D0=tex2D(_Texture,IN.uv_Texture.xy);
float4 Multiply1=Sampled2D0 * _saturation.xxxx;
float4 Split0=Sampled2D0;
float4 Add0=float4( Split0.x, Split0.x, Split0.x, Split0.x) + float4( Split0.y, Split0.y, Split0.y, Split0.y);
float4 Add1=Add0 + float4( Split0.z, Split0.z, Split0.z, Split0.z);
float4 Multiply0=float4( 0.3333,0.3333,0.3333,0.3333 ) * Add1;
float4 Assemble0_3_NoInput = float4(0,0,0,0);
float4 Assemble0=float4(Multiply0.x, Multiply0.y, Multiply0.z, Assemble0_3_NoInput.w);
float4 Subtract0=float4( 1,1,1,1 ) - _saturation.xxxx;
float4 Multiply2=Assemble0 * Subtract0;
float4 Add2=Multiply1 + Multiply2;
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Add2;
o.Emission = Add2;
o.Alpha = Sampled2D0.aaaa;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}