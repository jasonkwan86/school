Shader "Unlit/GradientMappingShader"
{
    Properties
    {
        _MainTex ("MainTexture", 2D) = "white" {}
        _MappedToTex ("MappedToTexture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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

            sampler2D _MainTex;
            sampler2D _MappedToTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                half gray = (col.r + col.g + col.b) / 3;

                // should be between 0-1
                // map intensity of 0 to left most texture and 1 to right most texture
                fixed4 finalCol = tex2D(_MappedToTex, gray);

                return finalCol;
            }
            ENDCG
        }
    }
}
