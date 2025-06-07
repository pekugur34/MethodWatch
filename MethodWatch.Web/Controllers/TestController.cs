using Microsoft.AspNetCore.Mvc;
using MethodWatch;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace MethodWatch.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("fast")]
        public IActionResult FastOperation()
        {
            using (MethodWatch.Measure())
            {
                return Ok("Fast operation completed");
            }
        }

        [HttpGet("slow")]
        public IActionResult SlowOperation()
        {
            using (MethodWatch.Measure())
            {
                Thread.Sleep(200);
                return Ok("Slow operation completed");
            }
        }

        [HttpGet("params")]
        public IActionResult OperationWithParams([FromQuery] string name, [FromQuery] int value)
        {
            using (MethodWatch.Measure())
            {
                return Ok($"Operation with params: {name}={value}");
            }
        }

        [HttpGet("exception")]
        public IActionResult OperationWithException()
        {
            using (MethodWatch.Measure())
            {
                throw new Exception("Test exception");
            }
        }

        [HttpGet("complex")]
        public IActionResult ComplexOperation()
        {
            using (MethodWatch.Measure())
            {
                var result = new ComplexObject
                {
                    Name = "Test",
                    Value = 42,
                    Items = new List<string> { "Item1", "Item2" }
                };
                result.Self = result; // Circular reference

                return Ok(result);
            }
        }

        [HttpGet("manual")]
        public IActionResult ManualMeasurement()
        {
            // Example 1: Simple measurement
            using (MethodWatch.Measure("SimpleMeasurement"))
            {
                // Simulate some work
                Thread.Sleep(50);
            }

            // Example 2: Measurement with custom name
            using (MethodWatch.Measure("MeasurementWithCustomName"))
            {
                // Simulate some work
                Thread.Sleep(75);
            }

            // Example 3: Nested measurements
            using (MethodWatch.Measure("OuterOperation"))
            {
                // Simulate some work
                Thread.Sleep(25);

                using (MethodWatch.Measure("InnerOperation"))
                {
                    // Simulate some work
                    Thread.Sleep(50);
                }

                // More work
                Thread.Sleep(25);
            }

            return Ok("Manual measurement examples completed");
        }
    }

    public class ComplexObject
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
        public List<string> Items { get; set; } = new();
        public ComplexObject? Self { get; set; }
    }
} 