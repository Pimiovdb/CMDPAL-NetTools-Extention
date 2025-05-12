//OpenSpeedTestWindowCommand.cs
using System.Threading;
using Microsoft.CommandPalette.Extensions.Toolkit;
using NetTools.Pages;
using Microsoft.UI.Xaml;
using Microsoft.CommandPalette.Extensions;
using WinRT.Interop;
using System.Runtime.InteropServices;
using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Documents;


namespace NetTools.Commands
{
    internal partial class OpenSpeedTestWindowCommand : InvokableCommand
    {

        //private static class NativeMethods
        //{
        //    public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        //    public const uint SWP_NOMOVE = 0x0002;
        //    public const uint SWP_NOSIZE = 0x0001;
        //
        //    [DllImport("user32.dll", SetLastError = true)]
        //    
        //    public static extern bool SetWindowPos(
        //        IntPtr hWnd,
        //        IntPtr hWndInsertAfter,
        //        int X, int Y, int cx, int cy,
        //        uint uFlags);
        //}


        public OpenSpeedTestWindowCommand()
        {
            Name = "Grafische Speedtest";
            Icon = new IconInfo("\uEC4A");
        }

        public override ICommandResult Invoke()
        {

            var uiThread = new Thread(() =>
            {
                if (Application.Current == null)
                {

                    Application.Start(_ => {
                        new App();  // Hiermee wordt OnLaunched() automatisch aangeroepen
                    });
                }
            });

            uiThread.SetApartmentState(ApartmentState.STA);
            uiThread.IsBackground = true;
            uiThread.Start();

            return CommandResult.KeepOpen();
        }
    }


}
