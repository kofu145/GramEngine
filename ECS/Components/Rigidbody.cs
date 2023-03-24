using System.Numerics;
using GramEngine.Core;

namespace GramEngine.ECS.Components;

public class Rigidbody : Component
{
    public Vector3 Velocity { get; set; }

    public Rigidbody()
    {
        Velocity = Vector3.Zero;
    }
    public void AddForce(Vector3 force)
    {
        Velocity += force;
    }

    public override void Initialize()
    {
    }

    public override void Update(GameTime gameTime)
    {
        //Console.WriteLine(Velocity);
        ParentEntity.Transform.Position += Velocity * (float)gameTime.DeltaTime;
    }
}