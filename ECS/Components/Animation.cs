using System.Collections.Concurrent;
using GramEngine.Core;
using SFML.Graphics;

namespace GramEngine.ECS.Components;

public class Animation : Component
{
    private string state;
    internal ConcurrentDictionary<string, FrameData> Animations;
    private int currFrame = 0;
    private float frameProgress = 0;
    private Sprite parentSprite;
    private bool loop = true;
    private bool particle = false;
    private bool paused = false;
    public bool Complete{
        get => currFrame >= Animations[state].Frames.Count-1 && !loop; 
    }
    public string State => state;  

    public bool ResetOnFinish = false;
    
    public Animation(bool particle = false)
    {
        this.particle = particle;
        Animations = new ConcurrentDictionary<string, FrameData>();
        
    }

    public void Play()
    {
        currFrame = 0;
        paused = false;
    }

    public void Pause()
    {
        paused = true;
    }

    public void Reset()
    {
        currFrame = 0;
        paused = true;
    }

    public void SetState(string state, bool loop = true)
    {
        this.loop = loop;
        if (this.state != state)
        {
            this.state = state;
            currFrame = 0;
            frameProgress = 0;
        }
    }

    public void LoadTextureAtlas(string textureFilePath, string stateName, float frameTime, (int x, int y) dimensions)
    {
        if (!Animations.TryAdd(stateName, new FrameData(new List<Texture>(), frameTime, dimensions))) 
            throw new Exception($"Tried to load {stateName}, but key already exists in this Animation!");
        
        
        var mainTexture = new Image(textureFilePath);
        for (int j=0; j < mainTexture.Size.Y / dimensions.y; j++)
        {
            for (int i = 0; i < mainTexture.Size.X / dimensions.x; i++)
            {
                Animations[stateName].Frames.Add(new Texture(
                    mainTexture, 
                    new IntRect(i*dimensions.x, j*dimensions.y, dimensions.x, dimensions.y)
                    ));
            }
        }
    }

    internal struct FrameData
    {
        internal List<Texture> Frames;
        internal float FrameTime;
        internal (int x, int y) Dimensions;

        internal FrameData(List<Texture> frames, float frameTime, (int x, int y) dimensions)
        {
            Frames = frames;
            FrameTime = frameTime;
            Dimensions = dimensions;
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        parentSprite = ParentEntity.GetComponent<Sprite>();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        

        if (gameTime.TotalTime.TotalSeconds > frameProgress)
        {
            if (currFrame >= Animations[state].Frames.Count-1 && loop)
                currFrame = 0;
            else if (currFrame >= Animations[state].Frames.Count - 1 && !loop)
            {
                if (particle)
                    ParentScene.DestroyEntity(ParentEntity);
            }
            else if (!paused)
                currFrame++;
            if (currFrame >= Animations[state].Frames.Count-1 && ResetOnFinish)
            {
                paused = true;
                currFrame = 0;
            }
            
            frameProgress = (float)gameTime.TotalTime.TotalSeconds + Animations[state].FrameTime;
        }

        parentSprite.sfmlSprite.Texture = Animations[state].Frames[currFrame];
        parentSprite.sfmlSprite.TextureRect = new IntRect(0, 0,
            (int)Animations[state].Frames[currFrame].Size.X,
            (int)Animations[state].Frames[currFrame].Size.Y);
    }
}