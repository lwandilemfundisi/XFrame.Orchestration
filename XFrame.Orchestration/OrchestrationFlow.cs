using XFrame.Common.Extensions;
using XFrame.ValueObjects.Extensions;

namespace XFrame.Orchestration
{
    public abstract class OrchestrationFlow<T> : IOrchestrationFlow<T> where T : class
    {
        private IList<IOrchestrationFlowItem<T>> _flowItems;

        #region Constructors

        public OrchestrationFlow(
            IServiceProvider serviceProvider)
        {
            OnInitializeFlowItems();
            ServiceProvider = serviceProvider;
        }

        #endregion

        #region IWorkflow

        public IServiceProvider ServiceProvider { get; }

        public Action<T, string> FlowAbortHandler => OnAbortHandler;

        public Action<T, OrchestrationFlowException> FlowExceptionHandler => OnException;

        public void AddWork(IOrchestrationFlowItem<T> work)
        {
            _flowItems.Add(work);
        }

        public async Task<T> ExecuteFlow(T data)
        {
            OrchestrationFlowControl<T> flowControl = new(this);
            return await InternalContinue(flowControl, data);
        }

        public async Task<T> ContinueWorkflow(OrchestrationFlowControl<T> flowControl, T data)
        {
            if (!flowControl.FlowState.IsIn(FlowStates.Of().Abort, FlowStates.Of().Done, FlowStates.Of().Error))
            {
                flowControl.FlowState = FlowStates.Of().Continue;

                await InternalContinue(flowControl, data);
            }

            return data;
        }

        #endregion

        #region Methods

        protected abstract void OnAbortHandler(T wrapper, string message);

        protected abstract void OnException(T wrapper, OrchestrationFlowException ex);

        protected virtual void OnInitializeFlowItems()
        {
            _flowItems = new List<IOrchestrationFlowItem<T>>();
        }

        private async Task<T> InternalContinue(OrchestrationFlowControl<T> flowControl, T data)
        {
            while (flowControl.FlowStep < _flowItems.Count && flowControl.FlowState.IsIn(FlowStates.Of().Continue))
            {
                var currentStep = _flowItems[flowControl.FlowStep++];
                await currentStep.Execute(flowControl, data)
                    .ContinueWith((f) =>
                    {
                        if (f.Exception.IsNotNull())
                        {
                            flowControl.StateMessage = $"There was an error executing step " +
                            $"{currentStep.StepName} of the orchestration. See logs!";
                            flowControl.Flow.FlowExceptionHandler(data, new OrchestrationFlowException(flowControl.StateMessage, f.Exception));
                            flowControl.FlowState = FlowStates.Of().Error;
                        }
                        else
                        {
                            flowControl.FlowState = f.Result;

                            if (flowControl.FlowState.IsIn(FlowStates.Of().Abort))
                            {
                                flowControl.Flow.FlowAbortHandler(data, flowControl.StateMessage);
                            }
                            else if (flowControl.FlowState.IsIn(FlowStates.Of().Error))
                            {
                                flowControl.Flow.FlowExceptionHandler(data, new OrchestrationFlowException(flowControl.StateMessage));
                            }
                        }
                    });
            }

            if (flowControl.FlowState.IsIn(FlowStates.Of().Defer))
            {
                flowControl.FlowState = FlowStates.Of().Deferred;
            }

            if (flowControl.FlowState.IsIn(FlowStates.Of().Abort, FlowStates.Of().Error))
            {
                await _flowItems[_flowItems.Count - 1].Execute(flowControl, data);
            }

            return data;
        }

        #endregion
    }
}
