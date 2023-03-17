using EirEngine.ECS;
using SFML.System;
using System.Numerics;

namespace EirEngine.Core;

/// <summary>
/// A class encapsulating the collection of entities for a gamestate.
/// </summary>
public class Scene
{
    /// <summary>
    /// The list of all entities in the scene.
    /// </summary>
    private List<Entity> entities;

    private List<Entity> entitiesToAdd;
    private List<Entity> entitiesToDestroy;
    
    public Vector2 backgroundOffset;

    public IReadOnlyList<Entity> Entities { get { return entities; } }
    
    public Scene()
    {
        entities = new List<Entity>();
        entitiesToAdd = new List<Entity>();
        entitiesToDestroy = new List<Entity>();
        backgroundOffset = new Vector2(0, 0);
    }

    public void AddEntity(Entity entity)
    {
        entitiesToAdd.Add(entity);
    }

    public void DestroyEntity(Entity entity)
    {
        entitiesToDestroy.Add(entity);
    }

    internal void UpdateEntities()
    {
        foreach(var entity in entitiesToAdd)
        {
            entities.Add(entity);

            entity.OnLoad();

        }

        foreach(var entity in entitiesToDestroy)
        {
            entity.Dispose();
            entities.Remove(entity);
        }

        entitiesToAdd.Clear();
        entitiesToDestroy.Clear();
    }

}