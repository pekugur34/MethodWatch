using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure logging to be more concise
builder.Logging.ClearProviders();
builder.Logging.AddConsole(options =>
{
    options.FormatterName = "simple";
});

builder.Services.Configure<ConsoleFormatterOptions>(options =>
{
    options.IncludeScopes = false;
    options.TimestampFormat = "[HH:mm:ss] ";
});

// Configure logging for specific namespaces
builder.Logging.AddFilter("Microsoft", LogLevel.Warning);
builder.Logging.AddFilter("System", LogLevel.Warning);
builder.Logging.AddFilter("MethodWatch", LogLevel.Debug);

var app = builder.Build();

// Initialize MethodWatch for manual measurement
MethodWatch.MethodWatch.Initialize(app.Services.GetRequiredService<ILoggerFactory>(), 
    enableStatistics: builder.Configuration.GetValue<bool>("MethodWatch:EnableStatistics", true));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Add default route for index.html
app.MapFallbackToFile("index.html");

app.UseAuthorization();
app.MapControllers();

app.Run();
