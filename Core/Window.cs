using GramEngine.ECS;
using SFML.Graphics;
using SFML.Window;
using GramEngine.ECS.Components;
using SFML.System;

namespace GramEngine.Core;

public class Window
{
    // set values that are typical of a window, probably through some struct
    // can use them to manipulate stuff about the window, like the style

    public String WindowTitle
    {
        get => windowTitle;
        set => window.SetTitle(windowTitle = value);
    }
    private String windowTitle;
    
    public uint Width { get => window.Size.X; }
    public uint Height { get => window.Size.Y; }

    private GameTime gameTime;
    private Styles style;
    private VideoMode mode;
    private SFML.Graphics.RenderWindow window;
    public readonly WindowSettings settings;
    
    public Window(IGameState initialGameState, WindowSettings settings)
    {
        gameTime = new GameTime();

        // TODO: come up with some smarter implementation for this
        // just have to pray no one makes more than one window
        GameStateManager.Window = this;
        GameStateManager.GameTime = gameTime;

        this.settings = settings;
        style = SFML.Window.Styles.Default;
        mode = new SFML.Window.VideoMode(settings.Width, settings.Height);
        window = new SFML.Graphics.RenderWindow(mode, settings.WindowTitle, style);
        GameStateManager.AddScreen(initialGameState);
    }

    
    public void Run()
    {
        
        window.KeyPressed += Window_KeyPressed;
        window.Resized += Window_Resized;
        window.Closed += Window_Closed;
        
        //window.SetFramerateLimit(60);
        //window.SetVerticalSyncEnabled(true);

        /*var circle = new SFML.Graphics.CircleShape(100f)
        {
            FillColor = SFML.Graphics.Color.Blue
        };*/

        // Within the loop, the gamestate manager will pull up the current top of the stack
        // The top stack state will be the one being rendered within the window
        
        GameStateManager.OnLoad();
        DateTime lastTime = new DateTime();
        float framesRendered = 0;
        GameStateManager.GetScreen().GameScene.AddEntity(
            new Entity().AddComponent(new TextComponent("", "./Content/square.ttf", 24))
            );
        // Start the game loop - Each iteration of this is one frame
        while (window.IsOpen)
        {
            // I hate we have to call to get current scene every frame lol
            var currentGameState = GameStateManager.GetScreen();
            var currentScene = currentGameState.GameScene;
            
            // Process events
            window.DispatchEvents();
            
            window.Clear();
            
            currentScene.UpdateEntitiesList();

            currentGameState.Update(gameTime);
            // may be more delay from onload to now vs between frames?
            gameTime.UpdateTime();
            //Console.WriteLine(gameTime.DeltaTime);
            
            var textEntities = currentScene.Entities
                .Where(e => e.HasComponent<ECS.Components.TextComponent>());
            // small fps calc
            framesRendered++;
            if ((DateTime.Now - lastTime).TotalSeconds >= 1)
            {
                var fps = framesRendered;
                framesRendered = 0;
                lastTime = DateTime.Now;
                //Console.WriteLine(fps);
                textEntities.First().GetComponent<TextComponent>().Text = "FPS: " + fps.ToString();
            }

            currentScene.UpdateEntities(gameTime);

            var renderableEntities = currentScene.Entities
                .Where(e => 
                    e.HasComponent<ECS.Components.Sprite>() ||
                    e.HasComponent<RenderRect>() ||
                    e.HasComponent<RenderCircle>()
                ).OrderBy(entity => entity.Transform.Position.Z);
            
            foreach (var entity in renderableEntities)
            {
                // this cleaner implementation here is super buggy and weird, maybe will actually try to get it working
                // sometime in the future.
                /*
                    // draw call based on whatever sfml implementations
    
                    // should try to get rid of this getcomp call in future whenever possible
                    
                    var renderables = entity.GetRenderable();
                    if (renderables.Count > 0)
                    {
                        foreach (IComponent component in renderables)
                        {
                            if (component.Enabled)
                            {
                                Console.WriteLine("component!");
                                IRenderable renderable = (IRenderable)component;
                                renderable.GetTransformTarget();
                                //renderable.Draw(window);
                                window.Draw(renderable.GetRenderTarget());
                            }
                        }
                    } */
                Draw(entity);
            }
                
            // debug rendering
            foreach (var entity in textEntities)
            {
                var textComponent = entity.GetComponent<TextComponent>();
                // shorthand for easy writing
                var sfmlVectorPos = entity.Transform.Position.ToSFMLVector();
                textComponent.text.Position = new Vector2f(sfmlVectorPos.X, sfmlVectorPos.Y);
                textComponent.text.Rotation = entity.Transform.Rotation.Z;
                textComponent.text.Scale = entity.Transform.Scale.ToSFMLVector();

                if (textComponent.Enabled)
                {
                    window.Draw(textComponent.text);
                }
            }
            
            var circleColliderEntities = currentScene.Entities
                .Where(e => e.HasComponent<ECS.Components.CircleCollider>());

            foreach (var entity in circleColliderEntities)
            {
                var circleCollider = entity.GetComponent<CircleCollider>();
                var sfmlVectorPos = entity.Transform.Position.ToSFMLVector();
                circleCollider.circleShape.Position = new Vector2f(
                    sfmlVectorPos.X + settings.GlobalXOffset, 
                    sfmlVectorPos.Y + settings.GlobalYOffset
                );                circleCollider.circleShape.Rotation = entity.Transform.Rotation.Z;
                //circleCollider.circleShape.Scale = entity.Transform.Scale.ToSFMLVector();
                if (settings.ShowColliders)
                {
                    window.Draw(circleCollider.circleShape);                  
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
        /*if (e.Code == Keyboard.Key.Escape)
        {
            window.Close();
        }*/
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

    private void Draw(Entity entity)
    {
        if (entity.HasComponent<ECS.Components.Sprite>())
        {
            var sprite = entity.GetComponent<ECS.Components.Sprite>();

            // We set the sprite render transform to be the same as the entity's
            // shorthand for easy writing
            var sfmlVectorPos = entity.Transform.Position.ToSFMLVector();
            sprite.sfmlSprite.Position = new Vector2f(
                sfmlVectorPos.X + settings.GlobalXOffset, 
                sfmlVectorPos.Y + settings.GlobalYOffset
                );
            sprite.sfmlSprite.Rotation = entity.Transform.Rotation.Z;
            sprite.sfmlSprite.Scale = entity.Transform.Scale.ToSFMLVector();

            if (sprite.Enabled)
            {
                
                // for z ordering, sort along 
                window.Draw(sprite.sfmlSprite);
            }
        }
        
        if (entity.HasComponent<RenderRect>())
        {
            var rect = entity.GetComponent<RenderRect>();

            // We set the sprite render transform to be the same as the entity's
            // shorthand for easy writing
            var sfmlVectorPos = entity.Transform.Position.ToSFMLVector();
            rect.rectangleShape.Position = new Vector2f(
                sfmlVectorPos.X + settings.GlobalXOffset, 
                sfmlVectorPos.Y + settings.GlobalYOffset
            );
            rect.rectangleShape.Rotation = entity.Transform.Rotation.Z;
            rect.rectangleShape.Scale = entity.Transform.Scale.ToSFMLVector();
        
            if (rect.Enabled)
                // for z ordering, sort along 
                window.Draw(rect.rectangleShape);
        }

        if (entity.HasComponent<RenderCircle>())
        {
            var circle = entity.GetComponent<RenderCircle>();

            // We set the sprite render transform to be the same as the entity's
            // shorthand for easy writing
            var sfmlVectorPos = entity.Transform.Position.ToSFMLVector();
            circle.circleShape.Position = new Vector2f(
                sfmlVectorPos.X + settings.GlobalXOffset, 
                sfmlVectorPos.Y + settings.GlobalYOffset
            );
            circle.circleShape.Rotation = entity.Transform.Rotation.Z;
            //circle.circleShape.Scale = entity.Transform.Scale.ToSFMLVector();
        
            if (circle.Enabled)
                // for z ordering, sort along 
                window.Draw(circle.circleShape);
        }
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