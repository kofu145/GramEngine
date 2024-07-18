using System.Numerics;
using GramEngine.Core;
using System.Drawing;
using SFML.Graphics;
using SFML.System;

namespace GramEngine.ECS.Components;

public class RenderRect : Component, IRenderable
{
    internal RectangleShape rectangleShape;
    Drawable IRenderable.GetRenderTarget() => GetRenderTarget();

    public Vector2 Origin
    {
        get { return rectangleShape.Origin.ToSysNumVector(); }
        set { rectangleShape.Origin = value.ToSFMLVector(); }
    }

    public System.Drawing.Color FillColor
    {
        get => rectangleShape.FillColor.ToSysColor();
        set => rectangleShape.FillColor = value.ToSFMLColor();
    }

    public System.Drawing.Color OutlineColor
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

    public RenderRect(Vector2 size, bool centerOrigin = true)
    {
        this.rectangleShape = new RectangleShape(size.ToSFMLVector());
        if (centerOrigin)
        {
            Origin = new Vector2(size.X / 2, size.Y / 2);
        }
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
        //rectangleShape.Scale = Transform.Scale.ToSFMLVector();
        rectangleShape.Draw(window, new RenderStates(rectangleShape.Transform));
    }

    protected void SetTexture(string texturePath)
    {
        var texture = new Texture(texturePath);

        rectangleShape.Texture = texture;
    }
}