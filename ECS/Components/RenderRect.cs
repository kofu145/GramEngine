using SFML.Graphics;
using System.Numerics;
using GramEngine.Core;
using SFML.System;
using Color = System.Drawing.Color;

namespace GramEngine.ECS.Components;

public class RenderRect : Component, IRenderable
{
    internal RectangleShape rectangleShape;
    Drawable IRenderable.GetRenderTarget() => GetRenderTarget();


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
    
    public SFML.Graphics.RectangleShape GetRenderTarget()
    {
        return rectangleShape;
    }
    
    public SFML.Graphics.Transform GetTransformTarget()
    {
        var settings = GameStateManager.Window.settings;
        var sfmlVectorPos = Transform.Position.ToSFMLVector();
        rectangleShape.Position = new Vector2f(
            sfmlVectorPos.X + settings.GlobalXOffset, 
            sfmlVectorPos.Y + settings.GlobalYOffset
        );
        rectangleShape.Rotation = Transform.Rotation.Z;
        rectangleShape.Scale = Transform.Scale.ToSFMLVector();
        return rectangleShape.Transform;
    }
    
    void IRenderable.Draw(RenderWindow window)
    {
        var settings = GameStateManager.Window.settings;
        var sfmlVectorPos = Transform.Position.ToSFMLVector();
        rectangleShape.Position = new Vector2f(
            sfmlVectorPos.X + settings.GlobalXOffset, 
            sfmlVectorPos.Y + settings.GlobalYOffset
        );
        rectangleShape.Rotation = Transform.Rotation.Z;
        rectangleShape.Scale = Transform.Scale.ToSFMLVector();
        rectangleShape.Draw(window, new RenderStates(rectangleShape.Transform));
    }

    protected void SetTexture(string texturePath)
    {
        var texture = new Texture(texturePath);

        rectangleShape.Texture = texture;
    }
}