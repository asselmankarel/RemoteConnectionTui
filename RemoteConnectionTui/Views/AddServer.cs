using RemoteConnectionTui.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteConnectionTui.Views;

internal static class AddServer
{
   public static Server Prompt()
    {
        AnsiConsole.Clear();
        Console.CursorVisible = true;
        AnsiConsole.Write(new Markup("Add a new server\n\n",new Style(Color.DarkCyan,decoration: Decoration.Underline)));

        var name = AnsiConsole.Prompt<string>(
            new TextPrompt<string>(" [Yellow]Server name:[/] "));

        var fqdn = AnsiConsole.Prompt<string>(
            new TextPrompt<string>(" [Yellow]Server FQDN or IP address:[/] "));

        var port = AnsiConsole.Prompt<int>(
            new TextPrompt<int>(" [Yellow]Server port:[/] ").DefaultValue(3389));

        var protocol = AnsiConsole.Prompt<ProtocolType>(
            new SelectionPrompt<ProtocolType>()
                .Title(" [Yellow]Select protocol:[/] ")
                .AddChoices(new[] {
                    ProtocolType.Rdp,
                    ProtocolType.Ssh
                }));
        AnsiConsole.Write(new Markup($"Protocol: {protocol}\n", new Style(Color.Yellow)));

        var description = AnsiConsole.Prompt<string>(
            new TextPrompt<string>(" Description: ").AllowEmpty());

        Console.CursorVisible = false;
        return new Server(name, fqdn, protocol, port, description);
    }
}
