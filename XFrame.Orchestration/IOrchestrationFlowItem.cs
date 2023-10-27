namespace XFrame.Orchestration
{
    public interface IOrchestrationFlowItem<T> where T : class
    {
        string StepName { get; }

        Task<OrchestrationFlowState> Execute(OrchestrationFlowControl<T> flowContinuation, T data);
    }
}
