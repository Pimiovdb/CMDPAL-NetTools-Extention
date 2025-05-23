using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using NetTools.Pages;



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
                new ListItem(new SpeedTestPage())
                {
                    Title    = "Run SpeedTest",
                    Subtitle = "Run SpeedTest with Ookla Speedtest service",
                    Icon     = new IconInfo("\uEC4A")
                },
                new ListItem(new IpPage())
                {
                    Title    = "Check IP",
                    Subtitle = "Display Network Info",
                    Icon     = new IconInfo("\uE701")
                }
            };
        }

    }
}
