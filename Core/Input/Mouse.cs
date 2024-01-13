namespace GramEngine.Core.Input;

////////////////////////////////////////////////////////////
/// <summary>
/// Mouse buttons
/// </summary>
////////////////////////////////////////////////////////////
public enum MouseButton
{
    /// <summary>The left mouse button</summary>
    Left,

    /// <summary>The right mouse button</summary>
    Right,

    /// <summary>The middle (wheel) mouse button</summary>
    Middle,

    /// <summary>The first extra mouse button</summary>
    XButton1,

    /// <summary>The second extra mouse button</summary>
    XButton2,

    /// <summary>Keep last -- the total number of mouse buttons</summary>
    ButtonCount
};

////////////////////////////////////////////////////////////
/// <summary>
/// Mouse wheels
/// </summary>
////////////////////////////////////////////////////////////
public enum Wheel
{
    /// <summary>The vertical mouse wheel</summary>
    VerticalWheel,

    /// <summary>The horizontal mouse wheel</summary>
    HorizontalWheel
};