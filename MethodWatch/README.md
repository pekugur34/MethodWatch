# MethodWatch

[![NuGet Version](https://img.shields.io/nuget/v/MethodWatch.svg)](https://www.nuget.org/packages/MethodWatch)
[![License](https://img.shields.io/github/license/yourusername/MethodWatch.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/9.0)

A lightweight .NET library for method execution time monitoring and logging. MethodWatch provides both automatic and manual method timing capabilities with customizable thresholds and real-time statistics.

## Quick Start

1. Install the package:
```bash
dotnet add package MethodWatch
```

2. Initialize in your application:
```csharp
// In Program.cs
MethodWatch.MethodWatch.Initialize(loggerFactory, enableStatistics: true);
```

3. Use the attribute to monitor methods:
```csharp
[MethodWatch(100)] // Log if execution takes longer than 100ms
public void MyMethod()
{
    // Your code here
}
```

4. Or use manual timing:
```csharp
using (MethodWatch.Measure("CustomOperation", 50))
{
    // Your code here
}
```

## Key Features

### 🚀 Automatic Method Timing
```csharp
[MethodWatch(100)]
public void SlowOperation()
{
    Thread.Sleep(200); // Will be logged as slow
}
```

### ⚡ Manual Timing
```csharp
using (MethodWatch.Measure("CustomOperation", 50))
{
    // Your code here
}
```

### 📊 Real-time Statistics
- Track execution times
- Monitor min/max values
- View failure rates
- Web UI for visualization

### ⚙️ Configurable
```json
{
  "MethodWatch": {
    "EnableStatistics": false
  }
}
```

### 🔍 Performance Optimization
- Disable statistics when not needed
- Minimal memory footprint
- Efficient logging

## Web UI

Add to your ASP.NET Core application:
```csharp
app.UseStaticFiles();
app.MapFallbackToFile("methodwatch.html");
```

Access at `/methodwatch.html` to see:
- Real-time statistics
- Search and filter methods
- Sort by various metrics
- Color-coded performance indicators

## Examples

### Basic Usage
```csharp
[MethodWatch(0)]
public void SimpleMethod()
{
    // Method implementation
}
```

### With Threshold
```csharp
[MethodWatch(100)]
public void MethodWithThreshold()
{
    // Method implementation
}
```

### Nested Measurements
```csharp
public void NestedOperations()
{
    using (MethodWatch.Measure("OuterOperation", 200))
    {
        using (MethodWatch.Measure("InnerOperation", 50))
        {
            // Inner work
        }
    }
}
```

### Parallel Operations
```csharp
[MethodWatch(0)]
public async Task ParallelOperations()
{
    using (MethodWatch.Measure("MainOperation", 200))
    {
        var tasks = new List<Task>();
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(async () =>
            {
                using (MethodWatch.Measure($"ParallelTask_{i}", 100))
                {
                    await Task.Delay(Random.Shared.Next(50, 300));
                }
            }));
        }
        await Task.WhenAll(tasks);
    }
}
```

## Requirements

- .NET 9.0 or later
- For Web UI: ASP.NET Core

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 