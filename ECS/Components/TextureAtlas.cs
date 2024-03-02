using System.Numerics;
using GramEngine.Core;
using SFML.Graphics;

namespace GramEngine.ECS.Components;

public class TextureAtlas : Component
{
    private Image textureAtlas;
    public TextureAtlas(string textureAtlasPath)
    {
        this.textureAtlas = new Image(textureAtlasPath);
    }

    public Sprite GetSpriteComponent((int width, int height) dimensions, (int x, int y) position)
    {
        var text = new Texture(
            textureAtlas,
            new IntRect(position.x, position.y, dimensions.width, dimensions.height)
        );

        var retSprite = new Sprite(text, false);

        return retSprite;
    }

    public void RenderToFrame(DisplayFrame displayFrame, (int width, int height) dimensions, (int x, int y) position, Vector3 displayPosition)
    {
        var text = new Texture(
            textureAtlas,
            new IntRect(position.x, position.y, dimensions.width, dimensions.height)
        );
        displayFrame.RenderTexture(text, displayPosition);
    }
    
    public override void Initialize()
    {
        base.Initialize();
        
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}