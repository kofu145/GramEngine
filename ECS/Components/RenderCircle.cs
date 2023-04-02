using System.Numerics;
using GramEngine.Core;
using System.Drawing;
using SFML.Graphics;
using SFML.System;

namespace GramEngine.ECS.Components;

public class RenderCircle : Component, IRenderable
{
    internal CircleShape circleShape;
    Drawable IRenderable.GetRenderTarget() => GetRenderTarget();

    
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
    
    public SFML.Graphics.CircleShape GetRenderTarget()
    {
        return circleShape;
    }

    public SFML.Graphics.Transform GetTransformTarget()
    {
        var settings = GameStateManager.Window.settings;
        var sfmlVectorPos = Transform.Position.ToSFMLVector();
        circleShape.Position = new Vector2f(
            sfmlVectorPos.X + settings.GlobalXOffset, 
            sfmlVectorPos.Y + settings.GlobalYOffset
        );
        circleShape.Rotation = Transform.Rotation.Z;
        circleShape.Scale = Transform.Scale.ToSFMLVector();
        return circleShape.Transform;
    }

    void IRenderable.Draw(RenderWindow window)
    {
        var settings = GameStateManager.Window.settings;
        var sfmlVectorPos = Transform.Position.ToSFMLVector();
        circleShape.Position = new Vector2f(
            sfmlVectorPos.X + settings.GlobalXOffset, 
            sfmlVectorPos.Y + settings.GlobalYOffset
        );
        circleShape.Rotation = Transform.Rotation.Z;
        //circleShape.Scale = Transform.Scale.ToSFMLVector();
        circleShape.Draw(window, new RenderStates(circleShape.Transform));
    }

    protected void SetTexture(string texturePath)
    {
        var texture = new Texture(texturePath);

        circleShape.Texture = texture;
    }
}