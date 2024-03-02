using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GramEngine.Core;

public class GameTime
{
    /// <summary>
    /// The time between the last frame to the current one. (Unused)
    /// </summary>
    private TimeSpan deltaTime;

    /// <summary>
    /// The time between the last frame to the current one.
    /// </summary>
    public float DeltaTime;
    
    /// <summary>
    /// Total time from the start of the game.
    /// </summary>
    public TimeSpan TotalTime { get; private set; }

    // more accurate measurement of total gametime
    private Stopwatch stopwatch;

    //TODO: helper methods as needed
    
    internal GameTime()
    {
        deltaTime = new TimeSpan();
        TotalTime = new TimeSpan();
        stopwatch = new Stopwatch();
        stopwatch.Start();
    }

    internal GameTime(double initialDeltaTime, double initialTotalTime)
    {
        deltaTime = TimeSpan.FromSeconds(initialDeltaTime);
        TotalTime = TimeSpan.FromSeconds(initialTotalTime);
        stopwatch = new Stopwatch();
        stopwatch.Start();
    }

    internal void UpdateTime()
    {
        deltaTime = stopwatch.Elapsed;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            DeltaTime = (float)((double)stopwatch.Elapsed.Ticks / Stopwatch.Frequency);
        else
            DeltaTime = (float)((double)stopwatch.ElapsedTicks / Stopwatch.Frequency);
        TotalTime += deltaTime;
        stopwatch.Restart();
    }
    
    internal void UpdateTime(double seconds)
    {
        deltaTime = TimeSpan.FromSeconds(seconds);
        //totalTime = stopwatch.Elapsed;
        TotalTime += deltaTime;
    }

}