namespace GramEngine.ECS;

/// <summary>
/// Prefab class, for defining entities with specific collections of components to be recreated later.
/// (There's currently nothing here right now, will add functionality as we go)
/// </summary>
public abstract class Prefab
{
    public abstract Entity Instantiate();
}