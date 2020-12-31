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
				fixed2 offset = (i.uv - 0.5) * 0.0125;
				fixed2 offset2 = offset * 2;
                fixed3 col = tex2D(_MainTex, i.uv - offset2).rgb
					+ tex2D(_MainTex, i.uv - offset).rgb * 4
					+ tex2D(_MainTex, i.uv).rgb * 7
					+ tex2D(_MainTex, i.uv - offset).rgb * 4
					+ tex2D(_MainTex, i.uv - offset2).rgb;
                // just invert the colors
				col = col / 17.f;
				//fixed4 col = fixed4((i.uv - 0.5) * 0.1, 0, 1);
                return fixed4(col, 1);
            }
            ENDCG
        }
    }
}
