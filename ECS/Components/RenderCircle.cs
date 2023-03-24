using System.Numerics;
using GramEngine.Core;
using System.Drawing;
using SFML.Graphics;

namespace GramEngine.ECS.Components;

public class RenderCircle : Component
{
    internal CircleShape circleShape;

    public System.Drawing.Color FillColor
    {
        get => circleShape.FillColor.ToSysColor();
        set => circleShape.FillColor = value.ToSFMLColor();
    }

    public System.Drawing.Color OutlineColor
    {
        get => circleShape.OutlineColor.ToSysColor();
        set => circleShape.OutlineColor = value.ToSFMLColor();
    }
    
    public float BorderThickness
    {
        get => circleShape.OutlineThickness;
        set => circleShape.OutlineThickness = value;
    }

    public float Radius
    {
        get => circleShape.Radius;
        set => circleShape.Radius = value;
    }

    public RenderCircle(float radius)
    {
        this.circleShape = new CircleShape(radius);
    }

    protected void SetTexture(string texturePath)
    {
        var texture = new Texture(texturePath);

        circleShape.Texture = texture;
    }
}