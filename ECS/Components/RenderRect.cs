using SFML.Graphics;
using System.Numerics;
using GramEngine.Core;
using Color = System.Drawing.Color;

namespace GramEngine.ECS.Components;

public class RenderRect : Component
{
    internal RectangleShape rectangleShape;

    public Color FillColor
    {
        get => rectangleShape.FillColor.ToSysColor();
        set => rectangleShape.FillColor = value.ToSFMLColor();
    }

    public Color OutlineColor
    {
        get => rectangleShape.OutlineColor.ToSysColor();
        set => rectangleShape.OutlineColor = value.ToSFMLColor();
    }
    
    public float BorderThickness
    {
        get => rectangleShape.OutlineThickness;
        set => rectangleShape.OutlineThickness = value;
    }

    public Vector2 Size
    {
        get => rectangleShape.Size.ToSysNumVector();
        set => rectangleShape.Size = value.ToSFMLVector();
    }

    public RenderRect(Vector2 size)
    {
        this.rectangleShape = new RectangleShape(size.ToSFMLVector());
    }

    protected void SetTexture(string texturePath)
    {
        var texture = new Texture(texturePath);

        rectangleShape.Texture = texture;
    }
}