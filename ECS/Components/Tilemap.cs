using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
using GramEngine.ECS;
using SFML.Graphics;
using SFML.System;
using SkiaSharp;

namespace GramEngine.Core;

public class Tilemap : Component, Drawable
{
    private Vector2 tileSize;
    public Vector2 TileSize { 
        get { return tileSize; }
        set { tileSize = value;  }
    }

    private Vector2 size;
    public Vector2 Size{
        get => size;
        set { size = value;  }
    }

    // width of the entire tilemap
    public uint Width => (uint)size.X;
    
    // height of the entire tilemap
    public uint Height => (uint)size.Y;

    internal Texture tileset;
    private Dictionary<Vector2, Tile> map;
    internal VertexArray mVertices;

    public Tilemap(Vector2 tileSize, Vector2 size, string tileAtlas)
    {
        this.tileSize = tileSize;
        this.size = size;
        this.tileset = new Texture(tileAtlas);
        // local vertex array for batch draw
        mVertices = new VertexArray();
    }

    public override void Initialize()
    {
        base.Initialize();
        Render();
    }

    
    internal void Render()
    {
        mVertices.PrimitiveType = PrimitiveType.Triangles;
        mVertices.Resize((uint)(Width * Height * 6));

        for (uint i = 0; i < Width; i++)
        {
            for (uint j = 0; j < Height; j++)
            {
                // index the tile we want from the atlas
                var tileidx = 0;
                // get the texture of the tile from the atlas
                
                int tu = tileidx % (int)(tileset.Size.X / tileSize.X);
                int tv = tileidx / (int)(tileset.Size.X / tileSize.X);

                var pos = (i + j * Width) * 6;
                var triangles = mVertices[pos];
                
                // define the 6 corners of the two triangles
                mVertices[pos] = new Vertex(new Vector2f(i * tileSize.X, j * tileSize.Y), 
                    new Vector2f(tu * tileSize.X, tv * tileSize.Y));
                
                mVertices[pos + 1] = new Vertex(new Vector2f((i + 1) * tileSize.X, j * tileSize.Y),
                    new Vector2f((tu + 1) * tileSize.X, tv * tileSize.Y));
                
                mVertices[pos + 2] = new Vertex(new Vector2f(i * tileSize.X, (j + 1) * tileSize.Y),
                        new Vector2f(tu * tileSize.X, (tv + 1) * tileSize.Y));
                
                mVertices[pos + 3] = new Vertex(new Vector2f(i * tileSize.X, (j + 1) * tileSize.Y),
                        new Vector2f(tu * tileSize.X, (tv + 1) * tileSize.Y));
                
                mVertices[pos + 4] = new Vertex(new Vector2f((i + 1) * tileSize.X, j * tileSize.Y),
                    new Vector2f((tu + 1) * tileSize.X, tv * tileSize.Y));
                
                mVertices[pos + 5] = new Vertex(new Vector2f((i + 1) * tileSize.X, (j + 1) * tileSize.Y),
                    new Vector2f((tu + 1) * tileSize.X, (tv + 1) * tileSize.Y));

            }
        }
}

    // maybe handle collisions here?
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
        
    }
}