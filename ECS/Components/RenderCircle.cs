using System.Numerics;
using GramEngine.Core;
using System.Drawing;
using SFML.Graphics;
using SFML.System;

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
    public Vector2 Origin
    {
        get { return circleShape.Origin.ToSysNumVector(); }
        set { circleShape.Origin = value.ToSFMLVector(); }
    }

    public RenderCircle(float radius, bool centerOrigin = true)
    {
        this.circleShape = new CircleShape(radius);
        if (centerOrigin)
            circleShape.Origin = new Vector2f(radius, radius);
    }

    protected void SetTexture(string texturePath)
    {
        var texture = new Texture(texturePath);

        circleShape.Texture = texture;
    }
}