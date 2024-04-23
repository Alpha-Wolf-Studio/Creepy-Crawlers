Shader "Custom/PreviewEffect"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Color_Cutout ("Color Cutout", Float) = 0
        _Blink_Speed ("Blink Speed", Float) = 0
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment SpriteFragCustom
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

            float _Blink_Speed;
            float _Color_Cutout;
            int _Valid_ID;

            float inverseLerp(float from, float to, float value)
            {
                return (value - from) / (to - from);
            }

            fixed4 SpriteFragCustom(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
                c.rgb *= c.a;

                if(c.r > _Color_Cutout || c.g > _Color_Cutout || c.b > _Color_Cutout)
                {
                    c.r = 0;
                    c.g = 0;
                    c.b = 0;
                    c.a = 0;

                    if (_Valid_ID == 0)
                        c.g = inverseLerp(-1, 1, sin(_Time * _Blink_Speed));
                    else
                        c.r = inverseLerp(-1, 1, sin(_Time * _Blink_Speed));
                }

                return c;
            }

        ENDCG
        }
    }
}
