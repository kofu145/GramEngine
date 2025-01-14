﻿namespace GramEngine.Core;

public struct WindowSettings
{
    public string WindowTitle;
    public uint Width;
    public uint Height;
    public int GlobalXOffset;
    public int GlobalYOffset;
    public bool ShowColliders;

    public int ColliderCellOffset;
    // This one is purely for demonstrating/comparing efficiency.
    public bool NaiveCollision;
    public bool ShowFPS;
    
    public uint BaseWindowHeight;
    public uint BaseWindowWidth;
    public bool SpriteCulling;

    public WindowSettings()
    {
        WindowTitle = "GramEngine Window";
        Width = 1280;
        Height = 720;
        GlobalXOffset = 0;
        GlobalYOffset = 0;
        ShowColliders = false;
        NaiveCollision = false;
        ColliderCellOffset = 100;
        BaseWindowHeight = 1280;
        BaseWindowWidth = 720;
        ShowFPS = false;
        SpriteCulling = false;
    }
    
}