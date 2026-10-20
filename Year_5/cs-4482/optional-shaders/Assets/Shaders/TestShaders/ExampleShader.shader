Shader "Fullscreen/Example"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline"}
        Pass
        {
            Name "FullscreenExample"
            ZWrite Off
            Cull Off
            Blend Off // Or specify a blend mode if needed
            
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            float4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float2 uv = UnityStereoTransformScreenSpaceTex(input.texcoord);
                half4 color = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_PointClamp, uv);
                
                //Simple color invert. Replace with your own logic.
                color.rgb = 1 - color.rgb;
                return color;
            }
            ENDHLSL
        }
    }
}