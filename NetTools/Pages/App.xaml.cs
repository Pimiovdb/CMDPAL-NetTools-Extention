using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace NetTools
{
    public partial class App : Application
    {
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            // Maak en activeer hier je window
            var window = new Pages.SpeedTestPage();
            window.AppWindow.Title = "SpeedTest";
            window.Activate();
        }
    }
}
