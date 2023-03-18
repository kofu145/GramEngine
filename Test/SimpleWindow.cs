using SFML.Graphics;
using SFML.Window;

namespace EirEngine.Test;

// Simple window provided from C# sfml binding examples
public class SimpleWindow
{
    public void Run()
    {
        uint width = 800;
        uint height = 600;
        var mode = new SFML.Window.VideoMode(width, height);
        var style = SFML.Window.Styles.Default;
        var window = new SFML.Graphics.RenderWindow(mode, "SFML works!", style);
        window.KeyPressed += Window_KeyPressed;

        window.Resized += Window_Resized;

        var circle = new SFML.Graphics.CircleShape(100f)
        {
            FillColor = SFML.Graphics.Color.Blue
        };

        // Start the game loop
        while (window.IsOpen)
        {
            window.Clear();
            // Process events
            
            window.DispatchEvents();
            
            
            window.Draw(circle);

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
        if (e.Code == SFML.Window.Keyboard.Key.Escape)
        {
            window.Close();
        }
    }

    private void Window_Resized(object sender, SFML.Window.SizeEventArgs e)
    {
        var window = (SFML.Graphics.RenderWindow)sender;
        window.SetView(getLetterboxView(window.GetView(), window.Size.X, window.Size.Y));
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