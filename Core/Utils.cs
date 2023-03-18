using SFML.System;
using System.Numerics;

namespace EirEngine.Core;

public static class Utils
{
    public static Vector2f toSFMLVector(this Vector2 vector)
    {
        return new Vector2f(vector.X, vector.Y);
    }
    
    public static Vector3f toSFMLVector(this Vector3 vector)
    {
        return new Vector3f(vector.X, vector.Y, vector.Z);
    }
    
    public static Vector2 toSysNumVector(this Vector2f vector)
    {
        return new Vector2(vector.X, vector.Y);
    }
    
    public static Vector3 toSysNumVector(this Vector3f vector)
    {
        return new Vector3(vector.X, vector.Y, vector.Z);
    }
}