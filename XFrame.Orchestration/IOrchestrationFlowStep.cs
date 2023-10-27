namespace XFrame.Orchestration
{
    public interface IOrchestrationFlowStep<T>
        where T : class
    {
        Func<OrchestrationFlowControl<T>, T, Task<OrchestrationFlowState>> FlowStep { get; }
    }
}
