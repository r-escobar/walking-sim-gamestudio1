Shader "Custom/Noise"
{
    Properties
    {
        _Factor1 ("Factor 1", float) = 1
        _Factor2 ("Factor 2", float) = 1
        _Factor3 ("Factor 3", float) = 1
    }
 
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Fog { Mode Global}
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma multi_compile_fog
           
            #include "UnityCG.cginc"
           
            float _Factor1;
            float _Factor2;
            float _Factor3;

            float noise(half2 uv)
            {
                return frac(sin(dot(uv, float2(_Factor1, _Factor2))) * _Factor3 * _Time.y);
            }
// 
//            v2f vert (appdata v)
//            {
//                v2f o;
//                o.vertex = UnityObjectToClipPos(v.vertex);
//                UNITY_TRANSFER_FOG(o,o.vertex);
//                return o;
//            }
// 
            fixed4 frag (v2f_img i) : SV_Target
            {
                fixed4 col = noise(i.uv) * 0.8f;
                //UNITY_APPLY_FOG(i.fogCoord, col);
                
                return col;
            }
            ENDCG
        }
    }
}