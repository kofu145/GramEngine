using GramEngine.Core;

namespace GramEngine.ECS.Components;

internal class CircleColliderSystem : Component
{
    private List<CircleCollider>[,] grid;
    private float offset;

    public CircleColliderSystem(float offset)
    {
        this.offset = offset;
        var settings = GameStateManager.Window.settings;
        grid = new List<CircleCollider>[
            (int)Math.Ceiling(settings.Height + 1000 / offset),
            (int)Math.Ceiling(settings.Width + 1000 / offset)
        ];

        for (int i=0; i<grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j] = new List<CircleCollider>();
            }
        }

    }
    
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        ConstructGrid();
        // skip left, right, top, and bottom rows (we're going ot be comparing with them anyway)
        for (int y = 1; y < grid.GetLength(0) - 1; y++)
        {
            for (int x = 1; x < grid.GetLength(1) - 1; x++)
            {
                var currentCell = grid[y, x];
                for (int dy = -1; dy <= 1; dy++)
                {
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        var otherCell = grid[y + dy, x + dx];
                        CheckCellsCollisions(currentCell, otherCell);
                    }
                }
            }
        }
        /*
        foreach (var collider in grid)
        {
            if (collider is not null)
                Console.WriteLine(collider.GetType());
        }*/
    }

    private void ConstructGrid()
    {
        // wtf are C# multidimensional arrays
        foreach (List<CircleCollider> list in grid)
        {
            list.Clear();
        }

        foreach (var entity in ParentScene.Entities.Where(e => e.HasComponent<CircleCollider>()))
        {
            var pos = entity.Transform.Position;
            if (pos.X < 0 || pos.Y < 0)
                continue;

            var circleCollider = entity.GetComponent<CircleCollider>();
            
            //Console.WriteLine(pos.ToString());
            //Console.WriteLine($"i, j: {(int)(pos.X/offset)}, {(int)(pos.Y/offset)}");
            if (circleCollider.Enabled)
                grid[(int)(pos.Y/offset), (int)(pos.X/offset)].Add(circleCollider);
            
        }
           
    }

    private void CheckCellsCollisions(List<CircleCollider> cell1, List<CircleCollider> cell2)
    {
        if (cell1.Count <= 0 || cell2.Count <= 0)
            return;
        
        foreach (var collider1 in cell1)
        {
            foreach (var collider2 in cell2)
            {
                if (collider1 != collider2)
                {
                    CheckCollision(collider1, collider2);
                }
            }
        }
    }

    private void CheckCollision(CircleCollider c1, CircleCollider c2)
    {
        var e1 = c1.ParentEntity;
        var e2 = c2.ParentEntity;

        var e1T = e1.Transform;
        var e2T = e2.Transform;

        
        c1.wasColliding = false;
        if (c1.IsColliding)
        {
            c1.IsColliding = false;
            c1.wasColliding = true;
        }
        
        c2.wasColliding = false;
        if (c2.IsColliding)
        {
            c2.IsColliding = false;
            c2.wasColliding = true;
        }
        
            // basic distance formula, sqrt((x2 - x1)^2 + (y2 - y1)^2) (but sqrt is expensive, so we are just doing comparison of squared)
            float squaredDistBetweenCircles =
                (e2T.Position.X - e1T.Position.X) * (e2T.Position.X - e1T.Position.X) +
                (e2T.Position.Y - e1T.Position.Y) * (e2T.Position.Y - e1T.Position.Y);

            if (squaredDistBetweenCircles < (c1.Radius + c2.Radius) * (c1.Radius + c2.Radius))
            {
                if (!c1.wasColliding)
                    c1.OnEnterCollide(c2);
                if (!c2.wasColliding)
                    c2.OnEnterCollide(c1);
                
                // invoke collision event
                c1.OnCollide(c2);
                c1.IsColliding = true;
                
                c2.OnCollide(c1);
                c2.IsColliding = true;

                double angle = Math.Atan2(e2T.Position.Y - e1T.Position.Y, e2T.Position.X - e1T.Position.X);

                // we only calc the dist here as needed, because sqrt() is expensive
                var distBetweenCircles = Math.Sqrt(squaredDistBetweenCircles);

                var distToMove = c1.Radius + c2.Radius - distBetweenCircles;
                // do moving circle collision (reposition circle accordingly)

                if (!c1.Dynamic && !c2.Dynamic)
                {
                    // static static collision
                }
                
                if (c1.Dynamic)
                {
                    

                    if (!c2.Dynamic)
                    {
                        // move the collider to match bounds
                        Transform.Position.X -= (float)(Math.Cos(angle) * distToMove);
                        Transform.Position.Y -= (float)(Math.Sin(angle) * distToMove);
                        c1.IsColliding = false;
                        c2.IsColliding = false;
                    }
                    else
                    {
                        // move the collider to match bounds
                        e2T.Position.X += (float)(Math.Cos(angle) * distToMove);
                        e2T.Position.Y += (float)(Math.Sin(angle) * distToMove);
                        c1.IsColliding = false;
                        c2.IsColliding = false;

                    }
                }
            }
            if (!c1.IsColliding && c1.wasColliding)
            {
                c1.OnExitCollide(c2);
            }
            if (!c2.IsColliding && c2.wasColliding)
            {
                c2.OnExitCollide(c1);
            }
    }
}