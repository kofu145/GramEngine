namespace EirEngine.Test;

// Simple window provided from C# sfml binding examples
class SimpleWindow
{
    public void Run()
    {
        var mode = new SFML.Window.VideoMode(800, 600);
        var window = new SFML.Graphics.RenderWindow(mode, "SFML works!");
        window.KeyPressed += Window_KeyPressed;

        var circle = new SFML.Graphics.CircleShape(100f)
        {
            FillColor = SFML.Graphics.Color.Blue
        };

        // Start the game loop
        while (window.IsOpen)
        {
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
}