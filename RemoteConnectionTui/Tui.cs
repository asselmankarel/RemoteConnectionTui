using RemoteConnectionTui.Helpers;
using RemoteConnectionTui.ViewModels;
using RemoteConnectionTui.Views;
using Spectre.Console;

namespace RemoteConnectionTui;

internal sealed class Tui
{
    private readonly ServerList _serverList;
    private readonly ProcessManager _processManager = new();
    private readonly MainWindow _mainWindow;    

    public Tui(ServerList serverList)
    {
        _serverList = serverList ?? new ServerList();
        _mainWindow = new MainWindow(_serverList, _processManager);

        _mainWindow.Update();
    }

    public void Run()
    {
        Console.CursorVisible = false;

        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(_mainWindow.GetLayout());
            var keypress = Console.ReadKey();

            if (keypress.Key == ConsoleKey.Escape)
            {
                break;
            }

            HandleKeypress(keypress);
        }
    }

    private void HandleKeypress(ConsoleKeyInfo keypress)
    {
        switch (keypress.Key)
        {
            case ConsoleKey.UpArrow:
                _serverList.MoveUp();
                _mainWindow.Update();
                break;
            case ConsoleKey.DownArrow:
                _serverList.MoveDown();
                _mainWindow.Update();
                break;
            case ConsoleKey.Enter:
                _processManager.Connect(_serverList.Servers[_serverList.IndexActiveServer]);
                _mainWindow.Update();
                break;
            case ConsoleKey.N:
                var newServer = AddServer.Prompt();
                _serverList.AddServer(newServer);
                _mainWindow.Update();
                break;
            default:
                break;
        }
    }
}
