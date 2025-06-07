# MethodWatch

A lightweight .NET library for method execution time monitoring and logging. MethodWatch provides both automatic and manual method timing capabilities.

## Features

- **Automatic Method Timing**: Use the `[MethodWatch]` attribute to automatically time method execution
- **Manual Method Timing**: Use the `MethodWatch.Measure()` method for manual timing of code blocks
- **Customizable Logging**: Configure logging thresholds and parameter visibility
- **Exception Handling**: Automatic timing of methods that throw exceptions
- **Circular Reference Handling**: Safe serialization of complex objects with circular references

## Installation

```bash
dotnet add package MethodWatch
```

## Usage

### Automatic Method Timing

Add the `[MethodWatch]` attribute to any method you want to monitor:

```csharp
[MethodWatch]
public void MyMethod()
{
    // Method implementation
}
```

### Manual Method Timing

Use the `MethodWatch.Measure()` method to time specific code blocks:

```csharp
// Simple usage - automatically uses the calling method name
using (MethodWatch.Measure())
{
    // Your code here
}

// With custom name for better identification in logs
using (MethodWatch.Measure("CustomOperation"))
{
    // Your code here
}
```

### Configuration Options

The `[MethodWatch]` attribute supports the following options:

```csharp
[MethodWatch(
    ThresholdMilliseconds = 100,  // Log only if execution takes longer than 100ms
    LogParameters = true          // Log method parameters
)]
public void MyMethod(string param1, int param2)
{
    // Method implementation
}
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

### Manual Timing

```csharp
public void ComplexOperation()
{
    using (MethodWatch.Measure("ComplexOperation"))
    {
        // Complex operation implementation
    }
}
```

### Nested Measurements

```csharp
public void NestedOperations()
{
    using (MethodWatch.Measure("OuterOperation"))
    {
        // Some work
        using (MethodWatch.Measure("InnerOperation"))
        {
            // Inner work
        }
        // More work
    }
}
```

## Logging Output

MethodWatch provides clear and concise logging output:

```
TestController.SimpleMethod completed in 50.23ms
TestController.MethodWithParams(name="test", value=42) completed in 75.45ms
TestController.ComplexOperation completed in 123.67ms
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 