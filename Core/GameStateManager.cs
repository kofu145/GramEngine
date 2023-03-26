﻿using System;

namespace GramEngine.Core;
public static class GameStateManager
    {
        
        private static Stack<IGameState> screens = new Stack<IGameState>();

        public static Window Window { get; internal set; }
        
        public static GameTime GameTime { get; internal set; }
        
        // I've kind of realized that the singleton pattern here is mostly pointless? So I'm changing it to a
        // static class instead.
        /*
        /// <summary>
        /// The singleton instance of the GameStateManager.
        /// </summary>
        private static GameStateManager instance;
        // Singleton Pattern Logic
        public static GameStateManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameStateManager();
                }
                return instance;
            }
        }
        */
        /// <summary>
        /// Adds a screen to the top of the stack.
        /// </summary>
        /// <param name="screen">The GameState to push to the top of the stack.</param>
        public static void AddScreen(IGameState screen)
        {
            try
            {
                // add screen to the stack
                screens.Push(screen);
                /*
                if (content != null)
                {
                    screens.Peek().LoadContent(content);
                }*/
                screens.Peek().Initialize();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// Returns the current rendering screen at the top of the stack.
        /// </summary>
        /// <returns></returns>
        public static IGameState GetScreen()
        {
            try
            {
                return screens.Peek();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
        /// <summary>
        /// Removes the top screen (gamestate) from the stack.
        /// </summary>
        public static void RemoveScreen()
        {
            if (screens.Count > 0)
            {
                try
                {
                    // var screen = screens.Peek();
                    screens.Pop();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Removes the top screen (gamestate) from the stack, and appends a new one.
        /// </summary>
        /// <param name="screen"></param>
        public static void SwapScreen(IGameState screen)
        {
            RemoveScreen();
            AddScreen(screen);
        }

        /// <summary>
        /// Clears the entire stack of gamestates.
        /// </summary>
        public static void ClearScreens()
        {
            screens.Clear();
        }
        /// <summary>
        /// Purges all screens from the stack, and adds a new one.
        /// </summary>
        /// <param name="screen">The new <see cref="IGameState"/> screen to add.</param>
        public static void ChangeScreen(IGameState screen)
        {
            ClearScreens();
            AddScreen(screen);
        }
        /// <summary>
        /// Updates the top screen of the stack.
        /// </summary>
        public static void Update(GameTime gameTime)
        {
            if (screens.Count > 0)
            {
                screens.Peek().Update(gameTime);

            }
        }
        /// <summary>
        /// Renders the top screen of the stack.
        /// </summary>
        public static void Draw()
        {
            if (screens.Count > 0)
            {
                screens.Peek().Draw();
            }
        }

        /// <summary>
        /// Calls OnLoad methods for all screens when loading.
        /// </summary>
        public static void OnLoad()
        {
            foreach (IGameState state in screens)
            {
                state.OnLoad();
            }
        }
        /// <summary>
        /// Calls Dispose methods for all screens when unloading.
        /// </summary>
        public static void Dispose()
        {
            foreach (IGameState state in screens)
            {
                state.Dispose();
            }
        }
    }