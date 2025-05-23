using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace NetTools.Pages
{
    internal sealed partial class IpPage : ListPage
    {
        public IpPage()
        {
            Title = "Check IP";
            Icon = new IconInfo("\uE701");
        }

        public override IListItem[] GetItems()
        {
            var internalIp = GetInternalIp();
            var externalIp = GetExternalIp();
            var dnsServers = GetDnsServers();
            var gateway = GetDefaultGateway();

            return new IListItem[]
            {
                new ListItem(new CopyTextCommand(internalIp))
                {
                    Title    = "Internal IP",
                    Icon     = new IconInfo("\uE896"),
                    Subtitle = internalIp
                },
                new ListItem(new CopyTextCommand(externalIp))
                {
                    Title    = "External IP",
                    Icon     = new IconInfo("\uE898"),
                    Subtitle = externalIp
                },
                new ListItem(new CopyTextCommand(dnsServers))
                {
                    Title    = "DNS Servers",
                    Icon     = new IconInfo("\uE774"),
                    Subtitle = dnsServers
                },
                new ListItem(new CopyTextCommand(gateway))
                {
                    Title    = "Default Gateway",
                    Icon     = new IconInfo("\uE968"),
                    Subtitle = gateway
                }
            };
        }

        private static string GetInternalIp()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                var address = host.AddressList
                                  .FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
                return address?.ToString() ?? "Niet gevonden";
            }
            catch (Exception ex)
            {
                return $"Fout: {ex.Message}";
            }
        }
        private static string GetExternalIp()
        {
            try
            {
                using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };
                return client.GetStringAsync("https://api.ipify.org").GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                return $"Niet beschikbaar ({ex.Message})";
            }
        }
        private static string GetDnsServers()
        {
            try
            {
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                var dnsAddresses = networkInterfaces
                    .SelectMany(ni => ni.GetIPProperties().DnsAddresses)
                    .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                    .Select(ip => ip.ToString());

                return dnsAddresses.Any() ? string.Join(", ", dnsAddresses) : "Niet gevonden";
            }
            catch (Exception ex)
            {
                return $"Fout: {ex.Message}";
            }
        }
        private static string GetDefaultGateway()
        {
            try
            {
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                var gatewayAddress = networkInterfaces
                    .SelectMany(ni => ni.GetIPProperties().GatewayAddresses)
                    .FirstOrDefault(g => g.Address.AddressFamily == AddressFamily.InterNetwork);

                return gatewayAddress?.Address.ToString() ?? "Niet gevonden";
            }
            catch (Exception ex)
            {
                return $"Fout: {ex.Message}";
            }
        }
    }
}