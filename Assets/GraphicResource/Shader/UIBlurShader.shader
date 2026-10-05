Shader "CustomRenderTexture/UIBlurShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _BlurTex ("Blur Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 10)) = 2
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _BlurTex;
            float4 _BlurTex_TexelSize;
            float _BlurSize;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 screenUV : TEXCOORD0;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                float4 screenPos = ComputeScreenPos(o.vertex);
                o.screenUV = screenPos.xy / screenPos.w;

                o.color = v.color;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.screenUV;

                float2 offset = _BlurTex_TexelSize.xy * _BlurSize;

                fixed4 color = 0;

                color += tex2D(_BlurTex, uv + float2(-offset.x, -offset.y));
                color += tex2D(_BlurTex, uv + float2( 0,         -offset.y));
                color += tex2D(_BlurTex, uv + float2( offset.x, -offset.y));

                color += tex2D(_BlurTex, uv + float2(-offset.x, 0));
                color += tex2D(_BlurTex, uv);
                color += tex2D(_BlurTex, uv + float2(offset.x, 0));

                color += tex2D(_BlurTex, uv + float2(-offset.x, offset.y));
                color += tex2D(_BlurTex, uv + float2( 0,         offset.y));
                color += tex2D(_BlurTex, uv + float2( offset.x, offset.y));

                color /= 9.0;

                color.a *= i.color.a;

                return color;
            }

            ENDHLSL
        }
    }
}
