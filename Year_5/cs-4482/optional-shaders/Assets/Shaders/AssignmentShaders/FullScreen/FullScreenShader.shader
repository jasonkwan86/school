// Template taken from URPs built-in StopNaNs shader
Shader "Fullscreen"
{
    Properties
    {
        _NoiseTex ("NoiseTexture", 2D) = "white" {}
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _DistortionPower ("Distortion Power", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        Pass
        {
            Name "Fullscreen"
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            Blend Off
            
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            sampler2D _NoiseTex;
            
            float _DistortionPower;

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
            CBUFFER_END
            
            // REMEMBER FULL SCREEN SHADERS HAVE NO VERTEX SHADERS
            
            float4 Frag(Varyings input) : SV_Target
            {
                float2 shakeOffset = float2(
                    tex2D(_NoiseTex, (input.texcoord + sin(_Time.y) + 1)/2).r * _DistortionPower,
                    tex2D(_NoiseTex, (input.texcoord + cos(_Time.y) + 1)/2).g * _DistortionPower
                );
                
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float2 uv = UnityStereoTransformScreenSpaceTex(input.texcoord);
                half4 col = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_PointClamp, uv + shakeOffset);
                col.r = col.r * (sin(_Time.y) + 1) / 2;
                col.g = col.g * (cos(_Time.y + 3) + 1) / 2;
                col.b = col.b * (sin(_Time.y + 1) + 1) / 2;
                
                return col;
            }
            ENDHLSL
        }
    }
}