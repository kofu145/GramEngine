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
        /// A string attributed to entities for ease of identification. 
        /// </summary>
        public string Tag { get; set; }
        
        internal bool isMaster { get; set; } = false;

        /// <summary>
        /// Initializes a new entity.
        /// </summary>
        public Entity()
        {
            this.id = Guid.NewGuid();
            this.components = new ConcurrentDictionary<Type, IComponent>();
            this.Transform = new Transform();
        }

        /// <summary>
        /// Initializes a new entity.
        /// </summary>
        /// <param name="id">The GUID identifier for this entity.</param>
        public Entity(Guid id)
        {
            this.id = id;
            this.components = new ConcurrentDictionary<Type, IComponent>();
        }

        /// Initializes a new entity.
        /// </summary>
        /// <param name="component">The component for this entity.</param>
        public Entity(IComponent component)
        {
            this.id = Guid.NewGuid();
            this.components = new ConcurrentDictionary<Type, IComponent>();
            this.components.TryAdd(component.GetType(), component);

        }

        internal Entity(ConcurrentDictionary<Type, IComponent> copyComponents)
        {
            this.id = Guid.NewGuid();
            components = copyComponents;
        }

        internal void Initialize()
        {
            foreach (var component in components)
            {
                component.Value.Initialize();
            }
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

        // Add a component. Returns the current entity for chain-adding.
        public Entity AddComponent<T>(T component) where T : IComponent
        {
            components[typeof(T)] = component;
            component.SetParent(this);
            return this;
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