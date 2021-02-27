Shader "Hidden/Lens"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
				fixed2 offset1 = (i.uv - 0.5) * 0.0125;
				fixed2 offset05 = offset1 * 0.5f;
				fixed2 offset15 = offset1 * 1.5f;
				fixed2 offset2 = offset1 * 2;

				fixed3 col = tex2D(_MainTex, i.uv + offset2).rgb * float3(0, 0.2f, 0.5f)
					+ tex2D(_MainTex, i.uv + offset15).rgb * float3(0, 0.2f, 0.3f)
					+ tex2D(_MainTex, i.uv + offset1).rgb * float3(0, 0.2f, 0.25f)
					+ tex2D(_MainTex, i.uv + offset05).rgb * float3(0, 0.2f, 0.2f)
					+ tex2D(_MainTex, i.uv).rgb * float3(0.25f, 0.2f, 0.25f)
					+ tex2D(_MainTex, i.uv - offset05).rgb * float3(0.2f, 0.2f, 0)
					+ tex2D(_MainTex, i.uv - offset1).rgb * float3(0.25f, 0.2f, 0)
					+ tex2D(_MainTex, i.uv - offset15).rgb * float3(0.3f, 0.2f, 0)
					+ tex2D(_MainTex, i.uv - offset2).rgb * float3(0.5f, 0.2f, 0);
				col = col / 1.4f;

				//fixed3 col = tex2D(_MainTex, i.uv + offset1).rgb * float3(0, 0, 1.f)
				//	+ tex2D(_MainTex, i.uv).rgb * float3(0, 1.f, 0)
				//	+ tex2D(_MainTex, i.uv - offset1).rgb * float3(1.f, 0, 0);
				//col = col / 1.4f;


				//fixed4 col = fixed4((i.uv - 0.5) * 0.1, 0, 1);
                return fixed4(col, 1);
            }
            ENDCG
        }
    }
}
