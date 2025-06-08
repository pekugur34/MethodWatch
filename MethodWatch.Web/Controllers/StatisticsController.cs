using Microsoft.AspNetCore.Mvc;
using MethodWatch;

namespace MethodWatch.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetStatistics()
    {
        if (!MethodWatch.IsStatisticsEnabled())
        {
            return NotFound("Statistics are not enabled");
        }

        var stats = MethodWatch.GetAllMethodStats();
        return Ok(stats);
    }

    [HttpGet("{methodName}")]
    public IActionResult GetMethodStatistics(string methodName)
    {
        if (!MethodWatch.IsStatisticsEnabled())
        {
            return NotFound("Statistics are not enabled");
        }

        var stats = MethodWatch.GetMethodStats(methodName);
        return Ok(stats);
    }
} 