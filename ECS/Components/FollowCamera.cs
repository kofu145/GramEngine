using System.Numerics;
using GramEngine.Core;

namespace GramEngine.ECS.Components;

public class FollowCamera : Component
{
    public Vector3 Offset
    {
        get => offset;
        set => offset = value;
    }
    private Vector3 offset;
    
    public FollowCamera(Vector3 offset)
    {
        this.offset = offset;
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (ParentEntity != null)
            Transform.Position = GameStateManager.Window.CameraPosition.ToVec3() + offset;
    }
}