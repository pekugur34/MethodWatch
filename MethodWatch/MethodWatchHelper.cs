using System.Text.Json;

namespace MethodWatch;

public static class MethodWatchHelper
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        MaxDepth = 10
    };

    public static string SafeSerialize(object? obj)
    {
        if (obj == null) return "null";
        
        try
        {
            return JsonSerializer.Serialize(obj, _jsonOptions);
        }
        catch (Exception ex)
        {
            return $"<Error serializing: {ex.Message}>";
        }
    }

    public static string GetMethodName(string methodName, string? customName)
    {
        return customName ?? methodName;
    }

    public static long GetElapsedMilliseconds(long startTicks)
    {
        return (DateTime.UtcNow.Ticks - startTicks) / TimeSpan.TicksPerMillisecond;
    }
} 