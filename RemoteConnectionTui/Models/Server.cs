namespace RemoteConnectionTui.Models;

internal record Server(string Name, string Fqdn, ProtocolType Protocol, int Port, string Description)
{
}
