Shader "Sprite-Unlit-Gradient"
{
	Properties
	{
		[HideInInspector] [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		[HideInInspector] _Color ("Tint", Color) = (1,1,1,1)
		
		[HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
		[HideInInspector] _Stencil ("Stencil ID", Float) = 0
		[HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
		[HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
		[HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255

		[HideInInspector] _ColorMask ("Color Mask", Float) = 15

		[HideInInspector] [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		//_MainTex("_MainTex", 2D) = "white" {}
		
		_Color0("Color 0", Color) = (1,0,0,1)
		_Color1("Color 1", Color) = (0.145825,1,0,0)
		_Color2("Color 2", Color) = (0,0.06572652,1,1)
		
		[Space]
		
		[MaterialToggle] _radial("Radial", Range( 0 , 1)) = 0
		_rotate("Rotate", Range( 0 , 1)) = 0
		_center("Center", Range( 0.000001 , 0.999)) = 0.5
		
		[Space]
		
		[HideInInspector] _ColBlend("Color Blend", Range( 0 , 1)) = 1
		_AlphaBlend("Alpha Blend", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		
		Stencil
		{
			Ref [_Stencil]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			Comp [_StencilComp]
			Pass [_StencilOp]
		}


		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		
		Pass
		{
			Name "Default"
		CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float4 _Color1;
			uniform float4 _Color2;
			uniform float _rotate;
			uniform float _radial;
			uniform float _center;
			uniform float4 _Color0;
			uniform float _ColBlend;
			uniform float _AlphaBlend;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.worldPosition = IN.vertex;
				
				
				OUT.worldPosition.xyz +=  float3( 0, 0, 0 ) ;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_MainTex = IN.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode6 = tex2D( _MainTex, uv_MainTex );
				float2 texCoord19 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float cos20 = cos( ( ( _rotate * 2.0 ) * UNITY_PI ) );
				float sin20 = sin( ( ( _rotate * 2.0 ) * UNITY_PI ) );
				float2 rotator20 = mul( texCoord19 - float2( 0.5,0.5 ) , float2x2( cos20 , -sin20 , sin20 , cos20 )) + float2( 0.5,0.5 );
				float lerpResult17 = lerp( (rotator20).x , length( (float2( -1,-1 ) + (rotator20 - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 ))) ) , saturate( ceil( _radial ) ));
				float temp_output_47_0 = saturate( _center );
				float4 lerpResult27 = lerp( _Color1 , _Color2 , (0.0 + (lerpResult17 - temp_output_47_0) * (1.0 - 0.0) / (1.0 - temp_output_47_0)));
				float4 lerpResult26 = lerp( _Color0 , _Color1 , (0.0 + (lerpResult17 - 0.0) * (1.0 - 0.0) / (temp_output_47_0 - 0.0)));
				float4 lerpResult28 = lerp( lerpResult27 , lerpResult26 , step( lerpResult17 , temp_output_47_0 ));
				float3 lerpResult39 = lerp( (tex2DNode6).rgb , (lerpResult28).rgb , saturate( _ColBlend ));
				float lerpResult44 = lerp( (tex2DNode6).a , (lerpResult28).a , saturate( _AlphaBlend ));
				float4 appendResult13 = (float4(lerpResult39 , lerpResult44));
				
				half4 color = appendResult13;
				
				#ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	
	Fallback Off
}