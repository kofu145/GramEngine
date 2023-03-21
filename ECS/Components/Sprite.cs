using GramEngine.Core;
using GramEngine.ECS;
using SFML.System;
using SFML.Graphics;
using System.Numerics;

namespace GramEngine.ECS.Components;

// Essentially a wrapper over SFML sprite
public class Sprite : Component
{
    private Texture texture;
    internal SFML.Graphics.Sprite sfmlSprite;
    
    public int Width => Convert.ToInt32(this.texture.Size.X);
    public int Height => Convert.ToInt32(this.texture.Size.Y);

    public Vector2 Origin
    {
        get { return sfmlSprite.Origin.ToSysNumVector(); }
        set { sfmlSprite.Origin = value.ToSFMLVector(); }
    }

    public Vector3 Color
    {
        get{ return sfmlSprite.Color.to }
        set{  }
    }

    public bool Enabled = true;
    
    /// <summary>
    /// Creates a new sprite component. Origin is automatically centered.
    /// </summary>
    /// <param name="textureFilePath">The filepath to the image you are using for your sprite.</param>
    public Sprite(string textureFilePath)
    {
        this.texture = new Texture(textureFilePath);
        // setting to point texture filtering
        this.texture.Smooth = false;
        this.sfmlSprite = new SFML.Graphics.Sprite(texture);
        this.Origin = new Vector2(Width / 2, Height / 2);
        this.Color = new Vector3(1f, 1f, 1f);
    }
    
    public override void OnLoad()
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