using Microsoft.AspNetCore.Mvc;
using MethodWatch;

namespace MethodWatch.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MethodWatchController : ControllerBase
{
    [HttpGet("statistics")]
    public IActionResult GetStatistics()
    {
        var stats = MethodWatch.GetAllMethodStats();
        return Ok(stats);
    }

    [HttpGet("statistics/{methodName}")]
    public IActionResult GetMethodStatistics(string methodName)
    {
        var stats = MethodWatch.GetMethodStats(methodName);
        return Ok(stats);
    }
} 