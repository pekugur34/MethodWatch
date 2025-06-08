using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Text.Json;

namespace MethodWatch;

[AttributeUsage(AttributeTargets.Method)]
public class MethodWatchAttribute : Attribute, IActionFilter
{
    public string? Name { get; set; }
    public long ThresholdMs { get; set; } = 1000;
    public bool LogParameters { get; set; } = true;
    public bool LogResult { get; set; } = true;
    public bool LogStatistics { get; set; } = true;

    public MethodWatchAttribute() { }

    public MethodWatchAttribute(string name)
    {
        Name = name;
    }

    public MethodWatchAttribute(long thresholdMs)
    {
        ThresholdMs = thresholdMs;
    }

    public MethodWatchAttribute(string name, long thresholdMs)
    {
        Name = name;
        ThresholdMs = thresholdMs;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.HttpContext.Items["MethodWatch_StartTime"] = DateTime.UtcNow.Ticks;
        context.HttpContext.Items["MethodWatch_Arguments"] = context.ActionArguments;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.HttpContext.Items.TryGetValue("MethodWatch_StartTime", out var startTimeObj) && startTimeObj is long startTime)
        {
            var elapsedMs = (DateTime.UtcNow.Ticks - startTime) / TimeSpan.TicksPerMillisecond;
            var className = context.Controller.GetType().Name;
            var methodName = context.ActionDescriptor.RouteValues["action"] ?? "Unknown";
            var displayName = $"{className}.{methodName}";

            if (MethodWatch.IsStatisticsEnabled())
            {
                MethodWatch.RecordExecution(displayName, elapsedMs, ThresholdMs, context.Exception == null);
            }

            if (elapsedMs > ThresholdMs)
            {
                var logger = MethodWatch.GetLogger();
                if (logger != null)
                {
                    var logMessage = $"[MethodWatch] {displayName} took {elapsedMs}ms (threshold: {ThresholdMs}ms)";
                    
                    if (LogParameters && context.HttpContext.Items.TryGetValue("MethodWatch_Arguments", out var argsObj) && argsObj is IDictionary<string, object?> args)
                    {
                        logMessage += $"\nParameters: {MethodWatchHelper.SafeSerialize(args)}";
                    }
                    
                    if (LogResult && context.Result != null)
                    {
                        logMessage += $"\nResult: {MethodWatchHelper.SafeSerialize(context.Result)}";
                    }

                    if (LogStatistics && MethodWatch.IsStatisticsEnabled())
                    {
                        var stats = MethodWatch.GetMethodStats(displayName);
                        logMessage += $"\nStatistics: {MethodWatchHelper.SafeSerialize(stats)}";
                    }

                    logger.LogWarning(logMessage);
                }
            }
        }
    }

    public void LogMethodCall(string methodName, object?[]? parameters, object? result, long executionTimeMs)
    {
        if (MethodWatch.IsStatisticsEnabled())
        {
            MethodWatch.RecordExecution(methodName, executionTimeMs, ThresholdMs, true);
        }

        if (executionTimeMs > ThresholdMs)
        {
            var logger = MethodWatch.GetLogger();
            if (logger != null)
            {
                var logMessage = $"[MethodWatch] {methodName} took {executionTimeMs}ms (threshold: {ThresholdMs}ms)";
                
                if (LogParameters && parameters != null)
                {
                    logMessage += $"\nParameters: {MethodWatchHelper.SafeSerialize(parameters)}";
                }
                
                if (LogResult)
                {
                    logMessage += $"\nResult: {MethodWatchHelper.SafeSerialize(result)}";
                }

                if (LogStatistics && MethodWatch.IsStatisticsEnabled())
                {
                    var stats = MethodWatch.GetMethodStats(methodName);
                    logMessage += $"\nStatistics: {MethodWatchHelper.SafeSerialize(stats)}";
                }

                logger.LogWarning(logMessage);
            }
        }
    }
} 