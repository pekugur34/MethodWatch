using System.Text.Json;

namespace MethodWatch
{
    public static class MethodWatchHelper
    {
        public static string SafeSerialize(object value)
        {
            if (value == null) return "null";
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = false,
                    MaxDepth = 2,
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
                };
                return JsonSerializer.Serialize(value, options);
            }
            catch
            {
                return value.ToString() ?? "<ToString failed>";
            }
        }
    }
} 