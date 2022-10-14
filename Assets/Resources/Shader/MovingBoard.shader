Shader "Custom/MovingBoard" {
    Properties{
        _Color("Main Color", Color) = (1,1,1,1)
        _MainTex("Base (RGB)", 2D) = "white" {}
        _HorizontalSpeed("Horizontal Speed", float) = 0
        _VerticalSpeed("Vertical Speed", float) = 0
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        fixed4 _Color;
        float _HorizontalSpeed;
        float _VerticalSpeed;
        struct Input {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, float2(IN.uv_MainTex.x, IN.uv_MainTex.y)) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
        }

            Fallback "Legacy Shaders/VertexLit"
}