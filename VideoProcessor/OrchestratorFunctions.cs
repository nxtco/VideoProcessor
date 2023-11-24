using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace VideoProcessor
{
    public static class OrchestratorFunctions
    {
        [FunctionName(nameof(ProcessVideoOrchestrator))]
        public static async Task<object> ProcessVideoOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var videoLocation = context.GetInput<string>();

            var transcodedLocation = await context.CallActivityAsync<string>("TranscodeVideo", videoLocation);
            var thumbnailLocation = await context.CallActivityAsync<string>("ExtractThumbnail", transcodedLocation);
            var withIntroLocation = await context.CallActivityAsync<string>("PrependIntro", transcodedLocation);

            return new
            {
                Transcoded = transcodedLocation,
                Thumbnail = thumbnailLocation,
                WithIntro = withIntroLocation
            };
        }
    }
}