﻿Shader "Custom/Wall"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Main Tint",Color) = (1,1,1,1)
            //透明度参考值
            _AlphaScale("Alpha Cutoff",Range(0,1)) = 1
    }
        SubShader
        {
            Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

            Pass
            {
                Tags {"LightMode" = "ForwardBase"}

                ZWrite Off
                Blend SrcAlpha OneMinusSrcAlpha

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"
                #include "Lighting.cginc"

                fixed4 _Color;
                sampler2D _MainTex;
                float4 _MainTex_ST;
                fixed _AlphaScale;

                struct a2v
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float3 worldNormal : TEXCOORD0;
                    float3 worldPos : TEXCOORD1;
                    float2 uv : TEXCOORD2;
                };

                v2f vert(a2v v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.worldNormal = UnityObjectToWorldNormal(v.normal);
                    o.worldPos = mul(unity_ObjectToWorld,v.vertex).xyz;
                    o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed3 worldNormal = normalize(i.worldNormal);
                    fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
                    fixed4 texColor = tex2D(_MainTex,i.uv);

                    //实现光照
                    //吸收系数
                    fixed3 albedo = texColor.rgb * _Color.rgb;
                    //环境光
                    fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
                    //漫反射
                    fixed3 diffuse = _LightColor0.rgb * albedo * max(0,dot(worldNormal,worldLightDir));
                    return fixed4(ambient + diffuse,texColor.a * _AlphaScale);
               }
               ENDCG
           }
        }
            Fallback "Transparent/Cutout/VertexLit"
}
