using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace VideoProcessor
{
    public static class Function1
    {
        [FunctionName(nameof(SayHello))]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation("Saying hello to {name}.", name);
            return $"Hello {name}!";
        }
    }
}