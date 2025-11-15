using RemoteConnectionTui.ViewModels;
using Spectre.Console;

namespace RemoteConnectionTui;

internal sealed class Tui
{
    private Layout _layout;
    private readonly ServerList _serverList;
    private readonly Style _darkCyan = new(Color.DarkCyan);
    private readonly Style _yellow = new(Color.Yellow);

    public Tui(ServerList serverList)
    {
        _serverList = serverList ?? new ServerList();

        _layout = new Layout().SplitColumns(
            new Layout("left").Size(30),
            new Layout("right")
                .SplitRows(
                new Layout("right-top"),
                    new Layout("right-bottom").Size(5)
                )
            );
        Draw();
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            AnsiConsole.Write(_layout);
            Console.CursorVisible = false;
            var keypress = Console.ReadKey();

            if (keypress.Key == ConsoleKey.Escape)
            {
                break;
            }

            HandleKeypress(keypress);
        }
    }

    private void Draw()
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

    private void HandleKeypress(ConsoleKeyInfo keypress)
    {
        switch (keypress.Key)
        {
            case ConsoleKey.UpArrow:
                _serverList.MoveUp();
                Draw();
                break;
            case ConsoleKey.DownArrow:
                _serverList.MoveDown();
                Draw();
                break;
            case ConsoleKey.Enter:
                break;
            case ConsoleKey.N:
                break;
            default:
                break;
        }
    }

    private Rows ServerList(ServerList serverList)
    {
        var servers = new List<Text>();
        servers.Add(new Text(""));

        for (int i = 0; i < serverList.Servers.Length; i++)
        {
            var server = serverList.Servers[i];
            if (i == serverList.IndexActiveServer)
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
}
