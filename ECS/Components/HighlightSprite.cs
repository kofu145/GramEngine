using System.Drawing;

namespace GramEngine.ECS.Components;

public class HighlightSprite : Component
{
    public Color Color;
    public HighlightSprite(Color color)
    {
        Color = color;
    }
}