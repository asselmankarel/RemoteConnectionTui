using RemoteConnectionTui.Helpers;
using RemoteConnectionTui.ViewModels;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteConnectionTui.Views;

internal class MainWindow
{
    private Layout _layout;
    public Layout GetLayout() => _layout;

    private readonly ServerList _serverList;
    private readonly ProcessManager _processManager;
    private readonly Style _darkCyan = new(Color.DarkCyan);
    private readonly Style _yellow = new(Color.Yellow);
    private readonly Style _green = new(Color.Green);

    public MainWindow(ServerList serverList, ProcessManager processManager)
    {
        _serverList = serverList ?? new ServerList();
        _processManager = processManager;
        _layout = new Layout().SplitColumns(
           new Layout("left").Size(30),
           new Layout("right")
               .SplitRows(
               new Layout("right-top"),
                   new Layout("right-bottom").Size(5)
               )
           );
    }
    public void Update() 
    {
        _layout["left"].Update(
           new Panel(ServerList(_serverList))
           { Header = new PanelHeader("([blue]Servers[/])") }
               .Border(BoxBorder.Rounded)
               .Expand()
           );

        _layout["right-top"].Update(
            new Panel(ServerDedails(_serverList))
            { Header = new PanelHeader("([blue]Details[/])") }
                .Border(BoxBorder.Rounded)
                .Expand()
            );

        _layout["right-bottom"].Update(
           new Panel(GetCommands())
           { Header = new PanelHeader("([blue]Keys[/])") }
               .Border(BoxBorder.Rounded)
               .Expand()
           );
    }

    private Rows ServerList(ServerList serverList)
    {
        var servers = new List<Text>();
        servers.Add(new Text(""));

        for (int i = 0; i < serverList.Servers.Length; i++)
        {
            var server = serverList.Servers[i];
            bool connected = _processManager.IsConnected(server);


            if (connected)
            {
                servers.Add(new Text($"  {server.Name} (*)", _green));
            }
            else if (i == serverList.IndexActiveServer)
            {
                servers.Add(new Text($"> {server.Name}", _darkCyan));
            }
            else
            {
                servers.Add(new Text($"  {server.Name}", new(Color.Yellow)));
            }
        }
        return new Rows(servers);
    }

    private Rows GetCommands()
    {
        var columns = new List<Text>()
        {
            new("Up/Down arrow: Server selection", _darkCyan),
            new("Enter: Connect", _darkCyan),
            new("N: Add new server", _darkCyan)

        };

        return new Rows(new Text(""), new Columns(columns));
    }

    private Table ServerDedails(ServerList serverList)
    {
        Table table = new();
        table.Title("");

        table.AddColumn("[DarkCyan]Server[/]");
        table.AddColumn("[DarkCyan]FQDN[/]");
        table.AddColumn("[DarkCyan]Protocol[/]");
        table.AddColumn("[DarkCyan]Port[/]");
        table.AddColumn("[DarkCyan]Description[/]");

        table.Border(TableBorder.Rounded);
        table.BorderColor(Color.DarkCyan);
        var server = serverList.Servers[serverList.IndexActiveServer];

        table.AddRow(
            new Text(server.Name, _yellow),
            new Text(server.Fqdn, _yellow),
            new Text(server.Protocol.ToString(), _yellow),
            new Text(server.Port.ToString(), _yellow),
            new Text(server.Description, _yellow)
            );

        return table.Expand();
    }
}
