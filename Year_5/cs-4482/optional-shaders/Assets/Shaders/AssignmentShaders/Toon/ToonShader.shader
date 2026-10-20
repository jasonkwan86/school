Shader "Unlit/ToonShader"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _Intensity1 ("Intensity 1", Range(0.0, 1.0)) = 0.75
        _Intensity2 ("Intensity 2", Range(0.0, 1.0)) = 0.5
        _Intensity3 ("Intensity 3", Range(0.0, 1.0)) = 0.25
        [MaterialToggle] _UseOptionalTexture ("Use Optional Texture", Float) = 0
        _OptionalTex ("OptionalTexture", 2D) = "" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float _Intensity1;
            float _Intensity2;
            float _Intensity3;
            float _UseOptionalTexture;
            sampler2D _OptionalTex;

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
            CBUFFER_END
            
            struct Attributes
            {
                float4 positionOS   : POSITION;
                // Declaring the variable containing the normal vector for each
                // vertex.
                half3 normal        : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                half3 normal        : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                // Use the TransformObjectToWorldNormal function to transform the
                // normals from object to world space. This function is from the
                // SpaceTransforms.hlsl file, which is referenced in Core.hlsl.
                
                OUT.normal = TransformObjectToWorldNormal(IN.normal);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Use left most optional texture as color if it's checked true
                // Otherwise, use base color
                half4 color = _UseOptionalTexture == 1 ? tex2D(_OptionalTex, 0) : _BaseColor;

                // value from 0-1
                half power = saturate(dot(IN.normal, GetMainLight().direction));

                if (power > _Intensity1)
                {
                    power = _Intensity1;
                }
                else if (power > _Intensity2)
                {
                    power = _Intensity2;
                }
                else if (power > _Intensity3)
                {
                    power = _Intensity3;
                }
                else
                {
                    power = 0;
                }
                    
                color.rgb = color * power;
                return color;
            }
            ENDHLSL
        }
    }
}
