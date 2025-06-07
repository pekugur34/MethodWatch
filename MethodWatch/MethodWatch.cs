using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace MethodWatch;

public static class MethodWatch
{
    private static ILogger? _logger;

    public static void Initialize(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger("MethodWatch");
    }

    public static IDisposable Measure(string className, string methodName, params (string name, object? value)[] parameters)
    {
        return new MethodTimer(className, methodName, parameters);
    }

    private class MethodTimer : IDisposable
    {
        private readonly string _className;
        private readonly string _methodName;
        private readonly (string name, object? value)[] _parameters;
        private readonly Stopwatch _stopwatch;

        public MethodTimer(string className, string methodName, (string name, object? value)[] parameters)
        {
            _className = className;
            _methodName = methodName;
            _parameters = parameters;
            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            var elapsed = _stopwatch.Elapsed.TotalMilliseconds;

            if (_logger != null)
            {
                string paramString = "()";
                if (_parameters.Length > 0)
                {
                    var paramList = _parameters
                        .Where(p => p.value != null)
                        .Select(p => $"{p.name}={p.value}")
                        .ToList();

                    paramString = $"({string.Join(", ", paramList)})";
                }

                var status = elapsed >= 100 ? "[SLOW]" : "[OK]";
                _logger.LogInformation("{Status} {Class}.{Method}{Params} -> {Elapsed:F2}ms",
                    status,
                    _className,
                    _methodName,
                    paramString,
                    elapsed);
            }
        }
    }
} 