using System.Numerics;
using GramEngine.ECS;
using SFML.Graphics;
using SFML.System;
using Sprite = GramEngine.ECS.Components.Sprite;
using Transform = GramEngine.ECS.Transform;

namespace GramEngine.Core;

public class DisplayFrame
{
    private RenderTexture frame;
    public Transform Transform;

    public DisplayFrame(uint width, uint height)
    {
        frame = new RenderTexture(width, height);
        Transform = new Transform();
    }

    public void RenderSprite(Entity entity)
    {
        var sprite = entity.GetComponent<Sprite>();
        sprite.GetTransformTarget();
        frame.Draw(sprite.sfmlSprite);
    }

    /// <summary>
    /// Puts this DisplayFrame in the render queue, meaning it will draw after everything else.
    /// </summary>
    public void Render()
    {
        GameStateManager.Window.displayFrames.Add(this);
    }

    public void StopRender()
    {
        GameStateManager.Window.displayFrames.Remove(this);
    }

    public void Clear()
    {
        frame.Clear();
    }
    
    internal void RenderTexture(Texture texture, Vector3 displayPosition)
    {
        var renderTarget = new SFML.Graphics.Sprite(texture);
        renderTarget.Position = displayPosition.ToVec2().ToSFMLVector();
        frame.Draw(renderTarget);
    }

    internal SFML.Graphics.Sprite GetRenderTarget()
    {
        frame.Display();
        var display = new SFML.Graphics.Sprite(frame.Texture);
        var settings = GameStateManager.Window.settings;
        var sfmlVectorPos = this.Transform.Position.ToSFMLVector();
        display.Position = new Vector2f(
            sfmlVectorPos.X + settings.GlobalXOffset, 
            sfmlVectorPos.Y + settings.GlobalYOffset
        );
        display.Rotation = Transform.Rotation.Z;
        display.Scale = Transform.Scale.ToSFMLVector();
        return display;
    }
}