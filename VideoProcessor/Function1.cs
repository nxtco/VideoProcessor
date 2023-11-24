using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace VideoProcessor
{
    public static class AtivityFunctions
    {
        [FunctionName(nameof(TranscodeVideo))]
        public static async Task<string> TranscodeVideo([ActivityTrigger] string inputVideo, ILogger log)
        {
            log.LogInformation($"Transcoding {inputVideo}.");

            await Task.Delay(5000);
            return $"{Path.GetFileNameWithoutExtension(inputVideo)}-transcoded.mp4";
        }

        [FunctionName(nameof(ExtractThumbnail))]
        public static async Task<string> ExtractThumbnail([ActivityTrigger] string inputVideo, ILogger log)
        {
            log.LogInformation($"Extracting thumbnail {inputVideo}.");

            await Task.Delay(5000);
            return $"{Path.GetFileNameWithoutExtension(inputVideo)}-thumbnail.png";
        }

        [FunctionName(nameof(PrependIntro))]
        public static async Task<string> PrependIntro([ActivityTrigger] string inputVideo, ILogger log)
        {
            var introLocation = Environment.GetEnvironmentVariable("Introduction");
            log.LogInformation($"Prepending Intro {introLocation} to {inputVideo}.");

            await Task.Delay(5000);
            return $"{Path.GetFileNameWithoutExtension(inputVideo)}-withintro.mp4";
        }
    }
} 