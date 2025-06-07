using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Text.Json;

namespace MethodWatch;

[AttributeUsage(AttributeTargets.Method)]
public class MethodWatchAttribute : ActionFilterAttribute
{
    public bool LogParameters { get; set; } = true;
    public double ThresholdMilliseconds { get; set; } = 0;

    private string _methodName;
    private ILogger _logger;
    private ActionExecutingContext? _executingContext;

    public MethodWatchAttribute()
    {
        _methodName = string.Empty;
        _logger = null!;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _methodName = context.RouteData.Values["action"]?.ToString() ?? "Unknown";
        _logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
            .CreateLogger("MethodWatch");
        _executingContext = context;
        context.HttpContext.Items["MethodWatchStartTime"] = Stopwatch.StartNew();
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.HttpContext.Items["MethodWatchStartTime"] is Stopwatch sw)
        {
            sw.Stop();
            var elapsed = sw.Elapsed.TotalMilliseconds;

            if (context.Exception != null)
            {
                _logger.LogError("[ERROR] {Class}.{Method} failed after {Elapsed:F2}ms", 
                    context.Controller.GetType().Name,
                    _methodName,
                    elapsed);
            }
            else if (ThresholdMilliseconds == 0 || elapsed >= ThresholdMilliseconds)
            {
                string paramString = "()";
                if (LogParameters && _executingContext?.ActionArguments.Count > 0)
                {
                    var parameters = _executingContext.ActionArguments
                        .Where(x => x.Value != null)
                        .Select(x => $"{x.Key}={SafeSerialize(x.Value)}")
                        .ToList();

                    paramString = $"({string.Join(", ", parameters)})";
                }

                var status = elapsed >= 100 ? "[SLOW]" : "[OK]";
                _logger.LogInformation("{Status} {Class}.{Method}{Params} -> {Elapsed:F2}ms", 
                    status,
                    context.Controller.GetType().Name,
                    _methodName,
                    paramString,
                    elapsed);
            }
        }
    }

    private static string SafeSerialize(object? obj)
    {
        if (obj == null) return "null";
        
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                MaxDepth = 3
            };
            return JsonSerializer.Serialize(obj, options);
        }
        catch
        {
            return obj.ToString() ?? "null";
        }
    }
} 