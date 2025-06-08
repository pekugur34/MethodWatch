namespace MethodWatch;

public class MethodStats
{
    public long TotalExecutions { get; set; }
    public long TotalFailures { get; set; }
    public long TotalTime { get; set; }
    public long MinTime { get; set; } = long.MaxValue;
    public long MaxTime { get; set; }
    public long LastExecutionTime { get; set; }
    public DateTime LastExecution { get; set; }
    public string? LastError { get; set; }
    public long Threshold { get; set; }
    public long ExceededThresholdCount { get; set; }

    public void AddExecution(long executionTimeMs)
    {
        TotalExecutions++;
        TotalTime += executionTimeMs;
        LastExecutionTime = executionTimeMs;
        LastExecution = DateTime.UtcNow;
        
        if (executionTimeMs < MinTime) MinTime = executionTimeMs;
        if (executionTimeMs > MaxTime) MaxTime = executionTimeMs;
        if (executionTimeMs > Threshold) ExceededThresholdCount++;
    }
} 