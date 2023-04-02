using GramEngine.Core;

namespace GramEngine.ECS;

internal interface IComponentSystem
{
    public void Initialize();
    
    public void OnLoad();
    
    public void Update(GameTime gameTime);
    
    public void FixedUpdate(GameTime gameTime);

    public void Dispose();

}