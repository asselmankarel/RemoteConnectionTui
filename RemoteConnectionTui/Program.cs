using RemoteConnectionTui;
using RemoteConnectionTui.Models;
using RemoteConnectionTui.ViewModels;

var app = new Tui(new ServerList
{
    Servers = [
        new Server("Linux Admin", "admin01.example.com", ProtocolType.Ssh, 22, "Primary Linux admin access via SSH"),
        new Server("Remote Desktop", "rdp01.example.com", ProtocolType.Rdp, 3389, "Windows RDP server for remote users"),
        new Server("Web Server", "web01.example.com", ProtocolType.Ssh, 22, "Public-facing HTTP web server"),
        new Server("Secure Web", "web02.example.com", ProtocolType.Ssh, 22, "Internal HTTPS-only web application"),
        new Server("Legacy FTP", "ftp01.example.com", ProtocolType.Rdp, 3389, "Legacy FTP file server for archival access")
    ]
});

app.Run();
