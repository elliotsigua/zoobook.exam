using System;
using System.Diagnostics;

namespace Zoobook.Shared
{
    public static class StopwatchExtension
    {
        public static string GetLapsedTime(this Stopwatch stopwatch, string process = "")
        {
            if (!stopwatch.IsRunning) return $"{ (string.IsNullOrEmpty(process) ? process : $"{ process } - ") }Elapsed Time: 00:00";

            stopwatch.Stop();
            var elapsedTime = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).ToString("mm\\:ss");
            stopwatch.Restart();

            return $"{ (string.IsNullOrEmpty(process) ? process : $"{ process } - " ) }Elapsed Time: { elapsedTime }";
        }
    }
}
