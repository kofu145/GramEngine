using SFML.System;
using System.Numerics;
using SFML.Graphics;

namespace EirEngine.Core;

public static class Utils
{
    public static Vector2f ToSFMLVector(this Vector2 vector)
    {
        return new Vector2f(vector.X, vector.Y);
    }
    
    public static Vector3f ToSFMLVector(this Vector3 vector)
    {
        return new Vector3f(vector.X, vector.Y, vector.Z);
    }
    
    public static Vector2 ToSysNumVector(this Vector2f vector)
    {
        return new Vector2(vector.X, vector.Y);
    }
    
    public static Vector3 ToSysNumVector(this Vector3f vector)
    {
        return new Vector3(vector.X, vector.Y, vector.Z);
    }

    public static Vector4 ColorToVector4(this SFML.Graphics.Color color)
    {
        return new System.Drawing.Color()
    }
}