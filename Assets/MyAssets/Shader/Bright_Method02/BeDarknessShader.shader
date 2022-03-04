Shader "TecrayC/BeDarknessShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color   ("Color", Color) = (0.5, 0.5, 0.5, 1.0)
    }
    SubShader
    {
        Tags { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent"
            "IgnoreProjector" = "True" 
            "ForceNoShadowCasting" = "True"
        }

        Pass
        {
            ZWrite Off 
            
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;

            };



            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                

                return _Color;
            }
            ENDCG
        }
    }
}
