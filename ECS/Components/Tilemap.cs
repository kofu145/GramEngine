using System.Numerics;
using GramEngine.ECS;
using SFML.Graphics;
using SFML.System;
using SkiaSharp;

namespace GramEngine.Core;

public class Tilemap : Component
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

    private Texture tileset;
    private Dictionary<Vector2, Tile> map;
    internal VertexArray mVertices;

    public Tilemap(Vector2 tileSize, Vector2 size, Image tileAtlas)
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
    }

    internal void Render()
    {
        mVertices.Clear();
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
                triangles.Position = new Vector2f(i * tileSize.X, j * tileSize.Y);
                triangles = mVertices[pos + 1];
                triangles.Position = new Vector2f((i + 1) * tileSize.X, j * tileSize.Y);
                triangles = mVertices[pos + 2];
                triangles.Position = new Vector2f(i * tileSize.X, (j + 1) * tileSize.Y);
                triangles = mVertices[pos + 3];
                triangles.Position = new Vector2f(i * tileSize.X, (j + 1) * tileSize.Y);
                triangles = mVertices[pos + 4];
                triangles.Position = new Vector2f((i + 1) * tileSize.X, j * tileSize.Y);
                triangles = mVertices[pos + 5];
                triangles.Position = new Vector2f((i + 1) * tileSize.X, (j + 1) * tileSize.Y);

                // define the 6 matching texture coordinates
                triangles = mVertices[pos];
                triangles.TexCoords = new Vector2f(tu * tileSize.X, tv * tileSize.Y);
                triangles = mVertices[pos + 1];
                triangles.TexCoords =  new Vector2f((tu + 1) * tileSize.X, tv * tileSize.Y);
                triangles = mVertices[pos + 2];
                triangles.TexCoords =  new Vector2f(tu * tileSize.X, (tv + 1) * tileSize.Y);
                triangles = mVertices[pos + 3];
                triangles.TexCoords =  new Vector2f(tu * tileSize.X, (tv + 1) * tileSize.Y);
                triangles = mVertices[pos + 4];
                triangles.TexCoords =  new Vector2f((tu + 1) * tileSize.X, tv * tileSize.Y);
                triangles = mVertices[pos + 5];
                triangles.TexCoords =  new Vector2f((tu + 1) * tileSize.X, (tv + 1) * tileSize.Y);

            }
        }
}

    // maybe handle collisions here?
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
    
}