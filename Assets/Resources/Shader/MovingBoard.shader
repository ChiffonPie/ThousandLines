Shader "Custom/MovingBoard" {
    Properties{
        _MainTex("Base (RGB)", 2D) = "white" {}
        _HorizontalSpeed("Horizontal Speed", float) = 0
        _VerticalSpeed("Vertical Speed", float) = 0
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 200

        CGPROGRAM
        #pragma surface surf NoLighting noambient

        sampler2D _MainTex;
        fixed4 _Color;
        float _HorizontalSpeed;
        float _VerticalSpeed;
        struct Input {
            float2 uv_MainTex;
        };

        fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten) {
            return fixed4(s.Albedo, s.Alpha);
        }

        void surf(Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, float2(IN.uv_MainTex.x, IN.uv_MainTex.y));
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
        }

            Fallback "Legacy Shaders/VertexLit"
}