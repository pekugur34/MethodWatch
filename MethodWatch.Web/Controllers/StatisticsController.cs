using Microsoft.AspNetCore.Mvc;
using MethodWatch;
using Microsoft.Extensions.Logging;

namespace MethodWatch.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly ILogger<StatisticsController> _logger;

    public StatisticsController(ILogger<StatisticsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var stats = MethodWatchStatistics.GetStatistics().ToList();
        _logger.LogInformation($"Retrieved {stats.Count} statistics records");
        return Ok(stats);
    }

    [HttpPost("clear")]
    public IActionResult Clear()
    {
        MethodWatchStatistics.Clear();
        _logger.LogInformation("Statistics cleared");
        return Ok();
    }
} 