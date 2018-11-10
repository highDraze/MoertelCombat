// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/Flare" {
Properties {
    _MainColor("Color", color) = (0,0,0,0)
}
SubShader {
    Tags {
        "Queue"="Transparent"
        "IgnoreProjector"="True"
        "RenderType"="Transparent"
        "PreviewType"="Plane"
    }
    Cull Off Lighting Off ZWrite Off Ztest Always
    Blend One One

    Pass {
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma target 2.0

        #include "UnityCG.cginc"

        fixed4 _MainColor;
        fixed4 _TintColor;

        struct appdata_t {
            float4 vertex : POSITION;
            fixed4 color : COLOR;
            float2 texcoord : TEXCOORD0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct v2f {
            float4 vertex : SV_POSITION;
            fixed4 color : COLOR;
            float2 texcoord : TEXCOORD0;
            UNITY_VERTEX_OUTPUT_STEREO
        };

        float4 _MainTex_ST;

        v2f vert (appdata_t v)
        {
            v2f o;
            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.color = v.color;
            return o;
        }

        fixed4 frag (v2f i) : SV_Target
        {
            fixed4 col;
            fixed4 tex = _MainColor;
            col.rgb = i.color.rgb * tex.rgb;
            col.a = tex.a;
            return col;
        }
        ENDCG
    }
}

}
