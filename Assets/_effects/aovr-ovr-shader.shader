Shader "Custom/aovr-ovr-shader" {
Properties
{
   _Color ("Main Color", Color) = (1,1,1,1)
   _GlowMap ("Glow (A)", 2D) = "bump" {}
   }
   Category
{
   Tags {"Queue" = "Transparent" }
      SubShader
   {
            Pass
      {                 
        ZWrite Off
              Offset -1, -1
              Cull Back
        Colormask A
        SetTexture [_GlowMap]
        {
           constantColor [_Color]
           combine constant, texture * constant
        }
 
      }
   }
}
}