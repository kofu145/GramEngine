using SFML.System;
using SFML.Graphics;

namespace GramEngine.ECS;

internal interface IRenderable
{
    public Drawable GetRenderTarget();

    public SFML.Graphics.Transform GetTransformTarget();

    public void Draw(RenderWindow window);
}