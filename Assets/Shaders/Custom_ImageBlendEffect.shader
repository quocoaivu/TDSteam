Shader "Custom/ImageBlendEffect"
{
  Properties
  {
    _MainTex ("Base", 2D) = "" {}
    _BlendTex ("Image", 2D) = "" {}
    _BumpMap ("Normalmap", 2D) = "bump" {}
  }
  SubShader
  {
    Tags
    { 
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
      }
      ZTest Always
      ZWrite Off
      Cull Off
      Fog
      { 
        Mode  Off
      } 
      GpuProgramID 15997
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      #define conv_mxt3x3_0(mat4x4) float3(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x)
      #define conv_mxt3x3_1(mat4x4) float3(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y)
      #define conv_mxt3x3_2(mat4x4) float3(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z)
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform sampler2D _MainTex;
      uniform sampler2D _BlendTex;
      uniform sampler2D _BumpMap;
      uniform float _BlendAmount;
      uniform float _EdgeSharpness;
      uniform float _SeeThroughness;
      uniform float _Distortion;
      struct IN_Data_Vert
      {
          float4 in_POSITION :POSITION;
          float4 in_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      OUT_Data_Vert vert(IN_Data_Vert in_v)
      {
          OUT_Data_Vert out_v;
          float2 tmpvar_1;
          tmpvar_1 = in_v.in_TEXCOORD0.xy;
          float2 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.in_POSITION.xyz;
          tmpvar_2 = tmpvar_1;
          out_v.gl_Position = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          out_v.xlv_TEXCOORD0 = tmpvar_2;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float4 overlayColor_2;
          float4 mainColor_3;
          float2 bump_4;
          float4 blendColor_5;
          float4 tmpvar_6;
          tmpvar_6 = tex2D(_BlendTex, in_f.xlv_TEXCOORD0);
          blendColor_5 = tmpvar_6;
          blendColor_5.w = (blendColor_5.w + ((_BlendAmount * 2) - 1));
          blendColor_5.w = clamp(((blendColor_5.w * _EdgeSharpness) - ((_EdgeSharpness - 1) * 0.5)), 0, 1);
          float2 tmpvar_7;
          tmpvar_7 = ((tex2D(_BumpMap, in_f.xlv_TEXCOORD0).xyz * 2) - 1).xy;
          bump_4 = tmpvar_7;
          float4 tmpvar_8;
          float2 P_9;
          P_9 = (in_f.xlv_TEXCOORD0 + ((bump_4 * blendColor_5.w) * _Distortion));
          tmpvar_8 = tex2D(_MainTex, P_9);
          mainColor_3 = tmpvar_8;
          overlayColor_2.w = blendColor_5.w;
          overlayColor_2.xyz = ((mainColor_3.xyz * (blendColor_5.xyz + 0.5)) * (blendColor_5.xyz + 0.5));
          float4 tmpvar_10;
          tmpvar_10 = lerp(blendColor_5, overlayColor_2, float4(_SeeThroughness, _SeeThroughness, _SeeThroughness, _SeeThroughness));
          blendColor_5 = tmpvar_10;
          float4 tmpvar_11;
          tmpvar_11 = lerp(mainColor_3, tmpvar_10, tmpvar_10.wwww);
          tmpvar_1 = tmpvar_11;
          out_f.SV_Target0 = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack ""
}
