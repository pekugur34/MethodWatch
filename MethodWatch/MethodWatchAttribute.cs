using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Text.Json;

namespace MethodWatch;

[AttributeUsage(AttributeTargets.Method)]
public class MethodWatchAttribute : ActionFilterAttribute
{
    private ILogger? _logger;
    private string _methodName = "Unknown";
    private readonly long _thresholdMilliseconds;

    public MethodWatchAttribute()
    {
        _thresholdMilliseconds = 0;
    }

    public MethodWatchAttribute(long thresholdMilliseconds)
    {
        _thresholdMilliseconds = thresholdMilliseconds;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _logger = context.HttpContext.RequestServices.GetService<ILogger<MethodWatchAttribute>>();
        _methodName = context.ActionDescriptor.RouteValues["action"] ?? "Unknown";
        
        var className = context.Controller.GetType().Name;
        var parameters = context.ActionArguments
            .Select(x => $"{x.Key}={x.Value}")
            .ToList();

        var logMessage = $"{className}.{_methodName}({string.Join(", ", parameters)})";
        _logger?.LogInformation(logMessage);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (_logger == null) return;

        var className = context.Controller.GetType().Name;
        var elapsed = context.HttpContext.Items["MethodWatchStartTime"] != null
            ? (DateTime.UtcNow - (DateTime)context.HttpContext.Items["MethodWatchStartTime"]!).TotalMilliseconds
            : 0;

        // Record statistics only if enabled
        if (MethodWatch.IsStatisticsEnabled())
        {
            MethodWatchStatistics.RecordExecution(
                className,
                _methodName,
                (long)elapsed,
                context.Exception != null,
                null,
                _thresholdMilliseconds
            );
        }

        if (context.Exception != null)
        {
            _logger.LogError(context.Exception, $"{className}.{_methodName} failed after {elapsed:F2}ms");
        }
        else
        {
            _logger.LogInformation($"{className}.{_methodName} completed in {elapsed:F2}ms");
        }
    }
} 