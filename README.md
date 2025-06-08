# MethodWatch

A lightweight .NET library for method execution time monitoring and logging. MethodWatch provides both automatic and manual method timing capabilities with customizable thresholds and real-time statistics.

## Features

- **Automatic Method Timing**: Use the `[MethodWatch]` attribute to automatically time method execution
- **Manual Method Timing**: Use the `MethodWatch.Measure()` method for manual timing of code blocks
- **Customizable Thresholds**: Set performance thresholds for each method
- **Real-time Statistics**: Track execution times, min/max values, and failure rates
- **Web UI**: Visualize method performance with a built-in web interface
- **Exception Handling**: Automatic timing of methods that throw exceptions
- **Configurable Statistics**: Enable/disable statistics collection for better performance

## Installation

```bash
dotnet add package MethodWatch
```

## Usage

### Basic Setup

Add MethodWatch to your application:

```csharp
// In Program.cs
MethodWatch.MethodWatch.Initialize(loggerFactory, enableStatistics: true);
```

### Automatic Method Timing

Add the `[MethodWatch]` attribute to any method you want to monitor:

```csharp
// Basic usage - all times will be logged
[MethodWatch(0)]
public void FastOperation()
{
    // Method implementation
}

// Set threshold to 100ms - times >= 100ms will be marked as slow
[MethodWatch(100)]
public void SlowOperation()
{
    Thread.Sleep(200);
}

// Set threshold to 50ms - times >= 50ms will be marked as slow
[MethodWatch(50)]
public void RandomOperation()
{
    Thread.Sleep(Random.Shared.Next(50, 300));
}
```

### Manual Method Timing

Use the `MethodWatch.Measure()` method to time specific code blocks:

```csharp
// Simple usage with custom name
using (MethodWatch.Measure("CustomOperation"))
{
    // Your code here
}

// With threshold - times >= 100ms will be marked as slow
using (MethodWatch.Measure("CustomOperation", 100))
{
    // Your code here
}

// Nested measurements with different thresholds
using (MethodWatch.Measure("OuterOperation", 200))
{
    // Outer operation code
    using (MethodWatch.Measure("InnerOperation", 50))
    {
        // Inner operation code
    }
}
```

### Web UI

MethodWatch includes a web interface to visualize method performance:

1. Add the web UI to your project:
```csharp
app.UseStaticFiles();
app.MapFallbackToFile("methodwatch.html");
```

2. Access the UI at `/methodwatch.html`

Features:
- Real-time statistics updates
- Search and filter methods
- Sort by various metrics
- Color-coded performance indicators
- Pagination for large datasets

### Configuration

#### Enable/Disable Statistics

Control statistics collection through configuration:

```json
{
  "MethodWatch": {
    "EnableStatistics": false
  }
}
```

Or in code:
```csharp
MethodWatch.MethodWatch.Initialize(loggerFactory, enableStatistics: false);
```

#### Logging Configuration

Configure logging to be more concise:
```csharp
builder.Logging.AddConsole(options =>
{
    options.IncludeScopes = false;
    options.TimestampFormat = "[HH:mm:ss] ";
});
```

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

### Manual Timing

```csharp
public void ComplexOperation()
{
    using (MethodWatch.Measure("ComplexOperation", 50))
    {
        // Complex operation implementation
    }
}
```

### Nested Measurements

```csharp
public void NestedOperations()
{
    using (MethodWatch.Measure("OuterOperation", 200))
    {
        // Some work
        using (MethodWatch.Measure("InnerOperation", 50))
        {
            // Inner work
        }
        // More work
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
            var taskId = i;
            tasks.Add(Task.Run(async () =>
            {
                using (MethodWatch.Measure($"ParallelTask_{taskId}", 100))
                {
                    await Task.Delay(Random.Shared.Next(50, 300));
                }
            }));
        }
        await Task.WhenAll(tasks);
    }
}
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 