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
        public static Vector2 MousePos => ((Vector2f)Mouse.GetPosition(GameStateManager.Window.window)).ToSysNumVector();

        internal static Dictionary<Keys, bool> keyStateWasPressedToUpdate = new Dictionary<Keys, bool>();
        internal static Dictionary<Keys, bool> keyStateWasReleasedToUpdate = new Dictionary<Keys, bool>();
        internal static Dictionary<MouseButton, bool> mouseButtonStateWasPressedToUpdate = new Dictionary<MouseButton, bool>();
        internal static Dictionary<MouseButton, bool> mouseButtonStateWasReleasedToUpdate = new Dictionary<MouseButton, bool>();
        
        internal static Dictionary<Keys, bool> keyStateWasPressed = new Dictionary<Keys, bool>();
        internal static Dictionary<Keys, bool> keyStateWasReleased = new Dictionary<Keys, bool>();
        internal static Dictionary<MouseButton, bool> mouseButtonStateWasPressed = new Dictionary<MouseButton, bool>();
        internal static Dictionary<MouseButton, bool> mouseButtonStateWasReleased = new Dictionary<MouseButton, bool>();

        
        public static Vector2 MouseWorldPos => 
            GameStateManager.Window.window.MapPixelToCoords(Mouse.GetPosition(GameStateManager.Window.window)).ToSysNumVector();
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
                keyStateWasReleasedToUpdate[key] = false;
                keyStateWasPressedToUpdate[key] = false;
            }
            
            foreach (MouseButton key in Enum.GetValues(typeof(MouseButton)))
            {
                mouseButtonStateWasPressedToUpdate[key] = false;
                mouseButtonStateWasReleasedToUpdate[key] = false;
            }
        }
        
        /// <summary>
        /// Poll if a key was pressed this frame.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        public static bool GetKeyPressed(Keys key)
        {
            return Keyboard.IsKeyPressed((Keyboard.Key)key);
        }

        public static bool GetKeyDown(Keys key)
        {
            bool keyPressed = Keyboard.IsKeyPressed((Keyboard.Key)key);
            if (keyPressed && !keyStateWasPressed[key])
            {
                keyStateWasPressedToUpdate[key] = true;
                return true;
            }
            keyStateWasPressedToUpdate[key] = keyPressed;
            return false;
        }

        public static bool GetKeyUp(Keys key)
        {
            bool keyPressed = Keyboard.IsKeyPressed((Keyboard.Key)key);
            if (!keyPressed && keyStateWasReleased[key])
            {
               keyStateWasReleasedToUpdate[key] = false;
                return true;
            }
            
            keyStateWasReleasedToUpdate[key] = keyPressed;
            
            return false;
        }

        public static bool GetMousePress(MouseButton mouseButton)
        {
            return Mouse.IsButtonPressed((Mouse.Button)mouseButton);
        }
        
        public static bool GetMouseButtonDown(MouseButton mouseButton)
        {
            bool mouseButtonPressed = Mouse.IsButtonPressed((Mouse.Button)mouseButton);
            if (mouseButtonPressed && !mouseButtonStateWasPressed[mouseButton])
            {
                mouseButtonStateWasPressedToUpdate[mouseButton] = true;
                return true;
            }
            mouseButtonStateWasPressedToUpdate[mouseButton] = mouseButtonPressed;
            return false;
        }

        public static bool GetMouseButtonUp(MouseButton mouseButton)
        {
            bool mouseButtonPressed = Keyboard.IsKeyPressed((Keyboard.Key)mouseButton);
            if (!mouseButtonPressed && mouseButtonStateWasReleased[mouseButton])
            {
                mouseButtonStateWasReleasedToUpdate[mouseButton] = false;
                return true;
            }
            
            mouseButtonStateWasReleasedToUpdate[mouseButton] = mouseButtonPressed;
            
            return false;
        }

        internal static void Update()
        {
            keyStateWasPressed = new Dictionary<Keys, bool>(keyStateWasPressedToUpdate);
            keyStateWasReleased = new Dictionary<Keys, bool>(keyStateWasReleasedToUpdate);
            mouseButtonStateWasPressed = new Dictionary<MouseButton, bool>(mouseButtonStateWasPressedToUpdate);
            mouseButtonStateWasReleased = new Dictionary<MouseButton, bool>(mouseButtonStateWasReleasedToUpdate);
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