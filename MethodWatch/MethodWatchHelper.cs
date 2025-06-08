using System.Text.Json;

namespace MethodWatch
{
    public static class MethodWatchHelper
    {
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = false,
            MaxDepth = 2,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        };

        public static string SafeSerialize(object value)
        {
            if (value == null) return "null";
            try
            {
                return JsonSerializer.Serialize(value, _jsonOptions);
            }
            catch
            {
                return value.ToString() ?? "<ToString failed>";
            }
        }
    }
} 