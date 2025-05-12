// SpeedTestCommand.cs
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

                // Vind de map waar de extension draait
                var assemblyDir = Path.GetDirectoryName(typeof(SpeedTestCommand).Assembly.Location)!;
                var exePath = Path.Combine(assemblyDir, "Assets", "Speedtest.exe"); // let op: exact naam

                // Eventueel: voeg hier argumenten toe
                var psi = new ProcessStartInfo(exePath)
                {
                    UseShellExecute = true
                };

                Process.Start(psi);

                // Sluit de Palette, of zet KeepOpen=true als je 'm open wilt houden
                return CommandResult.KeepOpen();
            

        }
    }
}
