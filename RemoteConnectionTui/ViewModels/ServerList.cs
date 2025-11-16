using RemoteConnectionTui.Models;

namespace RemoteConnectionTui.ViewModels;

internal sealed class ServerList
{
    public Server[] Servers { get; set; } = [];
    public int IndexActiveServer { get; set; } = 0;

    public void MoveUp()
    {
        if (Servers.Length == 0)
        {
            return; 
        }
        if (IndexActiveServer == 0)
        {
            IndexActiveServer = Servers.Length - 1;
        }
        else
        {
            IndexActiveServer--;
        }   
    }   

    public void MoveDown()
    {
        if (Servers.Length == 0)
        {
            return; 
        }
        if (IndexActiveServer == Servers.Length - 1)
        {
            IndexActiveServer = 0;
        }
        else
        {
            IndexActiveServer++;
        }
    }

    public void AddServer(Server server)
    {
        Servers = Servers.Append(server).ToArray();
    }

}
