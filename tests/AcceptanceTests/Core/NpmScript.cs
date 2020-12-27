using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AcceptanceTests.Core
{
    public class NpmScript : IDisposable
    {
        private readonly string scriptName;

        private static readonly Regex urls =
            new Regex(
                "(ht|f)tp(s?)\\:\\/\\/[0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*(:(0-9)*)*(\\/?)([a-zA-Z0-9\\-\\.\\?\\,\\'\\/\\\\\\+&%\\$#_]*)?",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private Process process;
        public string Url { get; private set; }
        public bool HasUrl => !string.IsNullOrEmpty(Url);

        public int ProcessId => process?.Id ?? 0;

        private readonly TaskCompletionSource<bool> signal = new TaskCompletionSource<bool>(false);

        public NpmScript(string scriptName = "dev") => this.scriptName = scriptName;

        public async Task RunAsync(Action<string> output = null, int timeoutMs = 100000, string apiUrl = "https://localhost:5001")
        {
            lock (signal)
            {
                if (process == null)
                {
                    var info = new ProcessStartInfo("cmd")
                    {
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        Arguments = $"/C npm run {scriptName}",
                        WorkingDirectory = @"C:\git\dotnet-playground\web",
                        CreateNoWindow = true,
                        ErrorDialog = false,
                        Environment =
                        {
                            { "apiUrl", apiUrl }
                        }
                    };

                    process = new Process { EnableRaisingEvents = true, StartInfo = info };
                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.OutputDataReceived += (sender, eventArgs) =>
                    {
                        output?.Invoke(eventArgs.Data);

                        if (!string.IsNullOrEmpty(eventArgs.Data) && string.IsNullOrEmpty(Url))
                        {
                            var results = urls.Matches(eventArgs.Data);

                            if (results.Any())
                            {
                                Url = results.First().Value;
                                signal.SetResult(true);
                            }
                        }
                    };

                    process.ErrorDataReceived += (sender, args) =>
                    {
                        output?.Invoke(args.Data);

                        if (!signal.Task.IsCompleted)
                        {
                            signal.SetException(new Exception("npm web server failed to start"));
                        }
                    };

                    var cancellationTokenSource = new CancellationTokenSource(timeoutMs);
                    cancellationTokenSource.Token.Register(() =>
                    {
                        if (signal.Task.IsCompleted)
                            return;

                        Url = string.Empty;
                        signal.SetResult(true);
                    }, false);
                }
            }

            await signal.Task;
        }

        public void Dispose()
        {
            if (process == null)
            {
                return;
            }

            if (!process.HasExited)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    KillWindowsProcess(process.Id);
                else
                    KillUnixProcess(process.Id);
            }

            process.Dispose();
            process = null;
        }

        private static void KillWindowsProcess(int processId)
        {
            using var killer = Process.Start(new ProcessStartInfo("taskkill.exe", $"/PID {processId} /T /F")
            { UseShellExecute = false });
            killer.WaitForExit(2000);
        }

        private static void KillUnixProcess(int processId)
        {
            using (var idGetter = Process.Start(new ProcessStartInfo("ps", $"-o pid= --ppid {processId}")
            { UseShellExecute = false, RedirectStandardOutput = true }))
            {
                var exited = idGetter.WaitForExit(2000);
                if (exited && idGetter.ExitCode == 0)
                {
                    var stdout = idGetter.StandardOutput.ReadToEnd();
                    var pids = stdout.Split("\n").Select(pid => int.Parse(pid)).ToList();

                    foreach (var pid in pids)
                        KillUnixProcess(pid);
                }
            }

            Process.GetProcessById(processId).Kill();
        }
    }
}
