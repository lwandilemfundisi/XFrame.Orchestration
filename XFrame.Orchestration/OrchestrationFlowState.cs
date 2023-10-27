using XFrame.ValueObjects.XmlValueObjects;

namespace XFrame.Orchestration
{
    [ValueObjectResourcePath("XFrame.Orchestration.OrchestrationFlowState.xml")]
    public class OrchestrationFlowState : XmlValueObject
    {
        #region Constructors

        public OrchestrationFlowState()
        {

        }

        #endregion
    }

    public class FlowStates : XmlValueObjectLookup<OrchestrationFlowState, FlowStates>
    {
        #region Constructors

        public FlowStates()
        {

        }

        #endregion

        #region Properties

        public OrchestrationFlowState Error { get { return FindValueObject("Error"); } }

        public OrchestrationFlowState Abort { get { return FindValueObject("Abort"); } }

        public OrchestrationFlowState Continue { get { return FindValueObject("Continue"); } }

        public OrchestrationFlowState Defer { get { return FindValueObject("Defer"); } }

        public OrchestrationFlowState Deferred { get { return FindValueObject("Deferred"); } }

        public OrchestrationFlowState Done { get { return FindValueObject("Done"); } }

        #endregion
    }
}
