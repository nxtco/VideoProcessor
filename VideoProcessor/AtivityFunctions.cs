using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace VideoProcessor
{
    public static class ActivityFunctions
    {
        [FunctionName(nameof(TranscodeVideo))]
        public static async Task<string> TranscodeVideo([ActivityTrigger] string inputVideo, ILogger log)
        {
            log.LogInformation($"Transcoding {inputVideo}.");
            // simulate doing the activity
            await Task.Delay(5000);
            return $"{Path.GetFileNameWithoutExtension(inputVideo)}-transcoded.mp4";
        }

        [FunctionName(nameof(ExtractThumbnail))]
        public static async Task<string> ExtractThumbnail([ActivityTrigger] string inputVideo, ILogger log)
        {
            log.LogInformation($"Extracting Thumbnail {inputVideo}.");
            if (inputVideo.Contains("error"))
            {
                throw new InvalidOperationException("Failed to extract thumbnail");
            }

            // simulate doing the activity
            await Task.Delay(5000);
            return $"{Path.GetFileNameWithoutExtension(inputVideo)}-thumbnail.png";
        }

        [FunctionName(nameof(PrependIntro))]
        public static async Task<string> PrependIntro([ActivityTrigger] string inputVideo, ILogger log)
        {
            var introLocation = Environment.GetEnvironmentVariable("IntroLocation");
            log.LogInformation($"Prepending Intro {introLocation} to  {inputVideo}.");

            // simulate doing the activity
            await Task.Delay(5000);
            return $"{Path.GetFileNameWithoutExtension(inputVideo)}-withintro.mp4";
        }

        [FunctionName(nameof(Cleanup))]
        public static async Task<string> Cleanup([ActivityTrigger] string[] filesToCleanUp, ILogger log)
        {
            foreach(var file in filesToCleanUp.Where(f => f != null))
            {
                log.LogInformation($"Deleting {file}");

                // simulate doing the activity
                await Task.Delay(1000);
            }
            return "Cleaned up successfully";
        }
    }

}