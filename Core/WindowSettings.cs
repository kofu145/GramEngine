namespace GramEngine.Core;

public struct WindowSettings
{
    public string WindowTitle;
    public uint Width;
    public uint Height;
    public int GlobalXOffset;
    public int GlobalYOffset;
    public bool ShowColliders;

    public WindowSettings()
    {
        WindowTitle = "GramEngine Window";
        Width = 1280;
        Height = 720;
        GlobalXOffset = 0;
        GlobalYOffset = 0;
        ShowColliders = false;
    }
    
}