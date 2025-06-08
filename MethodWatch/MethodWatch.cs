using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace MethodWatch;

/// <summary>
/// Provides manual method timing functionality.
/// </summary>
public static class MethodWatch
{
    private static ILogger? _logger;
    private static bool _enableStatistics = false;
    private static readonly ConcurrentDictionary<string, MethodStats> _stats = new();

    public static void Initialize(ILoggerFactory loggerFactory, bool enableStatistics = false)
    {
        _logger = loggerFactory.CreateLogger("MethodWatch");
        _enableStatistics = enableStatistics;
    }

    public static bool IsStatisticsEnabled() => _enableStatistics;

    public static void RecordExecution(string methodName, long executionTime, long threshold, bool isSuccess, string? error = null)
    {
        if (!_enableStatistics) return;

        var stats = _stats.GetOrAdd(methodName, _ => new MethodStats { Threshold = threshold });
        stats.TotalExecutions++;
        stats.TotalTime += executionTime;
        stats.LastExecutionTime = executionTime;
        stats.LastExecution = DateTime.UtcNow;

        if (executionTime < stats.MinTime) stats.MinTime = executionTime;
        if (executionTime > stats.MaxTime) stats.MaxTime = executionTime;
        if (executionTime > threshold) stats.ExceededThresholdCount++;

        if (!isSuccess)
        {
            stats.TotalFailures++;
            stats.LastError = error;
        }
    }

    public static MethodStats? GetStats(string methodName)
    {
        return _enableStatistics ? _stats.GetValueOrDefault(methodName) : null;
    }

    public static MethodStats[] GetAllStats()
    {
        return _enableStatistics ? _stats.Values.ToArray() : Array.Empty<MethodStats>();
    }

    public static void ClearStats()
    {
        if (_enableStatistics)
        {
            _stats.Clear();
        }
    }

    /// <summary>
    /// Measures the execution time of a code block.
    /// </summary>
    /// <param name="methodName">The name of the method to measure.</param>
    /// <param name="threshold">Optional threshold in milliseconds. If execution time exceeds this threshold, it will be marked as slow.</param>
    /// <returns>A disposable object that measures the execution time when disposed.</returns>
    /// <example>
    /// <code>
    /// using (MethodWatch.Measure("CustomOperation", 100))
    /// {
    ///     // Your code here
    /// }
    /// </code>
    /// </example>
    public static IDisposable Measure(string methodName, long threshold = 0)
    {
        return new MethodWatchScope(methodName, threshold);
    }

    private class MethodWatchScope : IDisposable
    {
        private readonly string _methodName;
        private readonly long _thresholdMilliseconds;
        private readonly Stopwatch _stopwatch;
        private bool _isException;

        public MethodWatchScope(string methodName, long thresholdMilliseconds)
        {
            _methodName = methodName;
            _thresholdMilliseconds = thresholdMilliseconds;
            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            var elapsed = _stopwatch.ElapsedMilliseconds;
            
            // Record statistics only if enabled
            if (_enableStatistics)
            {
                RecordExecution(_methodName, elapsed, _thresholdMilliseconds, !_isException);
            }
            
            Console.WriteLine($"MethodWatch: {_methodName} completed in {elapsed}ms (Threshold: {_thresholdMilliseconds}ms)");
        }

        public void SetException()
        {
            _isException = true;
        }
    }
}

public class MethodStats
{
    public long TotalExecutions { get; set; }
    public long TotalFailures { get; set; }
    public long TotalTime { get; set; }
    public long MinTime { get; set; } = long.MaxValue;
    public long MaxTime { get; set; }
    public long LastExecutionTime { get; set; }
    public DateTime LastExecution { get; set; }
    public string? LastError { get; set; }
    public long Threshold { get; set; }
    public long ExceededThresholdCount { get; set; }
} 