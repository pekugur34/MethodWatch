# MethodWatch Web UI

This is a web application that demonstrates the MethodWatch library's web UI capabilities.

## Setup

1. Install the package:
```bash
dotnet add package MethodWatch
```

2. Initialize in your application:
```csharp
// In Program.cs
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Information);
});
MethodWatch.MethodWatch.Initialize(loggerFactory, enableStatistics: true);
```

3. Configure the web UI:
```csharp
app.UseStaticFiles();
app.MapFallbackToFile("methodwatch.html");
```

## Features

- Real-time statistics visualization
- Method execution time tracking
- Performance monitoring
- Customizable thresholds
- Search and filter capabilities

## Access

Access the web UI at `/methodwatch.html` after starting the application. 