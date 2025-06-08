# MethodWatch

A lightweight .NET library for method execution time monitoring and logging with customizable thresholds and real-time statistics.

## Installation

### Core Package
```bash
dotnet add package MethodWatch
```

### Web Interface Package (Optional)
```bash
dotnet add package MethodWatch.Web
```

## Usage

### 1. Basic Usage with Core Package

```csharp
// In Program.cs or Startup.cs
using MethodWatch;

// Initialize with logging
builder.Services.AddLogging();
MethodWatch.Initialize(builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>());

// Optionally enable statistics
MethodWatch.Initialize(builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>(), enableStatistics: true);
```

Add the attribute to methods you want to monitor:

```csharp
[MethodWatch]
public async Task<string> GetDataAsync()
{
    // Your method implementation
}

// With custom threshold (in milliseconds)
[MethodWatch(thresholdMs: 500)]
public async Task ProcessDataAsync()
{
    // Your method implementation
}

// With custom name and threshold
[MethodWatch("ProcessData", 500)]
public async Task ProcessDataAsync()
{
    // Your method implementation
}
```

### 2. Using the Web Interface

If you want to visualize method execution statistics, add the web interface:

```csharp
// In Program.cs
using MethodWatch.Web;

// Add MethodWatch services
builder.Services.AddMethodWatch();

// Add the web interface
builder.Services.AddMethodWatchWeb();

var app = builder.Build();

// Map the web interface endpoint
app.MapMethodWatchWeb();
```

Then access the statistics dashboard at:
```
http://your-app/methodwatch.html
```

### 3. Advanced Configuration

#### Core Package Options

```csharp
[MethodWatch(
    Name = "CustomName",           // Custom name for the method
    ThresholdMs = 1000,            // Warning threshold in milliseconds
    LogParameters = true,          // Log method parameters
    LogResult = true,              // Log method result
    LogStatistics = true           // Log statistics with each execution
)]
public async Task MyMethod()
{
    // Your method implementation
}
```

#### Web Interface Features

The web interface provides:
- Real-time method execution statistics
- Execution time trends
- Success/failure rates
- Parameter and result logging
- Customizable thresholds

## Features

- Method execution time monitoring
- Customizable warning thresholds
- Parameter and result logging
- Real-time statistics
- Web-based dashboard
- Lightweight and non-intrusive
- Thread-safe implementation

## Requirements

- .NET 9.0 or later
- For web interface: ASP.NET Core

## License

MIT License 