# MethodWatch

MethodWatch is a lightweight performance monitoring library for .NET applications that helps you track method execution times and identify performance bottlenecks.

## Features

- 🚀 Simple attribute-based method monitoring
- 📊 Real-time statistics and performance metrics
- 🔍 Detailed execution logs with parameters and results
- ⚡ Low overhead with configurable thresholds
- 🎯 Support for both synchronous and asynchronous methods
- 🌐 Built-in web UI for monitoring (optional)
- 🔄 Support for ASP.NET Core controllers

## Installation

```bash
dotnet add package MethodWatch
```

## Quick Start

1. Add the MethodWatch service to your application:

```csharp
// In Program.cs or Startup.cs
builder.Services.AddMethodWatch(options =>
{
    options.EnableStatistics = true;
    options.EnableWebUI = true; // Optional: enables the web monitoring UI
});
```

2. Use the `[MethodWatch]` attribute to monitor methods:

```csharp
public class ExampleService
{
    [MethodWatch(thresholdMs: 100)] // Log if execution takes more than 100ms
    public string ProcessData(string input)
    {
        // Your method implementation
        return result;
    }
}
```

## Configuration Options

```csharp
services.AddMethodWatch(options =>
{
    options.EnableStatistics = true;        // Enable performance statistics
    options.EnableWebUI = true;             // Enable web monitoring UI
    options.StatisticsPort = 5001;          // Port for statistics server
    options.WebUIPort = 5002;               // Port for web UI
    options.StatisticsEndpoint = "/stats";  // Endpoint for statistics
    options.WebUIEndpoint = "/methodwatch"; // Endpoint for web UI
});
```

## Attribute Options

```csharp
[MethodWatch(
    thresholdMs: 100,    // Log if execution takes more than 100ms
    name: "CustomName",  // Optional custom name for the method
    logParameters: true, // Log method parameters
    logResult: true,     // Log method result
    logStatistics: true  // Log performance statistics
)]
public void YourMethod() { }
```

## Web UI

If enabled, MethodWatch provides a web interface to monitor method executions in real-time. Access it at:
```
http://localhost:5002/methodwatch
```

## Statistics API

MethodWatch exposes a statistics API endpoint that provides performance metrics:
```
http://localhost:5001/stats
```

## Best Practices

1. Set appropriate thresholds based on your application's requirements
2. Enable parameter and result logging only when needed
3. Use the web UI for development and debugging
4. Consider disabling statistics in production if not needed

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details. 