using GramEngine.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GramEngine.ECS;


/// <summary>
    /// Represents an container for component data.
    /// </summary>
    public class Entity
    {
        // use arrays for cache efficiency

        /// <summary>
        /// A readonly collection of all components attributed to this Entity.
        /// </summary>
        private readonly ConcurrentDictionary<Type, IComponent> components;
        /// <summary>
        /// A readonly GUID identifying this entity.
        /// </summary>
        public readonly Guid id;

        /// <summary>
        /// The <see cref="GramEngine.ECS.Transform"/> for this entity.
        /// </summary>
        public Transform Transform;

        // Just here for utility
        /// <summary>
        /// The parent <see cref="Scene"/> of this entity.
        /// </summary>
        public Scene ParentScene { get; internal set; }
        
        /// <summary>
        /// The parent entity of this entity, if any.
        /// </summary>
        public Entity? ParentEntity { get; private set; } = null;

        private readonly List<Entity> childEntities;
        
        /// <summary>
        /// A string attributed to entities for ease of identification. 
        /// </summary>
        public string Tag { get; set; }
        
        /// <summary>
        /// Designates whether or not this is a UI entity.
        /// </summary>
        public bool IsUIEntity { get; set; } = false;

        /// <summary>
        /// Quick check set, so components added after entity's been added to scene still get Initialize() called.
        /// </summary>
        private bool initialized = false;

        /// <summary>
        /// Initializes a new entity.
        /// </summary>
        public Entity(bool isUiEntity = false)
        {
            this.id = Guid.NewGuid();
            IsUIEntity = isUiEntity;
            this.components = new ConcurrentDictionary<Type, IComponent>();
            childEntities = new List<Entity>();
            this.Transform = new Transform();
        }

        /// <summary>
        /// Initializes a new entity.
        /// </summary>
        /// <param name="id">The GUID identifier for this entity.</param>
        public Entity(Guid id, bool isUiEntity = false)
        {
            this.id = id;
            this.IsUIEntity = isUiEntity;
            childEntities = new List<Entity>();
            this.components = new ConcurrentDictionary<Type, IComponent>();
        }

        /// Initializes a new entity with a component.
        /// </summary>
        /// <param name="component">The component for this entity.</param>
        public Entity(IComponent component)
        {
            this.id = Guid.NewGuid();
            this.components = new ConcurrentDictionary<Type, IComponent>();
            childEntities = new List<Entity>();
            this.components.TryAdd(component.GetType(), component);

        }

        internal Entity(ConcurrentDictionary<Type, IComponent> copyComponents)
        {
            this.id = Guid.NewGuid();
            components = copyComponents;
            childEntities = new List<Entity>();
        }

        internal void Initialize()
        {
            foreach (var component in components)
            {
                component.Value.Initialize();
            }

            initialized = true;
        }

        internal void OnLoad()
        {
            foreach (var component in components)
            {
                component.Value.OnLoad();
            }
        }

        internal void Dispose()
        {
            foreach (var component in components)
            {
                component.Value.Dispose();
            }
        }

        internal void Update(GameTime gameTime)
        {
            foreach (var component in components)
            {
                if (component.Value.Enabled)
                {
                    component.Value.Update(gameTime);
                }
            }
        }
        
        /// <summary>
        /// Grabs a specified type of component from the entity.
        /// </summary>
        /// <typeparam name="T">The type of component to obtain.</typeparam>
        /// <returns>The component specified as the typeparam.</returns>
        /// <example>
        /// <code>
        /// entity.GetComponent<Transform>();
        /// </code>
        /// </example>
        public T GetComponent<T>() where T : class, IComponent
        {
            return components.ContainsKey(typeof(T)) ? components[typeof(T)] as T : null;
        }

        /// <summary>
        /// Checks whether the entity owns a specific type of component.
        /// </summary>
        /// <typeparam name="T">The type of component to obtain.</typeparam>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// if (entity.HasComponent<Transform>())
        /// {
        ///     // do something
        /// }
        /// </code>
        /// </example>
        public bool HasComponent<T>() where T : IComponent
        {
            return components.ContainsKey(typeof(T));
        }

        // Add a component. Returns the component added, if needed for manipulation right afterward.
        public T AddComponent<T>(T component) where T : IComponent
        {
            components[typeof(T)] = component;
            component.SetParent(this);
            if (initialized)
            {
                component.Initialize();
            }
            return component;
        }

        public IComponent RemoveComponent<T>() where T: IComponent
        {
            IComponent value;
            components.TryRemove(typeof(T), out value);
            return value;
        }

        // overload to pass by instance, if you want
        public IComponent RemoveComponent<T>(T component) where T : IComponent
        {
            IComponent value;
            components.TryRemove(typeof(T), out value);
            return value;
        }

        /// <summary>
        /// Sets the parent of the current entity.
        /// </summary>
        /// <param name="entity">The entity to designate as this entity's parent</param>
        public void SetParent(Entity? entity)
        {
            if (entity != null)
            {
                if (ParentEntity != null)
                    ParentEntity.childEntities.Remove(this);
                
                ParentEntity = entity;
                ParentEntity.childEntities.Add(this);
                
            }
        }

        /// <summary>
        /// Get a list of all the child entities of this entity.
        /// Note that this is a copy of the actual list to keep track of children.
        /// </summary>
        /// <returns>A copy of the child entities list.</returns>
        public List<Entity> GetChildren()
        {
            return new List<Entity>(childEntities);
        }

        internal List<IComponent> GetRenderable()
        {
            var renderables = new List<IComponent>();
            foreach (var component in components)
            {
                if (component.Value is IRenderable)
                {
                    renderables.Add(component.Value);
                }
            }

            return renderables;
        }

        /*
         
        // Why is this needed? 
        
        /// <summary>
        /// Returns an array representing the components of this entity. 
        /// (don't modify this, it shouldn't mess with the values)
        /// </summary>
        /// <returns></returns>
        internal IComponent[] GetComponents()
        {
            IComponent[] iterableComponents = new IComponent[components.Count];
            components.Values.CopyTo(iterableComponents, 0);

            return Utils.QuickSortComponentsArray(iterableComponents, 0, iterableComponents.Length - 1);
            
        }
        */

    }