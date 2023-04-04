using GramEngine.ECS;
using SFML.System;
using System.Numerics;

namespace GramEngine.Core;

/// <summary>
/// A class encapsulating the collection of entities for a gamestate.
/// </summary>
public class Scene
{
    /// <summary>
    /// The list of all entities in the scene.
    /// </summary>
    private List<Entity> entities;
    private List<Entity> masterEntities;
    
    private List<Entity> entitiesToAdd;
    private List<Entity> entitiesToDestroy;
    private List<Entity> uninitializedEntities;
    internal List<IComponentSystem> systems;

    public Vector2 backgroundOffset;
    public IReadOnlyList<Entity> Entities { get { return entities; } }
    
    internal IReadOnlyList<Entity> EntitiesAndMaster => entities.Concat(masterEntities).ToList();

    public Scene()
    {
        entities = new List<Entity>();
        entitiesToAdd = new List<Entity>();
        entitiesToDestroy = new List<Entity>();
        uninitializedEntities = new List<Entity>();
        masterEntities = new List<Entity>();
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

    /// <summary>
    /// <para>Returns the first entity found with a particular tag.</para>
    /// Use <see cref="FindEntitiesWithTag"/> if you have multiple entities with the same tag.
    /// </summary>
    /// <param name="tag">The entity tag to be searched for.</param>
    public Entity FindWithTag(string tag)
    {
        foreach (var entity in entities)
        {
            if (entity.Tag == tag)
            {
                return entity;
            }
        }

        return null;
    }
    
    /// <summary>
    /// <para>Returns the first entity found with a particular tag.</para>
    /// Use <see cref="FindEntitiesWithTag"/> if you have multiple entities with the same tag.
    /// </summary>
    /// <param name="tag">The entity tag to be searched for.</param>
    internal Entity FindMasterEntityWithTag(string tag)
    {
        foreach (var entity in masterEntities)
        {
            if (entity.Tag == tag)
            {
                return entity;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns all entities found with a particular tag.
    /// </summary>
    /// <param name="tag">The entity tag to be searched for.</param>
    public List<Entity> FindEntitiesWithTag(string tag)
    {
        // simple linear search, TODO: faster algorithm?
        List<Entity> foundEntities = new List<Entity>();

        foreach (var entity in entities)
        {
            if (entity.Tag == tag)
            {
                foundEntities.Add(entity);
            }
        }

        return foundEntities;
    }

    internal void OnLoad()
    {
        UpdateEntitiesList();
        foreach (var entity in entities)
        {
            entity.OnLoad();
        }
    }

    internal void UpdateEntities(GameTime gameTime)
    {
        foreach(var entity in entities)
        {
            entity.Update(gameTime);
        }

        foreach (var entity in masterEntities)
        {
            entity.Update(gameTime);
        }
    }

    internal void UpdateEntitiesList()
    {
        foreach(var entity in entitiesToAdd)
        {
            entity.ParentScene = this;
            if (entity.isMaster)
                masterEntities.Add(entity);
            else
                entities.Add(entity);
            uninitializedEntities.Add(entity);
        }

        foreach(var entity in entitiesToDestroy)
        {
            if (entity.isMaster)
                masterEntities.Remove(entity);
            else
                entities.Remove(entity);
            entity.Dispose();

        }

        entitiesToAdd.Clear();
        entitiesToDestroy.Clear();
        
        foreach (var entity in uninitializedEntities)
        {
            // in case the collection entitiesToAdd gets modified within initialize();
            entity.Initialize();
        }
        uninitializedEntities.Clear();
    }

}