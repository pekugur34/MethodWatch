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

    public static void Initialize(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger("MethodWatch");
    }

    /// <summary>
    /// Measures the execution time of a code block.
    /// </summary>
    /// <param name="customName">Optional custom name to identify this measurement in logs. If not provided, the calling method name will be used.</param>
    /// <returns>A disposable object that measures the execution time when disposed.</returns>
    /// <example>
    /// <code>
    /// using (MethodWatch.Measure("CustomOperation"))
    /// {
    ///     // Your code here
    /// }
    /// </code>
    /// </example>
    public static IDisposable Measure(string? customName = null)
    {
        var stackFrame = new StackFrame(1);
        var method = stackFrame.GetMethod();
        var className = method?.DeclaringType?.Name ?? "Unknown";
        var methodName = customName ?? method?.Name ?? "Unknown";
        
        return new MethodWatchScope(className, methodName);
    }

    private class MethodWatchScope : IDisposable
    {
        private readonly string _className;
        private readonly string _methodName;
        private readonly Stopwatch _stopwatch;

        public MethodWatchScope(string className, string methodName)
        {
            _className = className;
            _methodName = methodName;
            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            var elapsed = _stopwatch.Elapsed.TotalMilliseconds;

            if (_logger != null)
            {
                var status = elapsed >= 100 ? "[SLOW]" : "[OK]";
                _logger.LogInformation("{Status} {Class}.{Method} -> {Elapsed:F2}ms",
                    status,
                    _className,
                    _methodName,
                    elapsed);
            }
        }
    }
} 