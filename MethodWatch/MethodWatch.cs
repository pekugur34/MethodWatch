using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace MethodWatch;

/// <summary>
/// Provides manual method timing functionality.
/// </summary>
public static class MethodWatch
{
    private static ILogger? _logger;
    private static bool _enableStatistics = true;

    public static void Initialize(ILoggerFactory loggerFactory, bool enableStatistics = true)
    {
        _logger = loggerFactory.CreateLogger("MethodWatch");
        _enableStatistics = enableStatistics;
    }

    public static bool IsStatisticsEnabled() => _enableStatistics;

    /// <summary>
    /// Measures the execution time of a code block.
    /// </summary>
    /// <param name="customName">Optional custom name to identify this measurement in logs. If not provided, the calling method name will be used.</param>
    /// <param name="thresholdMilliseconds">Optional threshold in milliseconds. If execution time exceeds this threshold, it will be marked as slow.</param>
    /// <returns>A disposable object that measures the execution time when disposed.</returns>
    /// <example>
    /// <code>
    /// using (MethodWatch.Measure("CustomOperation", 100))
    /// {
    ///     // Your code here
    /// }
    /// </code>
    /// </example>
    public static IDisposable Measure(string? customName = null, long thresholdMilliseconds = 0)
    {
        // Get the stack trace
        var stackTrace = new StackTrace();
        
        // Look for the first non-MethodWatch frame
        string className = "Unknown";
        string methodName = customName ?? "Unknown";
        
        for (int i = 0; i < stackTrace.FrameCount; i++)
        {
            var frame = stackTrace.GetFrame(i);
            var method = frame?.GetMethod();
            var declaringType = method?.DeclaringType;
            
            if (declaringType != null && declaringType.Namespace != "MethodWatch")
            {
                className = declaringType.Name;
                if (customName == null)
                {
                    methodName = method?.Name ?? "Unknown";
                    // Clean up method name
                    if (methodName.Contains("<>"))
                    {
                        methodName = "AnonymousMethod";
                    }
                    else if (methodName.Contains("__"))
                    {
                        methodName = methodName.Split('_')[0];
                    }
                }
                break;
            }
        }
        
        Console.WriteLine($"MethodWatch: Starting measurement for {className}.{methodName} (Custom: {customName}, Threshold: {thresholdMilliseconds}ms)");
        return new MethodWatchScope(className, methodName, customName, thresholdMilliseconds);
    }

    private class MethodWatchScope : IDisposable
    {
        private readonly string _className;
        private readonly string _methodName;
        private readonly string? _customName;
        private readonly long? _thresholdMilliseconds;
        private readonly Stopwatch _stopwatch;
        private bool _isException;

        public MethodWatchScope(string className, string methodName, string? customName = null, long? thresholdMilliseconds = null)
        {
            _className = className;
            _methodName = methodName;
            _customName = customName;
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
                MethodWatchStatistics.RecordExecution(_className, _methodName, elapsed, _isException, _customName, _thresholdMilliseconds);
            }
            
            Console.WriteLine($"MethodWatch: {_className}.{_methodName} completed in {elapsed}ms (Custom: {_customName}, Threshold: {_thresholdMilliseconds}ms)");
        }

        public void SetException()
        {
            _isException = true;
        }
    }
} 