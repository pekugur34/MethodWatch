# MethodWatch

A lightweight .NET library for method execution monitoring and performance tracking. MethodWatch provides both attribute-based and manual measurement capabilities to help you track method execution times and parameters in your applications.

## Features

- 🚀 Attribute-based method monitoring
- 📊 Manual measurement support
- ⏱️ Execution time tracking
- 🔍 Parameter logging
- 🎯 Performance thresholds
- 🛠️ Easy integration with ASP.NET Core

## Installation

```bash
dotnet add package MethodWatch
```

## Quick Start

### Attribute-based Monitoring

Add the `[MethodWatch]` attribute to any method you want to monitor:

```csharp
[MethodWatch]
public IActionResult FastOperation()
{
    return Ok("Fast operation completed");
}

[MethodWatch(LogParameters = true)]
public IActionResult OperationWithParams([FromQuery] string name, [FromQuery] int value)
{
    return Ok($"Operation with parameters: {name}, {value}");
}
```

### Manual Measurement

Use the `Measure` method for more granular control:

```csharp
using (MethodWatch.Measure("MyClass", "MyMethod", ("param1", value1), ("param2", value2)))
{
    // Your code here
}
```

### Configuration

Initialize MethodWatch in your ASP.NET Core application:

```csharp
// In Program.cs
MethodWatch.MethodWatch.Initialize(app.Services.GetRequiredService<ILoggerFactory>());
```

## Options

The `MethodWatch` attribute supports the following options:

- `LogParameters` (default: `true`): Log method parameters
- `ThresholdMilliseconds` (default: `0`): Only log if execution time exceeds this threshold

## Log Output

MethodWatch provides clear and concise log output:

```
[OK] TestController.FastOperation() -> 5.23ms
[SLOW] TestController.SlowOperation() -> 150.45ms
[ERROR] TestController.OperationWithException() -> 0.50ms
```

## Examples

### Basic Usage

```csharp
[MethodWatch]
public void SimpleMethod()
{
    // Method implementation
}
```

### With Parameters

```csharp
[MethodWatch(LogParameters = true)]
public void MethodWithParams(string name, int value)
{
    // Method implementation
}
```

### Manual Measurement

```csharp
// Simple measurement
using (MethodWatch.Measure("MyClass", "MyMethod"))
{
    // Code to measure
}

// With parameters
using (MethodWatch.Measure(
    "MyClass", 
    "MyMethod", 
    ("param1", "value1"), 
    ("param2", 42)))
{
    // Code to measure
}

// Nested measurements
using (MethodWatch.Measure("MyClass", "OuterOperation"))
{
    // Some work
    using (MethodWatch.Measure("MyClass", "InnerOperation"))
    {
        // Inner work
    }
    // More work
}
```

## Performance Considerations

- MethodWatch adds minimal overhead to method execution
- Parameter serialization is optimized for common types
- Circular references are handled safely
- Logging is asynchronous and non-blocking

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 