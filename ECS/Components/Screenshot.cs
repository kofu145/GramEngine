using GramEngine.Core;
using SFML.Graphics;

namespace GramEngine.ECS.Components;

public class Screenshot : Component
{
    private Texture texture;
    public Screenshot()
    {
        texture = new Texture(GameStateManager.Window.settings.Width, GameStateManager.Window.settings.Height);
    }

    internal void UpdateShot()
    {
    }

    public Sprite GetScreenshotAsSprite()
    {
        texture = new Texture(GameStateManager.Window.Width, GameStateManager.Window.Height);
        texture.Update(GameStateManager.Window.sfmlWindow);
        return new Sprite(texture);
    }

    public void SaveAsFile(string filename)
    {
        texture = new Texture(GameStateManager.Window.Width, GameStateManager.Window.Height);
        texture.Update(GameStateManager.Window.sfmlWindow);
        texture.CopyToImage().SaveToFile(filename);
    }
}