using GramEngine.Core;

namespace GramEngine.ECS;

/// <summary>
/// An interface for the contents of all components; any defined component MUST derive from this.
/// </summary>
public interface IComponent
{
    public Entity ParentEntity { get; }

    public int Priority { get; }

    public bool Enabled { get; set; }

    public void Initialize();
    public void OnLoad();
    // update might want scene?
    public void Update(GameTime gameTime);

    // Check Component.cs on why this is commented out
    //public void LateUpdate(GameTime gameTime);

    public void FixedUpdate(GameTime gameTime);

    public void Dispose();

    public void SetParent(Entity parentEntity);

    public void ApplyNewCopyParent(Entity entity);

}