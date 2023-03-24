using GramEngine.Core;
using SFML.Graphics;
using Color = System.Drawing.Color;

namespace GramEngine.ECS.Components;

/// <summary>
/// Wrapper component for <see cref="SFML.Graphics.Text"/>.
/// </summary>
public class TextComponent : Component
{
    ////////////////////////////////////////////////////////////
    /// <summary>
    /// Taken straight from SFML.Graphics.Text.Styles to represent styles w/out having to touch the library directly
    /// </summary>
    ////////////////////////////////////////////////////////////
    [Flags]
    public enum Styles
    {
        /// <summary>Regular characters, no style</summary>
        Regular = 0,

        /// <summary>Bold characters</summary>
        Bold = 1 << 0,

        /// <summary>Italic characters</summary>
        Italic = 1 << 1,

        /// <summary>Underlined characters</summary>
        Underlined = 1 << 2,

        /// <summary>Strike through characters</summary>
        StrikeThrough = 1 << 3
    }

    internal Text text;
    internal string fontPath;

    public string Text
    {
        get => text.DisplayedString;
        set => text.DisplayedString = value;
    }

    public string FontPath
    {
        get => fontPath;
        set => text.Font = new Font(value);
    }

    public int FontSize
    {
        get => (int)text.CharacterSize;
        set => text.CharacterSize = (uint)value;
    }

    public Color FillColor
    {
        get => text.FillColor.ToSysColor();
        set => text.FillColor = value.ToSFMLColor();
    }

    public Styles Style
    {
        get => (Styles)text.Style;
        set => text.Style = (SFML.Graphics.Text.Styles)value;
    }
    
    public TextComponent(string text, string fontPath, int size)
    {
        this.fontPath = fontPath;
        this.text = new Text(text, new Font(fontPath), (uint)size);
    }
    
    public override void Initialize()
    {
    }

    public override void Update(GameTime gameTime)
    {
    }
}