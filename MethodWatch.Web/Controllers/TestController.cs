using Microsoft.AspNetCore.Mvc;
using MethodWatch;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Diagnostics;

namespace MethodWatch.Web.Controllers
{
    [ApiController]
    [Route("test")]
    public partial class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private static readonly Random _random = new Random();

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        // Example 1: Basic usage with threshold 0 (all times marked as slow)
        [HttpGet("fast")]
        [MethodWatch(0)]
        public IActionResult FastOperation()
        {
            return Ok("Fast operation completed");
        }

        // Example 2: Set threshold to 100ms - times >= 100ms will be marked as slow
        [HttpGet("slow")]
        [MethodWatch(100)]
        public IActionResult SlowOperation()
        {
            Thread.Sleep(200);
            return Ok("Slow operation completed");
        }

        // Example 3: Set threshold to 50ms - times >= 50ms will be marked as slow
        [HttpGet("random")]
        [MethodWatch(50)]
        public IActionResult RandomOperation()
        {
            Thread.Sleep(Random.Shared.Next(50, 300));
            return Ok("Random operation completed");
        }

        // Example 4: CPU-intensive operation with 200ms threshold
        [HttpGet("burst")]
        [MethodWatch(200)]
        public IActionResult BurstOperation()
        {
            var result = 0;
            for (int i = 0; i < 1000000; i++)
            {
                result += i;
            }
            return Ok($"Burst operation completed with result: {result}");
        }

        // Example 5: Operation that might throw an exception - always log (threshold 0)
        [HttpGet("exception")]
        [MethodWatch(0)]
        public IActionResult OperationWithException()
        {
            if (Random.Shared.Next(100) < 30)
            {
                throw new Exception("Random exception occurred");
            }
            return Ok("Operation completed successfully");
        }

        // Example 6: Complex operation with nested measurements
        [HttpGet("complex")]
        [MethodWatch(50)]
        public IActionResult ComplexOperation()
        {
            using (MethodWatch.Measure("ComplexOperation", 50))
            {
                Thread.Sleep(Random.Shared.Next(50, 300));
                using (MethodWatch.Measure("InnerOperation", 20))
                {
                    Thread.Sleep(30);
                }
                return Ok("Complex operation completed");
            }
        }

        // Example 7: Nested operation with manual measurements
        [HttpGet("nested")]
        [MethodWatch(0)]
        public IActionResult NestedOperation()
        {
            using (MethodWatch.Measure("NestedOperation", 100))
            {
                Thread.Sleep(50);
                using (MethodWatch.Measure("InnerOperation", 50))
                {
                    Thread.Sleep(30);
                }
            }
            return Ok("Nested operation completed");
        }

        // Example 8: Stress test with parallel operations
        [HttpGet("stress")]
        [MethodWatch(0)]
        public async Task<IActionResult> StressTest()
        {
            using (MethodWatch.Measure("StressTest", 200))
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
                return Ok("Stress test completed");
            }
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