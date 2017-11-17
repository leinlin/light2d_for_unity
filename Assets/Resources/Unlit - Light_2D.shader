Shader "Unlit/Light_2D"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
	}
	
	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite On
			Fog { Mode Off }
			Offset -1, -1
			//Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
	
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
			};
	
			v2f o;

            float circleSDF(float x, float y, float cx, float cy, float r) {
                float ux = x - cx, uy = y - cy;
                return sqrt(ux * ux + uy * uy) - r;
            }

            float trace(float ox, float oy, float dx, float dy) {
                int MAX_STEP = 10;
                float MAX_DISTANCE = 2.0f;
                float EPSILON = 1e-6f;

                float t = 0.0f;
                for (int i = 0; i < MAX_STEP && t < MAX_DISTANCE; i++) {
                    float cx = sin(_Time.z) * 0.4 + 0.5;
                    float cy = cos(_Time.z) * 0.4 + 0.5;
                    float sd = circleSDF(ox + dx * t, oy + dy * t, cx, cy, 0.1f);
                    if (sd < EPSILON)
                        return 2.0f;
                    t += sd;
                }
                return 0.0f;
            }

            float rand(float x) {
                float TWO_PI = 6.28318530718f;
                x = x * 2048 * TWO_PI + 5221;//加个不均匀的数值，防止坐落在周期之中
                float val = cos(x);
                val = val * val;
                return val;
            }

            float sample(float x, float y) {
                float TWO_PI = 6.28318530718f;
                int N = 256;
                float sum = 0.0f;

                for (int i = 0; i < N; i++) {
                    float r = rand(x);
                    float a = TWO_PI * (i + r) / N;
                    sum += trace(x, y, cos(a), sin(a));
                }
                return sum / N;
            }

			v2f vert (appdata_t v)
			{
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
                o.uv = v.texcoord;

				return o;
			}
				
			fixed4 frag (v2f IN) : COLOR
			{
                float2 pos = IN.uv;
                float t = sin(_Time.y);
				return (0.5 + 1.5* t*t) * sample(pos.x, pos.y);
			}
			ENDCG
		}
	}

	//备胎设为Unity自带的普通漫反射  
    Fallback" Diffuse "  
}
