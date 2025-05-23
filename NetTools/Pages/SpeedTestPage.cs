using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;

namespace NetTools.Pages
{
    public sealed partial class SpeedTestPage : ContentPage
    {
        private readonly GenerateSpeedTestForm _form = new();

        public SpeedTestPage()
        {
            Name = "SpeedTest";
            Title = "Speed Test";
            Icon = new IconInfo("\uE8C8");
        }
        
        public override IContent[] GetContent()
            => new IContent[] { _form };
    }

    internal sealed partial class GenerateSpeedTestForm : FormContent
    {
        public GenerateSpeedTestForm()
        {
            TemplateJson = $$"""
            {
              "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
              "type": "AdaptiveCard",
              "version": "1.6",
              "body": [
                {
                  "type": "TextBlock",
                  "text": "Click **Run SpeedTest** to measure download & upload speeds.",
                  "wrap": true,
                  "verticalContentAlignment": "Center",
                  "horizontalAlignment": "Center"
                }
              ],
              "actions": [
                {
                  "type": "Action.Submit",
                  "title": "Run SpeedTest"
                }
              ],
              "verticalContentAlignment": "Center",
              "horizontalContentAlignment": "Center"
            }
            """;
            DataJson = "{}";
        }

        public override ICommandResult SubmitForm(string payload)
        {
            var status = new StatusMessage
            {
                Message = "Starting SpeedTest...",
                State = MessageState.Info,
                Progress = new ProgressState { IsIndeterminate = true }
            };
            ExtensionHost.ShowStatus(status, StatusContext.Page);

            _ = Task.Run(async () =>
            {
                try
                {
                    string exe = "SpeedTest.exe";
                    string basedir = AppDomain.CurrentDomain.BaseDirectory;
                    string path = Path.Combine(basedir, "Assets", exe);
                    if (!File.Exists(path))
                        throw new FileNotFoundException($"CLI Not Found: {path}");

                    var psi = new ProcessStartInfo
                    {
                        FileName = path,
                        Arguments = "--format=json --progress --accept-license --accept-gdpr",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                    using var proc = Process.Start(psi)!;

                    DataReceivedEventHandler handler = (s, e) =>
                    {
                        if (string.IsNullOrWhiteSpace(e.Data))
                            return;

                        try
                        {
                            using var doc = JsonDocument.Parse(e.Data);
                            var root = doc.RootElement;
                            string type = root.GetProperty("type").GetString() ?? "";

                            switch (type)
                            {
                                case "ping":
                                    {
                                        var pingObj = root.GetProperty("ping");
                                        double lat = pingObj.GetProperty("latency").GetDouble();
                                        double pct = pingObj.GetProperty("progress").GetDouble() * 100;
                                        status.Progress = new ProgressState
                                        {
                                            IsIndeterminate = false,
                                            ProgressPercent = (uint)Math.Round(pct)
                                        };
                                        status.Message = $"Ping: {lat:F0} ms ({pct:F0}%)";
                                        break;
                                    }
                                case "download":
                                    {
                                        var dlObj = root.GetProperty("download");
                                        double bandwidth = dlObj.GetProperty("bandwidth").GetDouble();
                                        double pct = dlObj.GetProperty("progress").GetDouble() * 100;
                                        status.Progress = new ProgressState
                                        {
                                            IsIndeterminate = false,
                                            ProgressPercent = (uint)Math.Round(pct)
                                        };
                                        double mbps = bandwidth * 8 / 1_000_000;
                                        status.Message = $"Download: {mbps:F2} Mbps ({pct:F0}%)";
                                        break;
                                    }
                                case "upload":
                                    {
                                        var upObj = root.GetProperty("upload");
                                        double bandwidth = upObj.GetProperty("bandwidth").GetDouble();
                                        double pct = upObj.GetProperty("progress").GetDouble() * 100;
                                        status.Progress = new ProgressState
                                        {
                                            IsIndeterminate = false,
                                            ProgressPercent = (uint)Math.Round(pct)
                                        };
                                        double mbps = bandwidth * 8 / 1_000_000;
                                        status.Message = $"Upload:   {mbps:F2} Mbps ({pct:F0}%)";
                                        break;
                                    }
                                default:
                                    if (root.TryGetProperty("download", out _))
                                    {
                                        double downBps = root.GetProperty("download").GetProperty("bandwidth").GetDouble();
                                        double upBps = root.GetProperty("upload").GetProperty("bandwidth").GetDouble();
                                        double pingMs = root.GetProperty("ping").GetProperty("latency").GetDouble();
                                        double downMbps = downBps * 8 / 1_000_000;
                                        double upMbps = upBps * 8 / 1_000_000;

                                        status.Progress = null;
                                        status.State = MessageState.Success;
                                        status.Message =
                                            $"✅ Download: {downMbps:F2} Mbps · " +
                                            $"Upload: {upMbps:F2} Mbps · " +
                                            $"Ping:     {pingMs:F0} ms";
                                    }
                                    break;
                            }
                            ExtensionHost.ShowStatus(status, StatusContext.Page);
                        }
                        catch{}
                    };
                    proc.OutputDataReceived += handler;
                    proc.ErrorDataReceived += handler;
                    proc.BeginOutputReadLine();
                    proc.BeginErrorReadLine();

                    await proc.WaitForExitAsync();
                }
                catch (Exception ex)
                {
                    status.Progress = null;
                    status.State = MessageState.Error;
                    status.Message = $"❌ Speedtest Failed: {ex.Message}";
                    ExtensionHost.ShowStatus(status, StatusContext.Page);
                }
            });
            return CommandResult.KeepOpen();
        }
    }
}
