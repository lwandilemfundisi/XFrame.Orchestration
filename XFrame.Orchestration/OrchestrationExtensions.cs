using Microsoft.Extensions.DependencyInjection;
using XFrame.Common.Extensions;

namespace XFrame.Orchestration
{
    public static class OrchestrationExtensions
    {
        public static TService GetService<TService, T>(this IOrchestrationFlow<T> orchestrationFlow)
            where T : class
        {
            if (orchestrationFlow.IsNull())
                throw new ArgumentNullException($"{typeof(OrchestrationExtensions).PrettyPrint()} : orchestrationFlow is null");

            return orchestrationFlow.ServiceProvider.GetService<TService>();
        }
    }
}
