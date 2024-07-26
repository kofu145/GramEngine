using System.Numerics;
using GramEngine.Core.Input;
using GramEngine.ECS;
using SFML.Graphics;
using SFML.Window;
using GramEngine.ECS.Components;
using SFML.System;
using Transform = GramEngine.ECS.Transform;

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
    public readonly WindowSettings settings;
    public Vector2 CameraPosition;
    public System.Drawing.Color BackgroundColor;
    public uint Width { get => window.Size.X; }
    public uint Height { get => window.Size.Y; }
    public float Zoom;
    public bool WindowFocused => windowFocused;

    public event MidTransition OnMidTransition;
    public delegate void MidTransition();

    private String windowTitle;
    private GameTime gameTime;
    private Styles style;
    private VideoMode mode;
    private View mainView;
    private SceneTransition transition;
    private bool windowFocused;
    internal List<DisplayFrame> displayFrames;


    private double transitionTimer;

    private double transitionTarget;
    // internal thanks to InputManager, mouse coords needs relative
    internal SFML.Graphics.RenderWindow window;

    
    public Window(IGameState initialGameState, WindowSettings settings)
    {
        gameTime = new GameTime();

        // TODO: come up with some smarter implementation for this
        // just have to pray no one makes more than one window
        GameStateManager.Window = this;
        GameStateManager.GameTime = gameTime;

        this.settings = settings;
        
        // super quick bandaid fix for base height/win height clamp issues when using settings
        if (settings.BaseWindowHeight != settings.Height || settings.BaseWindowWidth != settings.Width)
        {
            this.settings.BaseWindowHeight = settings.Height;
            this.settings.BaseWindowWidth = settings.Width;
        }
        style = SFML.Window.Styles.Default;
        mode = new SFML.Window.VideoMode(settings.Width, settings.Height);
        window = new SFML.Graphics.RenderWindow(mode, settings.WindowTitle, style);
        CameraPosition = new Vector2();
        mainView = new View(new FloatRect(CameraPosition.ToSFMLVector(),
            new Vector2f(settings.Width, settings.Height)));
        mainView.Zoom(Zoom);
        window.SetView(getLetterboxView(mainView, window.Size.X, window.Size.Y));
        BackgroundColor = System.Drawing.Color.Black;
        displayFrames = new List<DisplayFrame>();
        windowFocused = true;
        transition = SceneTransition.None;
        transitionTimer = 0;
        GameStateManager.AddScreen(initialGameState);
    }

    public void SetTransitionEffect(SceneTransition transition)
    {
        this.transition = transition;
        transitionTimer = 0;
        transitionTarget = .3;
    }

    
    public void Run()
    {
        
        window.KeyPressed += Window_KeyPressed;
        window.KeyReleased += Window_KeyReleased;
        window.Resized += Window_Resized;
        window.Closed += Window_Closed;
        window.LostFocus += Window_LostFocus;
        window.GainedFocus += Window_GainedFocus;
        
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
        
        // TODO: this data needs to be recycled on a per scene basis
        float framesRendered = 0;
        var fpsEntity = new Entity(true);
        fpsEntity.AddComponent(new TextComponent("", "./SourceFiles/square.ttf", 24));
        fpsEntity.IsUIEntity = true;
        fpsEntity.Tag = "FPS";
        if (settings.ShowFPS)
            GameStateManager.GetScreen().GameScene.AddEntity(fpsEntity);
        
        float lowestFPS = int.MaxValue;
        var lowFPSEntity = new Entity(true);
            lowFPSEntity.AddComponent(new TextComponent("", "./SourceFiles/square.ttf", 24));
        lowFPSEntity.Transform.Position.Y += 30;
        lowFPSEntity.IsUIEntity = true;
        lowFPSEntity.Tag = "lowFPS";
        if (settings.ShowFPS)
            GameStateManager.GetScreen().GameScene.AddEntity(lowFPSEntity);
        

        fpsEntity.AddComponent(new FollowCamera(fpsEntity.Transform.Position));
        lowFPSEntity.AddComponent(new FollowCamera(lowFPSEntity.Transform.Position));

        if (!settings.NaiveCollision)
        {
            var circleCollisionManager = new Entity();
            circleCollisionManager.AddComponent(new CircleColliderSystem(100));
            GameStateManager.GetScreen().GameScene.AddEntity(circleCollisionManager);
        }

        // Start the game loop - Each iteration of this is one frame
        while (window.IsOpen)
        {
            // I hate we have to call to get current scene every frame lol
            var currentGameState = GameStateManager.GetScreen();
            var currentScene = currentGameState.GameScene;
            // Process events
            window.DispatchEvents();
            gameTime.UpdateTime();
            window.Clear(BackgroundColor.ToSFMLColor());

            InputManager.Update();

            currentScene.UpdateEntitiesList();
            
            currentGameState.Update(gameTime);
            
            // may be more delay from onload to now vs between frames?
            //Console.WriteLine(gameTime.DeltaTime);
            
            var textEntities = currentScene.EntitiesAndUIEntities
                .Where(e => e.HasComponent<ECS.Components.TextComponent>());

            // TODO: update all new scenes with FPS entity if enabled
            if (settings.ShowFPS)
            {
                fpsEntity = currentScene.FindUIEntityWithTag("FPS");
                lowFPSEntity = currentScene.FindUIEntityWithTag("lowFPS");
                // small fps calc
                framesRendered++;
                if ((DateTime.Now - lastTime).TotalSeconds >= 1)
                {
                    var fps = framesRendered;
                    framesRendered = 0;
                    lastTime = DateTime.Now;
                    //Console.WriteLine(fps);
                    fpsEntity.GetComponent<TextComponent>().Text = "FPS: " + fps.ToString();
                    if (fps < lowestFPS && fps > 10)
                    {
                        lowestFPS = fps;
                        lowFPSEntity.GetComponent<TextComponent>().Text = "Lowest FPS: " + lowestFPS.ToString();
                    }
                
                }
            }
            //lowFPSEntity.GetComponent<TextComponent>().Text = "entities: " + currentScene.Entities.Count;

            currentScene.UpdateEntities(gameTime);

            Render(currentScene);
            
            // do we even need this? everything is abstracted away anyway
            currentGameState.Draw();
            mainView = new View(new FloatRect(CameraPosition.ToSFMLVector(),
                new Vector2f(settings.Width, settings.Height)));
            window.SetView(getLetterboxView(mainView, window.Size.X, window.Size.Y));
            mainView.Zoom(Zoom);

            
            // Scene Transition!
            if (transition != SceneTransition.None)
            {
                switch (transition)
                {
                    case SceneTransition.FadeIn:

                        RectangleShape fade = new RectangleShape();
                        fade.Size = new Vector2f(window.Size.X, window.Size.Y);
                        double currentAlpha = 0;
                        transitionTimer += gameTime.DeltaTime;

                        if (transitionTimer >= transitionTarget)
                        {
                            currentAlpha = 255;
                            OnMidTransition?.Invoke();
                            SetTransitionEffect(SceneTransition.FadeOut);
                        }
                        else
                        {
                            currentAlpha = MathUtil.Lerp(0, 255,
                                transitionTimer / transitionTarget);
                        }
                        fade.FillColor = new Color(0, 0, 0, (byte)currentAlpha);


                        window.Draw(fade);

                        break;
                    case SceneTransition.FadeOut:

                        fade = new RectangleShape();
                        fade.Size = new Vector2f(window.Size.X, window.Size.Y);
                        currentAlpha = 0;
                        transitionTimer += gameTime.DeltaTime;

                        if (transitionTimer >= transitionTarget)
                        {
                            transition = SceneTransition.None;
                        }
                        else
                        {
                            currentAlpha = MathUtil.Lerp(255, 0,
                                transitionTimer / transitionTarget);
                        }
                        fade.FillColor = new Color(0, 0, 0, (byte)currentAlpha);
                        
                        window.Draw(fade);

                        break;
                }
            }

            foreach (var frame in displayFrames)
            {
                var toRender = frame.GetRenderTarget();
                window.Draw(toRender);
                toRender.Dispose();
            }
            Render(currentScene);
            // Finally, display the rendered frame on screen
            window.Display();
        }
    }
    
    private void Render(Scene currentScene)
    {
        var renderableEntities = currentScene.Entities
                .Where(e => 
                    e.HasComponent<ECS.Components.Sprite>() ||
                    e.HasComponent<RenderRect>() ||
                    e.HasComponent<RenderCircle>() ||
                    e.HasComponent<Tilemap>()
                ).OrderBy(entity => entity.Transform.Position.Z);
        var textEntities = currentScene.Entities
            .Where(e => e.HasComponent<ECS.Components.TextComponent>());
        
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
                var sfmlVectorPos = (entity.Transform.Position + textComponent.TextOffset.ToVec3()).ToSFMLVector();
                textComponent.text.Position = new Vector2f(sfmlVectorPos.X, sfmlVectorPos.Y);
                textComponent.text.Rotation = entity.Transform.Rotation.Z;
                if (!textComponent.UseLocalScale)
                    textComponent.text.Scale = entity.Transform.Scale.ToSFMLVector();
                else
                    textComponent.text.Scale = textComponent.LocalScale.ToSFMLVector();

                if (textComponent.Enabled)
                {
                    window.Draw(textComponent.text);
                }
            }
            
            var circleColliderEntities = currentScene.EntitiesAndUIEntities
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
            
            // separate render pass for UI entities, always on top
            
            var renderableUIEntities = currentScene.UIEntities
                .Where(e => 
                    e.HasComponent<ECS.Components.Sprite>() ||
                    e.HasComponent<RenderRect>() ||
                    e.HasComponent<RenderCircle>() ||
                    e.HasComponent<Tilemap>()
                ).OrderBy(entity => entity.Transform.Position.Z);
            var renderableTextUIEntities = currentScene.UIEntities
                .Where(e => e.HasComponent<ECS.Components.TextComponent>());
            
            foreach (var entity in renderableUIEntities)
            {
                Draw(entity);
            }
                
            // debug rendering
            foreach (var entity in renderableTextUIEntities)
            {
                var textComponent = entity.GetComponent<TextComponent>();
                // shorthand for easy writing
                var sfmlVectorPos = (entity.Transform.Position + textComponent.TextOffset.ToVec3()).ToSFMLVector();
                textComponent.text.Position = new Vector2f(sfmlVectorPos.X, sfmlVectorPos.Y);
                textComponent.text.Rotation = entity.Transform.Rotation.Z;
                if (!textComponent.UseLocalScale)
                    textComponent.text.Scale = entity.Transform.Scale.ToSFMLVector();
                else
                    textComponent.text.Scale = textComponent.LocalScale.ToSFMLVector();

                if (textComponent.Enabled)
                {
                    window.Draw(textComponent.text);
                }
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
    
    /// <summary>
    /// Function called when key released
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_KeyReleased(object sender, SFML.Window.KeyEventArgs e)
    {
        var window = (SFML.Window.Window)sender;
        /*if (e.Code == Keyboard.Key.Escape)
        {
            window.Close();
        }*/

        
    }
    
    /// <summary>
    /// Function called when window gains focus
    /// </summary>
    private void Window_GainedFocus(object? sender, EventArgs e)
    {
        var window = (SFML.Window.Window)sender;
        /*if (e.Code == Keyboard.Key.Escape)
        {
            window.Close();
        }*/
        windowFocused = true;

    }
    
    private void Window_LostFocus(object? sender, EventArgs e)
    {
        var window = (SFML.Window.Window)sender;
        /*if (e.Code == Keyboard.Key.Escape)
        {
            window.Close();
        }*/
        windowFocused = false;

    }

    private void Window_Resized(object sender, SFML.Window.SizeEventArgs e)
    {
        var window = (RenderWindow)sender;
        window.SetView(getLetterboxView(mainView, window.Size.X, window.Size.Y));
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        // TODO: Should be calling Dispose() methods here
        RenderWindow window = (RenderWindow)sender;
        GameStateManager.Dispose();
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
                if (settings.SpriteCulling && 
                    entity.Transform.Position.X > -10 && entity.Transform.Position.X < window.Size.X + 10 &&
                    entity.Transform.Position.Y > -10 && entity.Transform.Position.Y < window.Size.Y + 10) 
                    window.Draw(sprite.sfmlSprite);
                else
                {
                    window.Draw(sprite.sfmlSprite);

                }
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
            //rect.rectangleShape.Scale = entity.Transform.Scale.ToSFMLVector();
        
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

        if (entity.HasComponent<Tilemap>())
        {
            var tilemap = entity.GetComponent<Tilemap>();

            // We set the sprite render transform to be the same as the entity's
            // shorthand for easy writing
            var sfmlVectorPos = entity.Transform.Position.ToSFMLVector();
            //rect.rectangleShape.Scale = entity.Transform.Scale.ToSFMLVector();
            
            if (tilemap.Enabled)
            {
                tilemap.Render();
                
                var renderState = new RenderStates(tilemap.tileset);
                renderState.Transform.Translate(tilemap.Transform.Position.ToVec2().ToSFMLVector());
                renderState.Transform.Scale(tilemap.Transform.Scale.ToSFMLVector());
                
                window.Draw(tilemap.mVertices, renderState);
            }
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

        bool horizontalSpacing = !(windowRatio < viewRatio);

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