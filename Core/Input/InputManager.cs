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
        public static Vector2 MousePos => ((Vector2f)Mouse.GetPosition(GameStateManager.Window.sfmlWindow)).ToSysNumVector();

        internal static Dictionary<Keys, bool> KeyStateWasPressedToUpdate = new Dictionary<Keys, bool>();
        internal static Dictionary<Keys, bool> KeyStateWasReleasedToUpdate = new Dictionary<Keys, bool>();
        internal static Dictionary<MouseButton, bool> MouseButtonStateWasPressedToUpdate = new Dictionary<MouseButton, bool>();
        internal static Dictionary<MouseButton, bool> MouseButtonStateWasReleasedToUpdate = new Dictionary<MouseButton, bool>();
        
        internal static Dictionary<Keys, bool> KeyStateWasPressed = new Dictionary<Keys, bool>();
        internal static Dictionary<Keys, bool> KeyStateWasReleased = new Dictionary<Keys, bool>();
        internal static Dictionary<MouseButton, bool> MouseButtonStateWasPressed = new Dictionary<MouseButton, bool>();
        internal static Dictionary<MouseButton, bool> MouseButtonStateWasReleased = new Dictionary<MouseButton, bool>();

        
        public static Vector2 MouseWorldPos => 
            GameStateManager.Window.sfmlWindow.MapPixelToCoords(Mouse.GetPosition(GameStateManager.Window.sfmlWindow)).ToSysNumVector();
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
        /// Constructor
        /// </summary>
        static InputManager()
        {
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                KeyStateWasPressed[key] = false;
                KeyStateWasReleased[key] = false;
                KeyStateWasPressedToUpdate[key] = false;
                KeyStateWasReleasedToUpdate[key] = false;
            }
            
            foreach (MouseButton key in Enum.GetValues(typeof(MouseButton)))
            {
                MouseButtonStateWasPressed[key] = false;
                MouseButtonStateWasReleased[key] = false;
                MouseButtonStateWasPressedToUpdate[key] = false;
                MouseButtonStateWasReleasedToUpdate[key] = false;
            }
        }
        
        /// <summary>
        /// Poll if a key was pressed this frame.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        public static bool GetKeyPressed(Keys key)
        {
            return Keyboard.IsKeyPressed((Keyboard.Key)key) && GameStateManager.Window.WindowFocused;
        }

        public static bool GetKeyDown(Keys key)
        {
            if (GameStateManager.Window.WindowFocused)
            {
                if (KeyStateWasPressed[key])
                    KeyStateWasPressedToUpdate[key] = true;
                return KeyStateWasPressed[key];
            }

            return false;

        }

        public static bool GetKeyUp(Keys key)
        {
            if (GameStateManager.Window.WindowFocused)
            {
                if (KeyStateWasReleased[key])
                    KeyStateWasReleasedToUpdate[key] = true;
                return KeyStateWasReleased[key];
            }

            return false;
        }

        public static bool GetMousePress(MouseButton mouseButton)
        {
            return Mouse.IsButtonPressed((Mouse.Button)mouseButton) && GameStateManager.Window.WindowFocused;
        }
        
        public static bool GetMouseButtonDown(MouseButton mouseButton)
        {
            if (GameStateManager.Window.WindowFocused)
            {
                if (MouseButtonStateWasPressed[mouseButton])
                    MouseButtonStateWasPressedToUpdate[mouseButton] = true;
                return MouseButtonStateWasPressed[mouseButton];
            }

            return false;
        }

        public static bool GetMouseButtonUp(MouseButton mouseButton)
        {
            if (GameStateManager.Window.WindowFocused)
            {
                if (MouseButtonStateWasReleased[mouseButton])
                    MouseButtonStateWasReleasedToUpdate[mouseButton] = true;
                return MouseButtonStateWasReleased[mouseButton];
            }

            return false;
        }

        internal static void Update()
        {
            foreach (var key in KeyStateWasPressed.Keys.ToList())
            {
                if (KeyStateWasPressedToUpdate[key])
                {
                    KeyStateWasPressed[key] = false;
                    KeyStateWasPressedToUpdate[key] = false;
                }

                if (KeyStateWasReleasedToUpdate[key])
                {
                    KeyStateWasReleased[key] = false;
                    KeyStateWasReleasedToUpdate[key] = false;
                }
            }
            foreach (var key in MouseButtonStateWasPressed.Keys.ToList())
            {
                if (MouseButtonStateWasPressedToUpdate[key])
                {
                    MouseButtonStateWasPressed[key] = false;
                    MouseButtonStateWasPressedToUpdate[key] = false;
                }

                if (MouseButtonStateWasReleasedToUpdate[key])
                {
                    MouseButtonStateWasReleased[key] = false;
                    MouseButtonStateWasReleasedToUpdate[key] = false;
                }
            }
            /*keyStateWasPressed = new Dictionary<Keys, bool>(keyStateWasPressedToUpdate);
            keyStateWasReleased = new Dictionary<Keys, bool>(keyStateWasReleasedToUpdate);
            mouseButtonStateWasPressed = new Dictionary<MouseButton, bool>(mouseButtonStateWasPressedToUpdate);
            mouseButtonStateWasReleased = new Dictionary<MouseButton, bool>(mouseButtonStateWasReleasedToUpdate);*/
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