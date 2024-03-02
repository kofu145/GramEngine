using GramEngine.Core;
using GramEngine.ECS;
using SFML.System;
using SFML.Graphics;
using System.Numerics;
using System.Drawing;

namespace GramEngine.ECS.Components;

// Essentially a wrapper over SFML sprite
public class Sprite : Component, IRenderable
{
    internal Texture texture;
    internal SFML.Graphics.Sprite sfmlSprite { get; set; }
    public int Width => Convert.ToInt32(this.texture.Size.X);
    public int Height => Convert.ToInt32(this.texture.Size.Y);

    Drawable IRenderable.GetRenderTarget() => GetRenderTarget();
    public Vector2 Origin
    {
        get { return sfmlSprite.Origin.ToSysNumVector(); }
        set { sfmlSprite.Origin = value.ToSFMLVector(); }
    }

    public System.Drawing.Color Color
    {
        get { return sfmlSprite.Color.ToSysColor(); }
        set { sfmlSprite.Color = value.ToSFMLColor(); }
    }

    public bool Enabled = true;

    private bool centerOrigin;

    internal Sprite(Texture texture, bool centerOrigin = true)
    {
        this.texture = texture;
        // setting to point texture filtering (in future, have a sprite settings struct, like window)
        this.texture.Smooth = false;
        this.sfmlSprite = new SFML.Graphics.Sprite(texture);
        this.Color = System.Drawing.Color.FromArgb(255, 255, 255);
        if (centerOrigin)
        {
            Origin = new Vector2(Width / 2, Height / 2);
        }
    }
    
    /// <summary>
    /// Creates a new sprite component. Origin is automatically centered.
    /// </summary>
    /// <param name="textureFilePath">The filepath to the image you are using for your sprite.</param>
    public Sprite(string textureFilePath, bool centerOrigin = true)
    {
        this.texture = new Texture(textureFilePath);
        // setting to point texture filtering (in future, have a sprite settings struct, like window)
        this.texture.Smooth = false;
        this.sfmlSprite = new SFML.Graphics.Sprite(texture);
        this.Color = System.Drawing.Color.FromArgb(255, 255, 255);
        if (centerOrigin)
        {
            Origin = new Vector2(Width / 2, Height / 2);
        }
    }

    public Sprite(MemoryStream textureBitmap)
    {
        this.texture = new Texture(textureBitmap);
        // setting to point texture filtering (in future, have a sprite settings struct, like window)
        this.texture.Smooth = false;
        this.sfmlSprite = new SFML.Graphics.Sprite(texture);
        this.Color = System.Drawing.Color.FromArgb(255, 255, 255);
        if (centerOrigin)
        {
            this.Origin = new Vector2(Width / 2, Height / 2);
        }
    }

    private SFML.Graphics.Sprite GetRenderTarget()
    {
        return sfmlSprite;
    }

    // TODO: update all of these + other renderables to "UpdateTransform" or something of the like
    public SFML.Graphics.Transform GetTransformTarget()
    {
        var settings = GameStateManager.Window.settings;
        var sfmlVectorPos = Transform.Position.ToSFMLVector();
        sfmlSprite.Position = new Vector2f(
            sfmlVectorPos.X + settings.GlobalXOffset, 
            sfmlVectorPos.Y + settings.GlobalYOffset
        );
        sfmlSprite.Rotation = Transform.Rotation.Z;
        sfmlSprite.Scale = Transform.Scale.ToSFMLVector();
        return sfmlSprite.Transform;
    }

    void IRenderable.Draw(RenderWindow window)
    {
        // We set the sprite render transform to be the same as the entity's
        // shorthand for easy writing
        var settings = GameStateManager.Window.settings;
        var sfmlVectorPos = Transform.Position.ToSFMLVector();
        sfmlSprite.Position = new Vector2f(
            sfmlVectorPos.X + settings.GlobalXOffset, 
            sfmlVectorPos.Y + settings.GlobalYOffset
        );
        sfmlSprite.Rotation = Transform.Rotation.Z;
        sfmlSprite.Scale = Transform.Scale.ToSFMLVector();
        sfmlSprite.Draw(window, new RenderStates(sfmlSprite.Transform));
    }
    
    public override void Initialize()
    {
    }

    public override void Dispose()
    {
        sfmlSprite.Dispose();
        texture.Dispose();
    }
    
    public override void Update(GameTime gameTime)
    {
    }
    
    
}