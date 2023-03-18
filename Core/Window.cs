using SFML.Graphics;
using SFML.Window;
using EirEngine.ECS.Components;
using SFML.System;

namespace EirEngine.Core;

public class Window
{
    // set values that are typical of a window, probably through some struct
    // can use them to manipulate stuff about the window, like the style

    private String windowTitle;
    
    // change this (access modifier) later so we can access these fields from anywhere
    private uint width;
    private uint height;

    private GameTime gameTime;
    
    public Window(IGameState initialGameState, string windowTitle = "Eir Engine Window", uint width = 1280, uint height = 720)
    {
        this.windowTitle = windowTitle;
        this.width = width;
        this.height = height;
        gameTime = new GameTime(0, 0);
        GameStateManager.Instance.AddScreen(initialGameState);
    }

    
    public void Run()
    {
        var style = SFML.Window.Styles.Default;
        var mode = new SFML.Window.VideoMode(width, height);
        var window = new SFML.Graphics.RenderWindow(mode, this.windowTitle, style);
        window.KeyPressed += Window_KeyPressed;
        window.Resized += Window_Resized;
        window.Closed += Window_Closed;

        /*var circle = new SFML.Graphics.CircleShape(100f)
        {
            FillColor = SFML.Graphics.Color.Blue
        };*/

        // Within the loop, the gamestate manager will pull up the current top of the stack
        // The top stack state will be the one being rendered within the window
        
        GameStateManager.Instance.OnLoad();
        
        // Start the game loop - Each iteration of this is one frame
        while (window.IsOpen)
        {
            // I hate we have to call to get current scene every frame lol
            var currentGameState = GameStateManager.Instance.GetScreen();
            var currentScene = currentGameState.GameScene;
            
            // Process events
            window.DispatchEvents();
            
            window.Clear();
            
            currentScene.UpdateEntities();
            currentGameState.Update(gameTime);
            // may be more delay from onload to now vs between frames?
            gameTime.UpdateTime();
            
            var spriteEntities = currentScene.Entities
                .Where(e => e.HasComponent<ECS.Components.Sprite>());

            foreach (var entity in spriteEntities)
            {
                // draw call based on whatever sfml implementations

                // should try to get rid of this getcomp call in future whenever possible
                var sprite = entity.GetComponent<ECS.Components.Sprite>();

                // We set the sprite render transform to be the same as the entity's
                
                // shorthand for easy writing
                var sfmlVectorPos = entity.Transform.Position.toSFMLVector();
                sprite.sfmlSprite.Position = new Vector2f(sfmlVectorPos.X, sfmlVectorPos.Y);
                sprite.sfmlSprite.Rotation = entity.Transform.Rotation.Z;
                sprite.sfmlSprite.Scale = entity.Transform.Scale.toSFMLVector();
                
                if (sprite.Enabled)
                {
                    // for z ordering, sort along 
                    window.Draw(sprite.sfmlSprite);
                }
            }
            // do we even need this? everything is abstracted away anyway
            currentGameState.Draw();


            // Finally, display the rendered frame on screen
            window.Display();
        }
    }

    /// <summary>
    /// Function called when a key is pressed
    /// </summary>
    private void Window_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
    {
        var window = (SFML.Window.Window)sender;
        if (e.Code == Keyboard.Key.Escape)
        {
            window.Close();
        }
    }

    private void Window_Resized(object sender, SFML.Window.SizeEventArgs e)
    {
        var window = (RenderWindow)sender;
        window.SetView(getLetterboxView(window.GetView(), window.Size.X, window.Size.Y));
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        // TODO: Should be calling Dispose() methods here
        RenderWindow window = (RenderWindow)sender;
        window.Close();
    }
    
    // Neat little function to convert to letterbox view, using window width/height
    // source: https://github.com/SFML/SFML/wiki/Source%3A-Letterbox-effect-using-a-view
    private View getLetterboxView(View view, uint width, uint height)
    {
        // Compares the aspect ratio of the window to the aspect ratio of the view,
        // and sets the view's viewport accordingly in order to archieve a letterbox effect.
        // A new view (with a new viewport set) is returned.

        float windowRatio = width / (float) height;
        float viewRatio = view.Size.X / (float) view.Size.Y;
        float sizeX = 1;
        float sizeY = 1;
        float posX = 0;
        float posY = 0;

        bool horizontalSpacing = true;
        if (windowRatio < viewRatio)
            horizontalSpacing = false;

        // If horizontalSpacing is true, the black bars will appear on the left and right side.
        // Otherwise, the black bars will appear on the top and bottom.

        if (horizontalSpacing) {
            sizeX = viewRatio / windowRatio;
            posX = (1 - sizeX) / 2f;
        }

        else {
            sizeY = windowRatio / viewRatio;
            posY = (1 - sizeY) / 2f;
        }

        view.Viewport = new FloatRect(posX, posY, sizeX, sizeY );

        return view;
        
    }
}