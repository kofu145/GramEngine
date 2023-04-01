using System.Numerics;

namespace GramEngine.Core;

public class MathUtil
{
    public static float Lerp(float p1, float p2, float t)
    {
        return p1 * (1 - t) + p2 * t;
    }

    public static Vector2 Lerp(Vector2 v1, Vector2 v2, float t)
    {
        return new Vector2(Lerp(v1.X, v2.X, t), Lerp(v1.Y, v2.Y, t));
    }

    public static float RadToDeg(float radians)
    {
        return 180 / (float)Math.PI * radians;
    }
    public static double RadToDeg(double radians)
    {
        return 180 / Math.PI * radians;
    }
    
    public static float DegToRad(float degrees)
    {
        return (float)Math.PI / 180 * degrees;
    }
    
    public static double DegToRad(double degrees)
    {
        return Math.PI / 180 * degrees;
    }
    /// <summary>
    /// Calculates a position along a quadratic bezier curve that outputs a value based on the given variable for t
    /// along the given control points, taken as <see cref="Vector2"/>.
    /// </summary>
    /// <param name="t">A normalized value representing the change in time (elapsed position) of the bezier curve.</param>
    /// <remarks>
    /// Use <see cref="InverseLerp"/> to get a normalized value from any point from a defined
    /// interval, if your changing parameter is not normalized.
    /// </remarks>
    /// <param name="p0">A <see cref="Vector2"/> representing the first control point of the curve.</param>
    /// <param name="p1">A <see cref="Vector2"/> representing the second control point of the curve.</param>
    /// <param name="p2">A <see cref="Vector2"/> representing the third control point of the curve.</param>
    /// <returns>A <see cref="Vector2"/> result position along the curve, based on t.</returns>
    public static Vector2 QuadraticBezierCurve(float t, Vector2 p0, Vector2 p1, Vector2 p2) 
    {
        return p1 + ((1 - t) * (1 - t)) * (p0 - p1) + (t * t) * (p2 - p1);
    }

    /// <summary>
    /// Calculates a position along a quadratic bezier curve that outputs a value based on the given variable for t
    /// along the given control points, taken as <see cref="Vector2"/>.
    /// </summary>
    /// <param name="t">A normalized value representing the change in time (elapsed position) of the bezier curve.
    /// </param>
    /// <remarks>
    /// Use <see cref="InverseLerp"/> to get a normalized value from any point from a defined
    /// interval, if your changing parameter is not normalized.
    /// </remarks>
    /// <param name="p0">A <see cref="Vector2"/> representing the first control point of the curve.</param>
    /// <param name="p1">A <see cref="Vector2"/> representing the second control point of the curve.</param>
    /// <param name="p2">A <see cref="Vector2"/> representing the third control point of the curve.</param>
    /// <param name="p3">A <see cref="Vector2"/> representing the fourth control point of the curve.</param>
    /// <returns>A <see cref="Vector2"/> result position along the curve, based on t.</returns>
    public static Vector2 CubicBezierCurve(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3) 
    {
        return (1 - t) * QuadraticBezierCurve(t, p0, p1, p2) + t * QuadraticBezierCurve(t, p1, p2, p3);
    }

    /// <summary>
    /// Same thing as <see cref="CubicBezierCurve"/>, except the formula is calculated directly, rather than through
    /// calling <see cref="QuadraticBezierCurve"/>. Should be identical in functionality.
    /// </summary>
    /// <param name="t">A normalized value representing the change in time (elapsed position) of the bezier curve.</param>
    /// <remarks>
    /// Use <see cref="InverseLerp"/> to get a normalized value from any point from a defined
    /// interval, if your changing parameter is not normalized.
    /// </remarks>
    /// <param name="p0">A <see cref="Vector2"/> representing the first control point of the curve.</param>
    /// <param name="p1">A <see cref="Vector2"/> representing the second control point of the curve.</param>
    /// <param name="p2">A <see cref="Vector2"/> representing the third control point of the curve.</param>
    /// <param name="p3">A <see cref="Vector2"/> representing the fourth control point of the curve.</param>
    /// <returns>A <see cref="Vector2"/> result position along the curve, based on t.</returns>
    public static Vector2 ExplicitCubicBezierCurve(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return ((1 - t) * (1 - t) * (1 - t)) * p0 + 3 * ((1 - t) * (1 - t)) * t * p1 + 3 * ((1 - t) * (1 - t)) * p2 + (t * t * t) * p3;
    }

    /// <summary>
    /// An extremely cheap, simple way to estimate the arc length, taken as the average between the chord and control net.
    /// </summary>
    /// <remarks>See here: https://stackoverflow.com/questions/29438398/cheap-way-of-calculating-cubic-bezier-length</remarks>
    /// <param name="p0">A <see cref="Vector2"/> representing the first control point of the curve.</param>
    /// <param name="p1">A <see cref="Vector2"/> representing the second control point of the curve.</param>
    /// <param name="p2">A <see cref="Vector2"/> representing the third control point of the curve.</param>
    /// <param name="p3">A <see cref="Vector2"/> representing the fourth control point of the curve.</param>
    /// <returns>An estimated arclength of the bezier curve.</returns>
    public static float GetBezierArcLength(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        var chord = (p3 - p0).Length();
        var controlNet = (p0 - p1).Length() + (p2 - p1).Length() + (p3 - p2).Length();

        return (controlNet + chord) / 2;
    }

    /// <summary>
    /// Returns a normalized point (0 - 1) representative of a given interval.
    /// </summary>
    /// <example>
    /// For example, an interval from 200 to 300, given the point 250, would return the normalized value .5.
    /// </example>
    /// <param name="min">The minimum value of the interval.</param>
    /// <param name="max">The maximum value of the interval.</param>
    /// <param name="point">The given point within the interval that is to be normalized.</param>
    /// <returns>A normalized point representing the given point in the interval.</returns>
    public static float InverseLerp(float min, float max, float point)
    {
        return (point - min) / (max - min);
    }
    
}