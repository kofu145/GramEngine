using System.Numerics;
using GramEngine.Core;

namespace GramEngine.ECS.Components;

/// <summary>
/// Component to set an entity to track its position relative towards a parent entity's, based on a local offset.
/// </summary>
public class FollowParent : Component
{
    public Vector3 Offset
    {
        get => offset;
        set => offset = value;
    }
    private Vector3 offset;
    
    public FollowParent(Vector3 offset)
    {
        this.offset = offset;
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (ParentEntity.ParentEntity != null)
            ParentEntity.Transform.Position = ParentEntity.ParentEntity.Transform.Position + offset;
        
    }
}