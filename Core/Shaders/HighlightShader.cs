using SFML.Graphics;

namespace GramEngine.Core.Shaders;

internal class HighlightShader
{
    internal Shader Shader;
    private const string memShader = 
    @"#version 120
uniform sampler2D texture;
uniform vec4 highlight_color;

float offset = 1.0/32;


void main()
{
  vec2 v_texCoords = gl_TexCoord[0].xy;
  vec4 col = texture2D(texture, v_texCoords);

  if (col.a == 1)
    gl_FragColor = gl_Color * col;
  else {
    float au = texture2D(texture, vec2(v_texCoords.x , v_texCoords.y - offset)).a;
    float ad = texture2D(texture, vec2(v_texCoords.x , v_texCoords.y + offset)).a;
    float al = texture2D(texture, vec2(v_texCoords.x - offset, v_texCoords.y)).a;
    float ar = texture2D(texture, vec2(v_texCoords.x + offset, v_texCoords.y)).a;

    if (au == 1.0 || ad == 1.0 || al == 1.0 || ar == 1.0)
      gl_FragColor = highlight_color;
    else
      gl_FragColor = col;
  }
}";

    internal HighlightShader()
    { 
        Shader = Shader.FromString(null, null, memShader);
    }
}