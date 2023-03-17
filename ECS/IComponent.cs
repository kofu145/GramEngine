using EirEngine.Core;

namespace EirEngine.ECS;

/// <summary>
/// An interface for the contents of all components; any defined component MUST derive from this.
/// </summary>
public interface IComponent
{
    public Entity ParentEntity { get; }

    public int Priority { get; }

    public void OnLoad();
    // update might want scene?
    public void Update(GameTime gameTime);

    public void LateUpdate(GameTime gameTime);

    public void FixedUpdate(GameTime gameTime);

    public void Dispose();

    public void SetParent(Entity parentEntity);

    public void ApplyNewCopyParent(Entity entity);

}