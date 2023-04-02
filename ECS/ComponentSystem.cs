using GramEngine.Core;

namespace GramEngine.ECS;

/// <summary>
/// These aren't REAL systems, as every component includes behavior anyway - these are just simple ways to organize
/// and have higher order systems in place in order to iterate over and manage individual components where needed.
/// </summary>
internal abstract class ComponentSystem : IComponentSystem
{
    public virtual void Initialize() {}

    public virtual void OnLoad() { }
    // Late and Early update might come up sometime in the future? but they don't seem like they're very needed for now
    
    //public virtual void EarlyUpdate(GameTime gameTime) { }

    public virtual void Update(GameTime gameTime) {}

    //public virtual void LateUpdate(GameTime gameTime) { }

    public virtual void FixedUpdate(GameTime gameTime) { }

    public virtual void Dispose() { }
}