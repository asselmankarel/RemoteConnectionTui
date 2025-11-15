using RemoteConnectionTui.ViewModels;
using Spectre.Console;

namespace RemoteConnectionTui;

internal sealed class Tui
{
    private Layout _layout;
    private readonly ServerList _serverList;

    public Tui(ServerList serverList)
    {
        _serverList = serverList ?? new ServerList();

        _layout = new Layout().SplitColumns(
           new Layout("left").Size(30),
           new Layout("right")
           );

        Draw();
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            AnsiConsole.Write(_layout);
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
            { Header = new PanelHeader("Servers") }
                .Border(BoxBorder.Rounded)
                .Expand()

            );

        _layout["right"].Update(
            new Panel(ServerDedails(_serverList))
            { Header = new PanelHeader("Details") }
                .Border(BoxBorder.Rounded)
                .Expand()
            );
    }

    private static Table ServerDedails(ServerList serverList)
    {
        Table table = new Table();
        table.AddColumn("[green]Server[/]");
        table.AddColumn("[green]FQDN[/]");
        table.AddColumn("[green]Protocol[/]");
        table.AddColumn("[green]Port[/]");
        table.AddColumn("[green]Description[/]");

        table.Border(TableBorder.Rounded);
        table.BorderColor(Color.Yellow);
        var server = serverList.Servers[serverList.IndexActiveServer];

        var textSyle = new Style(Color.Yellow);
        table.AddRow(
            new Text(server.Name, textSyle),
            new Text(server.Fqdn, textSyle),
            new Text(server.Protocol.ToString(), textSyle),
            new Text(server.Port.ToString(), textSyle),
            new Text(server.Description, textSyle)
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
            default:
                break;
        }
    }

    private static Rows ServerList(ServerList serverList)
    {
        var servers = new List<Text>();

        for (int i = 0; i < serverList.Servers.Length; i++)
        {
            var server = serverList.Servers[i];
            if (i == serverList.IndexActiveServer)
            {
                servers.Add(new Text($"> {server.Name}", new(Color.Blue)));
            }
            else
            {
                servers.Add(new Text(server.Name));
            }
        }
        return new Rows(servers);
    }
}
