Shader "Unlit/FullScreenShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("NoiseTexture", 2D) = "white" {}
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _DistortionPower ("Distortion Power", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            
            HLSLPROGRAM
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
            sampler2D _NoiseTex;
            
            float _DistortionPower;
            float4 _MainTex_ST;

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
            CBUFFER_END
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return half4(0, 1, 0, 1); // Green color
                // float2 shakeOffset = float2(
                //     tex2D(_NoiseTex, (i.uv + sin(_Time.y) + 1)/2).r * _DistortionPower,
                //     tex2D(_NoiseTex, (i.uv + cos(_Time.y) + 1)/2).g * _DistortionPower
                // );
                // float2 uv = i.uv + shakeOffset;
                // fixed4 col = tex2D(_MainTex, uv);

                
                // fixed4 col = _BaseColor;
                
                // fixed redDistortion = _BaseColor.r * (sin(_Time.y * 5) + 1) / 2;
                // fixed greenDistortion = _BaseColor.g * (sin(_Time.y / 2) + 1) / 2;
                // fixed blueDistortion = _BaseColor.b * (sin(_Time.y * 3) + 1) / 2;
                //
                // fixed4 noise = tex2D(_NoiseTex, uv + _Time);
                // fixed noisePower = (noise.r + noise.g + noise.b) / 3;
                //
                // col.r = noisePower * col.r * redDistortion;
                // col.g = noisePower * col.g * greenDistortion;
                // col.b = noisePower * col.b * blueDistortion;
                // col.a = _BaseColor.a;
                // return col;
            }
            
            // fixed4 frag (v2f i) : SV_Target
            // {
            //     float2 shakeOffset = float2(
            //         cos((_Time.y + i.uv.y) * 5),
            //         sin((_Time.x + i.uv.x) * 5)
            //     );
            //     // float2 uv = i.uv + shakeOffset;
            //     float2 uv = i.uv;
            //     fixed noiseRed = tex2D(_NoiseTex, uv + sin(_Time.y) / 10).r;
            //
            //     fixed4 lerpedUV = lerp(tex2D(_MainTex, uv), tex2D(_NoiseTex, uv), 0.5f);
            //     return lerpedUV;
            //     // fixed4 col = tex2D()
            //     // fixed4 col = _BaseColor * tex2D(_MainTex, uv + noiseRed);
            //     //
            //     // // fixed4 col = _BaseColor;
            //     // fixed4 noise = tex2D(_NoiseTex, uv + _Time);
            //     // fixed noisePower = (noise.r + noise.g + noise.b) / 3;
            //     //
            //     // fixed redDistortion = _BaseColor.r * (sin(_Time.y * 5) + 1) / 2;
            //     // fixed greenDistortion = _BaseColor.g * (sin(_Time.y / 2) + 1) / 2;
            //     // fixed blueDistortion = _BaseColor.b * (sin(_Time.y * 3) + 1) / 2;
            //     // col.r = noisePower * col.r * redDistortion;
            //     // col.g = noisePower * col.g * greenDistortion;
            //     // col.b = noisePower * col.b * blueDistortion;
            //     // col.a = _BaseColor.a;
            //     // return col;
            //
            //     return lerpedUV;
            // }
            ENDHLSL
        }
    }
}
