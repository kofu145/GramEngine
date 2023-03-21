using GramEngine.Core;
using GramEngine.ECS;

namespace GramEngine.ECS;

public abstract class Component : IComponent
{
    // this can be a guid, but then there is a cost because you have to search from the scene every time
    private Entity parentEntity;
    public Entity ParentEntity { get { return parentEntity; } private set { parentEntity = value; } }

    public int Priority { get; set; } = 3;

    // these two methods aren't abstract in case the users don't want to define them

    public abstract void OnLoad();

    // Late and Early update might come up sometime in the future? but they don't seem like they're very needed for now
    
    //public virtual void EarlyUpdate(GameTime gameTime) { }

    public abstract void Update(GameTime gameTime);

    //public virtual void LateUpdate(GameTime gameTime) { }

    public virtual void FixedUpdate(GameTime gameTime) { }

    public virtual void Dispose() { }

    // sparse definition for now
    public void ApplyNewCopyParent(Entity entity)
    {
        ParentEntity = entity;
    }

    public void SetParent(Entity entity)
    {
        if (ParentEntity != null)
        {
            ParentEntity.RemoveComponent(this);
            entity.AddComponent(this);
        }
        ParentEntity = entity;
    }
}