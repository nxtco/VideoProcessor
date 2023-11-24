using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace VideoProcessor
{
    public static class OrchestratorFunctions
    {
        [FunctionName(nameof(ProcessVideoOrchestrator))]
        public static async Task<object> ProcessVideoOrchestrator(
    [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            log = context.CreateReplaySafeLogger(log);
            var videoLocation = context.GetInput<string>();

            string transcodedLocation = null;
            string thumbnailLocation = null;
            string withIntroLocation = null;

            try
            {
                transcodedLocation = await context.CallActivityAsync<string>("TranscodeVideo", videoLocation);
                log.LogInformation("about to call extract thumbnail activity");
                thumbnailLocation = await context.CallActivityAsync<string>("ExtractThumbnail", transcodedLocation);

                log.LogInformation("about to call prepend intro activity");
                withIntroLocation = await context.CallActivityAsync<string>("PrependIntro", transcodedLocation);
            }
            catch (Exception e)
            {
                log.LogError($"Caught an error from an activity: {e.Message}");

                await context.CallActivityAsync<string>("Cleanup", new[] { transcodedLocation, thumbnailLocation, withIntroLocation });
                return new
                {
                    Error = "Failed to process uploaded video",
                    Message = e.Message
                };
            }

            return new
            {
                Transcoded = transcodedLocation,
                Thumbnail = thumbnailLocation,
                WithIntro = withIntroLocation
            };
        }
    }

}