using System.Collections.Concurrent;

namespace MethodWatch;

/// <summary>
/// Collects and provides statistics about method execution times.
/// </summary>
public static class MethodWatchStatistics
{
    private static readonly ConcurrentDictionary<string, MethodStats> _stats = new();

    private class MethodStats
    {
        public string ClassName { get; set; } = "";
        public string MethodName { get; set; } = "";
        public string? CustomName { get; set; }
        public long TotalExecutions { get; set; }
        public long TotalTime { get; set; }
        public long MinTime { get; set; } = long.MaxValue;
        public long MaxTime { get; set; }
        public long FailedExecutions { get; set; }
        public DateTime LastExecuted { get; set; }
        public long? ThresholdMilliseconds { get; set; }
    }

    /// <summary>
    /// Records a method execution in the statistics.
    /// </summary>
    public static void RecordExecution(string className, string methodName, long elapsedMilliseconds, bool isException, string? customName = null, long? thresholdMilliseconds = null)
    {
        var key = $"{className}.{methodName}";
        var stats = _stats.GetOrAdd(key, _ => new MethodStats 
        { 
            ClassName = className,
            MethodName = methodName,
            CustomName = customName,
            ThresholdMilliseconds = thresholdMilliseconds
        });

        stats.TotalExecutions++;
        stats.TotalTime += elapsedMilliseconds;
        stats.MinTime = Math.Min(stats.MinTime, elapsedMilliseconds);
        stats.MaxTime = Math.Max(stats.MaxTime, elapsedMilliseconds);
        stats.LastExecuted = DateTime.UtcNow;
        
        if (isException)
        {
            stats.FailedExecutions++;
        }
    }

    /// <summary>
    /// Gets the current statistics for all recorded methods.
    /// </summary>
    public static IEnumerable<MethodStatistics> GetStatistics()
    {
        return _stats.Select(kvp => new MethodStatistics
        {
            ClassName = kvp.Value.ClassName,
            MethodName = kvp.Value.MethodName,
            CustomName = kvp.Value.CustomName,
            TotalExecutions = kvp.Value.TotalExecutions,
            AverageTime = kvp.Value.TotalExecutions > 0 ? kvp.Value.TotalTime / kvp.Value.TotalExecutions : 0,
            MinTime = kvp.Value.MinTime == long.MaxValue ? 0 : kvp.Value.MinTime,
            MaxTime = kvp.Value.MaxTime,
            FailedExecutions = kvp.Value.FailedExecutions,
            LastExecuted = kvp.Value.LastExecuted,
            ThresholdMilliseconds = kvp.Value.ThresholdMilliseconds
        });
    }

    /// <summary>
    /// Clears all collected statistics.
    /// </summary>
    public static void Clear()
    {
        _stats.Clear();
    }
}

/// <summary>
/// Represents statistics for a method.
/// </summary>
public class MethodStatistics
{
    public string ClassName { get; set; } = "";
    public string MethodName { get; set; } = "";
    public string? CustomName { get; set; }
    public long TotalExecutions { get; set; }
    public long AverageTime { get; set; }
    public long MinTime { get; set; }
    public long MaxTime { get; set; }
    public long FailedExecutions { get; set; }
    public DateTime LastExecuted { get; set; }
    public long? ThresholdMilliseconds { get; set; }
} 