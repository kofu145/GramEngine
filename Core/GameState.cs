using GramEngine.ECS;
using SFML.System;
using SFML.Window;

namespace GramEngine.Core;

/// <summary>
/// An abstract class defining the contents of a gamestate. All gamestates MUST derive from this.
/// </summary>
/// <remarks>
/// Gamestates act as environments to hold and collect your content. You can think of them as
/// levels to utilize, whether for a full simple game or for ui elements like menus.
/// </remarks>
public abstract class GameState : IGameState
{
    /// <summary>
    /// The main scene of the game state.
    /// </summary>
    public Scene GameScene { get; private set; }
    
    public GameState()
    {
        GameScene = new Scene();
    }

    public GameState(Scene scene)
    {
        GameScene = new Scene();
    }

    /// <summary>
    /// Called on the initialization of the game state.
    /// </summary>
    public virtual void Initialize()
    {
        
    }

    /// <summary>
    /// Called on the loading of everything in the scene.
    /// </summary>
    public virtual void OnLoad()
    {
        GameScene.OnLoad();
    }

    /// <summary>
    /// Called when content is unloaded.
    /// </summary>
    public virtual void Dispose()
    {
        
    }

    /// <summary>
    /// Called on a fixed timestep. This should include your game logic.
    /// </summary>
    public virtual void Update(GameTime gameTime)
    {
        
    }

    /// <summary>
    /// The rendering call, called before Update. Draw calls for rendering sprites should be called here.
    /// </summary>
    public virtual void Draw()
    {
        
    }

    /// <summary>
    /// Small shorthand for adding entities
    /// </summary>
    /// <param name="entity"></param>
    public void AddEntity(Entity entity)
    {
        GameScene.AddEntity(entity);
    }
}