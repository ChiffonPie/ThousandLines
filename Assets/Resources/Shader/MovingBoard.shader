Shader "Custom/MovingBoard" {
    Properties{
        _Color("Main Color", Color) = (1,1,1,1)
        _MainTex("Base (RGB)", 2D) = "white" {}
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 200

        CGPROGRAM
        #pragma surface surf NoLighting alpha

        sampler2D _MainTex;
        fixed4 _Color;
        struct Input {
            float2 uv_MainTex;
        };

        fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten) {
            return fixed4(s.Albedo, s.Alpha);
        }

        void surf(Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, float2(IN.uv_MainTex.x, IN.uv_MainTex.y)) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = _Color.a;
        }
        ENDCG
        }

            Fallback "Legacy Shaders/VertexLit"
}