
namespace Microsoft.Phone.Scheduler
{
    public enum AgentExitReason
    {
        None,
        Completed,
        Aborted,
        MemoryQuotaExceeded,
        ExecutionTimeExceeded,
        UnhandledException,
        Terminated,
        Other,
    }
}
