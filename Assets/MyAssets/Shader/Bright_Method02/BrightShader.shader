Shader "TecrayShader/BrightShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightCol("LightCol", Color) = (1.0, 1.0, 1.0, 1.0)     // 假光颜色
        _LightInt("LightInt", Float) = 1.0                      // 假光强度
        _LightPow("_LightPow", Float) = 1.0                     // 控制假光发散程度
    }
    SubShader
    {
        Tags{
            "RenderType" = "Transparent"
            "Queue" = "Transparent" 
            "IgnoreProjector" = "True"           //shader不会受到投影器(Projectors)的影响
            "ForceNoShadowCasting" = "True"      //关闭阴影投射
        }
        
        GrabPass {          // 抓取屏幕纹理
                "_BGTex"
            }
        
        // No culling or depth
        Cull Off ZWrite Off ZTest Always        // 双面显示，深度写入关闭
        Pass
        {
            Name "FORWARD"
            Tags{
                    "LightMode" = "ForwardBase"
                
            } 
            
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            float4 _LightCol;
            float _LightInt;
            float _LightPow;
            sampler2D  _BGTex;

            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv0 : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 grabPos : TEXCOORD1;     // 背景纹理坐标
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv0 = v.uv0;
                o.grabPos = ComputeGrabScreenPos(o.pos);    // 背景纹理采样坐标
                return o;
            }

                // 中心向外扩散的渐变圆
                float makePointLightFun(float2 uv)
            {
                uv -= 0.5;          // uv起点偏移到中心
                return (1-dot(uv, uv));     
            }
            
            half4 frag (v2f i) : SV_Target
            {
                half4 var_BGTex = tex2Dproj(_BGTex, i.grabPos);         // 采样背景纹理
                half pointlight = pow(makePointLightFun(i.uv0), _LightPow);     // 得到半径可控的亮圆
                half3 fianlCol = _LightCol.rgb * var_BGTex.rgb * pointlight * _LightInt;    //加光的颜色
                half  opacity = pointlight;     // 不透明度
                // return 0;
                return half4(fianlCol, opacity);
                
            }
            ENDCG
        }
    }
}
