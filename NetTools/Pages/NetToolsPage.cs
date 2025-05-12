//NetToolsPage.cs
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using NetTools.Pages;
using NetTools.Commands;


namespace NetTools.Pages

{
    internal sealed partial class NetToolsPage : ListPage
    {
        public NetToolsPage()
        {
            Title = "NetTools";
            Icon = new IconInfo("\uEC7A");
        }

        public override IListItem[] GetItems()
        {
            return new IListItem[]
            {
            // Bestaande CLI-variant
                new ListItem(new SpeedTestCommand())
                {
                    Title    = "Run Speedtest (CLI)",
                    Subtitle = "Meet internetsnelheid",
                    Icon     = new IconInfo("\uEC4A")
                },

                // ★ Nieuwe grafische variant ★
                new ListItem(new OpenSpeedTestWindowCommand())
                {
                    Title    = "Run SpeedTest (WinUI 3)",
                    Subtitle = "WinUI 3 in eigen venster",
                    Icon     = new IconInfo("\uEC4A")
                },

                // Overige items
                new ListItem(new IpPage())
                {
                    Title    = "Check IP",
                    Subtitle = "Toont intern en extern IP",
                    Icon     = new IconInfo("\uE701")
                }
            };
        }

    }
}
