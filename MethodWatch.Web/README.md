# MethodWatch Web

This is the web project for MethodWatch, demonstrating the usage of the MethodWatch library in an ASP.NET Core application.

## Features

- 📊 Real-time method execution monitoring
- 🌐 Web UI for performance visualization
- 📈 Statistics API endpoint
- 🔍 Example controller implementations

## Getting Started

1. Clone the repository
2. Open the solution in your IDE
3. Run the project:
```bash
dotnet run
```

## Web UI

Access the MethodWatch web interface at:
```
https://localhost:7000/methodwatch
```

## Statistics API

Access the statistics API at:
```
https://localhost:7000/stats
```

## Publishing to NuGet

### Prerequisites

1. A NuGet account (create one at https://www.nuget.org)
2. Your NuGet API key (get it from your NuGet account settings)

### Steps to Publish

1. Update the version in `MethodWatch/MethodWatch.csproj`:
```xml
<Version>1.1.0</Version>
```

2. Build the project in Release mode:
```bash
dotnet build --configuration Release
```

3. Pack the project:
```bash
dotnet pack MethodWatch/MethodWatch.csproj --configuration Release
```

4. Push to NuGet:
```bash
dotnet nuget push MethodWatch/bin/Release/MethodWatch.1.1.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

### Versioning Guidelines

- Major version (1.x.x): Breaking changes
- Minor version (x.1.x): New features, no breaking changes
- Patch version (x.x.1): Bug fixes and minor improvements

## Project Structure

```
MethodWatch.Web/
├── Controllers/
│   ├── MethodWatchController.cs    # Main controller for MethodWatch UI
│   └── StatisticsController.cs     # Statistics API endpoint
├── wwwroot/
│   └── methodwatch.html           # Web UI
└── Program.cs                     # Application configuration
```

## Configuration

The web project demonstrates how to configure MethodWatch in an ASP.NET Core application:

```csharp
builder.Services.AddMethodWatch(options =>
{
    options.EnableStatistics = true;
    options.EnableWebUI = true;
    options.StatisticsPort = 7000;  // Using HTTPS port
    options.WebUIPort = 7000;       // Using HTTPS port
});
```

## Development

### Adding New Features

1. Create a new branch
2. Implement your changes
3. Update tests if necessary
4. Submit a pull request

### Testing

Run the tests:
```bash
dotnet test
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details. 