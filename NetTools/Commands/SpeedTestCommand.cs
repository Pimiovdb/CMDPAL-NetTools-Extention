using System;
using System.Diagnostics;
using System.IO;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace NetTools.Commands
{
    internal sealed partial class SpeedTestCommand : InvokableCommand
    {

        public override CommandResult Invoke()
        {
                var assemblyDir = Path.GetDirectoryName(typeof(SpeedTestCommand).Assembly.Location)!;
                var exePath = Path.Combine(assemblyDir, "Assets", "Speedtest.exe");

                var psi = new ProcessStartInfo(exePath)
                {
                    UseShellExecute = true
                };

                Process.Start(psi);
                return CommandResult.KeepOpen();
            

        }
    }
}
