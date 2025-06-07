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
        [MethodWatch]
        public IActionResult FastOperation()
        {
            return Ok("Fast operation completed");
        }

        [HttpGet("slow")]
        [MethodWatch]
        public async Task<IActionResult> SlowOperation()
        {
            await Task.Delay(200);
            return Ok("Slow operation completed");
        }

        [HttpGet("with-params")]
        [MethodWatch(LogParameters = true)]
        public IActionResult OperationWithParams([FromQuery] string name, [FromQuery] int value)
        {
            return Ok($"Operation with parameters: {name}, {value}");
        }

        [HttpGet("with-exception")]
        [MethodWatch]
        public IActionResult OperationWithException()
        {
            throw new Exception("Test exception");
        }

        [HttpGet("complex")]
        [MethodWatch(LogParameters = true)]
        public IActionResult ComplexOperation([FromBody] ComplexObject data)
        {
            return Ok(data);
        }

        [HttpGet("manual")]
        public IActionResult ManualMeasurement()
        {
            // Example 1: Simple measurement
            using (MethodWatch.Measure("TestController", "ManualMeasurement"))
            {
                // Simulate some work
                Thread.Sleep(50);
            }

            // Example 2: Measurement with parameters
            using (MethodWatch.Measure(
                "TestController", 
                "ManualMeasurement", 
                ("param1", "value1"), 
                ("param2", 42)))
            {
                // Simulate some work
                Thread.Sleep(75);
            }

            // Example 3: Nested measurements
            using (MethodWatch.Measure("TestController", "OuterOperation"))
            {
                // Simulate some work
                Thread.Sleep(25);

                using (MethodWatch.Measure("TestController", "InnerOperation"))
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