using RemoteConnectionTui.Models;
using System.Diagnostics;

namespace RemoteConnectionTui.Helpers;

internal sealed class ProcessManager
{
    private readonly Dictionary<string, int> _processes = new();
    public IReadOnlyDictionary<string, int> Processes => _processes;

    public void Connect(Server server)
    {
        if (IsConnected(server))
        {
            return;
        }

        StartProcess(server);
    }

    internal bool IsConnected(Server server)
    {
        if (!_processes.ContainsKey(server.Name)) return false;
        
        try
        {
            if (Process.GetProcessById(_processes[server.Name]).HasExited)
            {
                _processes.Remove(server.Name);
            }
        }
        catch (ArgumentException)
        {
            _processes.Remove(server.Name);
        }        

        return _processes.TryGetValue(server.Name, out int _);
    }

    private void StartProcess(Server server)
    {
        var process = new Process();
        if (server.Protocol == ProtocolType.Ssh)
        {
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = "ssh";
            process.StartInfo.Arguments = $"{server.Fqdn} -p {server.Port}";
        }
        else
        {
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = "mstsc.exe";
            process.StartInfo.Arguments = $"/v:{server.Fqdn}:{server.Port}";
        }

        process.Start();
        _processes.Add(server.Name, process.Id);
    }
}
