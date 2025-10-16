Shader "Custom/RandomSharpWaveLinesMoving"
{
    Properties
    {
        _MainColor("Main Color", Color) = (1,1,1,1)
        _LineColor("Line Color", Color) = (0,0,0,1)
        _MinSpacing("Min Spacing", Float) = 0.05
        _MaxSpacing("Max Spacing", Float) = 0.2
        _WaveAmount("Wave Amount", Float) = 0
        _LineWidth("Line Width", Float) = 0.01
        _LineCount("Line Count", Int) = 10
        _ScrollSpeed("Scroll Speed", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _MainColor;
            float4 _LineColor;
            float _MinSpacing;
            float _MaxSpacing;
            float _WaveAmount;
            float _LineWidth;
            int _LineCount;
            float _ScrollSpeed;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            float hash(float x)
            {
                return frac(sin(x)*43758.5453);
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float y = fmod(IN.uv.y + _Time.y * _ScrollSpeed, 1.0);
                float x = IN.uv.x;
                float mask = 0;
            
                for(int i=0; i < _LineCount; i++)
                {
                    float seed = i * 12.9898 + 78.233;
                    float linePos = hash(seed) * (_MaxSpacing - _MinSpacing) + _MinSpacing + i * (_MaxSpacing + _MinSpacing);
                    linePos = fmod(linePos,1.0); // sürekli tekrar
            
                    float waveFreq = lerp(5, 20, hash(seed+2)); 
                    float waveAmp = _WaveAmount * hash(seed+3); 
                    float wave = sin(x * waveFreq + seed) * waveAmp;
            
                    float dist = abs(y - (linePos + wave));
                    mask = max(mask, step(dist, _LineWidth));
                }
            
                return lerp(_MainColor, _LineColor, mask);
            }
            
            ENDHLSL
        }
    }
}
