using System.Runtime.Serialization;

namespace XFrame.Orchestration
{
    public class OrchestrationFlowException : Exception
    {
        #region Constructors

        public OrchestrationFlowException()
            : base()
        {
        }

        public OrchestrationFlowException(string message)
            : base(message)
        {
        }

        public OrchestrationFlowException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public OrchestrationFlowException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
