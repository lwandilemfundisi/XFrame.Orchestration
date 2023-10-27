namespace XFrame.Orchestration
{
    public interface IOrchestrationFlow<T>
        where T : class
    {
        IServiceProvider ServiceProvider { get; }

        Action<T, string> FlowAbortHandler { get; }

        Action<T, OrchestrationFlowException> FlowExceptionHandler { get; }

        void AddWork(IOrchestrationFlowItem<T> work);

        Task<T> ExecuteFlow(T data);

        Task<T> ContinueWorkflow(OrchestrationFlowControl<T> flowContinuation, T data);
    }
}