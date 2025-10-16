Shader "OutlineAndOverlay/Custom"
{
    Properties
    {
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineSize("Outline Size", Range(0,20)) = 1
        _OverlayAlpha("Overlay Strength", Range(0,1)) = 0.3
        _OverlayHeight("Overlay Height", Range(0,1)) = 0.2
    }

    SubShader
    {
        Tags{"RenderType"="Transparent" "Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Name "InnerOutlineWithOverlay_Colored"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_TexelSize;
            float4 _OutlineColor;
            float _OutlineSize;
            float _OverlayAlpha;
            float _OverlayHeight;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.color = IN.color; // SpriteRenderer.color buraya geliyor
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv) * IN.color;

                if (col.a <= 0.001) discard;

                float minA = 1;
                int range = ceil(_OutlineSize * 2);
                for (int x = -range; x <= range; x++)
                    for (int y = -range; y <= range; y++)
                        minA = min(minA, SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(x, y) * _MainTex_TexelSize.xy).a);

                if (minA < col.a)
                    return _OutlineColor;

                float overlayStartY = 0;
                const int steps = 256;
                for (int i = 0; i < steps; i++)
                {
                    float y = i / (float)steps;
                    float a = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x, y)).a;
                    if (a > 0.99)
                    {
                        overlayStartY = y;
                        break;
                    }
                }

                if (uv.y < overlayStartY + _OverlayHeight)
                    col.rgb *= 1.0 - _OverlayAlpha;

                return col;
            }
            ENDHLSL
        }
    }
}
