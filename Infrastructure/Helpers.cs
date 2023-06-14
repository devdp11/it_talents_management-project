using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IPVC.ESTG.ES2
{
    public static class Helpers
    {
        private static string ExecuteCommandInternal(string command, bool redirectStandardOutput, bool returnOutput)
        {
            string fileName;
            string arguments;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                fileName = "cmd.exe";
                arguments = $"/C \"{command}\"";
            }
            else
            {
                fileName = "/bin/bash";
                arguments = $"-c \"{command}\"";
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = redirectStandardOutput,
                    UseShellExecute = false,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };

            Console.CancelKeyPress += (_, eventArgs) =>
            {
                // Propagate the CTRL+C event to the child process
                if (!process.HasExited)
                    process.Kill();

                eventArgs.Cancel = true;
            };

            process.Start();
            process.WaitForExit();

            return returnOutput ? process.StandardOutput.ReadToEnd() : "";
        }

        public static bool IsCommandAvailable(string command)
        {
            var output = ExecuteCommandInternal(
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"where {command}" : $"command -v {command}",
                redirectStandardOutput: true,
                returnOutput: true);

            return !string.IsNullOrEmpty(output);
        }

        public static void ExecuteCommand(string command, bool redirectStandardOutput = true)
        {
            ExecuteCommandInternal(command, redirectStandardOutput, returnOutput: false);
        }

        public static bool AreContainersRunning()
        {
            var output = ExecuteCommandInternal("docker-compose ps -q", redirectStandardOutput: true, returnOutput: true);
            return !string.IsNullOrEmpty(output);
        }
    }
}
