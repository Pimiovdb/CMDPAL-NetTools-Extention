using Microsoft.CommandPalette.Extensions.Toolkit;
using Microsoft.CommandPalette.Extensions;
using NetTools.Pages;

namespace NetTools;

public partial class NetToolsCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;

    public NetToolsCommandsProvider()
    {
        DisplayName = "NetTools";
        Icon = new IconInfo("\uEC7A");

        _commands = new ICommandItem[]
        {
            new CommandItem(new NetToolsPage())
            {
                Title    = "NetTools",
                Subtitle = "Collection for NetworkTools",
                Icon     = new IconInfo("\uEC7A")
            }
        };
    }

    public override ICommandItem[] TopLevelCommands() => _commands;
}
