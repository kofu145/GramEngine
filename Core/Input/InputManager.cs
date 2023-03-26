using System.Numerics;
using SFML.Window;
using SFML.System;

namespace GramEngine.Core.Input;

// TODO: maybe get rid of singleton pattern?
// TODO: Massive overhaul required including event system.
public static class InputManager
    {
        // TODO: idea, can poll values continuously with events and update some events here?

        // TODO: ecs integration, UPDATE ALL DEFINITIONS TO CURRENT SFML
        /// <summary>
        /// The current mouse position, relative to the window's 0, 0. (Top left corner)
        /// </summary>
        public static Vector2 MousePos => ((Vector2f)Mouse.GetPosition()).ToSysNumVector();
        /// <summary>
        /// The difference in mouse position from the last frame.
        /// </summary>
        /*
        public Vector2 mouseDelta { get { return (Vector2)mouseState.Delta; } private set { } }
        public Vector2 prevMousePos { get { return (Vector2)mouseState.PreviousPosition; } private set { } }
        public Vector2 prevMouseScroll { get { return (Vector2)mouseState.PreviousScroll; } private set { } }
        public Vector2 scrollPos { get { return (Vector2)mouseState.Scroll; } private set { } }
        public Vector2 scrollDelta { get { return (Vector2)mouseState.ScrollDelta; } private set { } }
        */
        /// <summary>
        /// Poll if a key was pressed this frame.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        public static bool GetKeyPressed(Keys key)
        {
            return Keyboard.IsKeyPressed((Keyboard.Key)key);
        }
        /*
        public bool GetKeyPressed(Keys key)
        {
            retu
        }
        public bool GetKeyUp(Keys key)
        {
            return keyboardState.IsKeyReleased((OpenTK.Windowing.GraphicsLibraryFramework.Keys)key);
        }

        public bool GetMouseButtonDown(MouseButton button){
            return mouseState.IsButtonDown((OpenTK.Windowing.GraphicsLibraryFramework.MouseButton)button);
        }

        public bool GetMouseButtonWasDown(MouseButton button){
            return mouseState.WasButtonDown((OpenTK.Windowing.GraphicsLibraryFramework.MouseButton)button);
        }

        public bool GetIsAnyMouseKeyDown(){
            return mouseState.IsAnyButtonDown;
        }

        // probably don't need these, easy enough to check for themselves, but they still here just in case.

        public bool MouseHasMoved()
        {
            return mouseState.Position != mouseState.PreviousPosition;
        }

        public bool ScrollHasMoved()
        {
            return mouseState.Scroll != mouseState.PreviousScroll;
        }
        */
        

    }