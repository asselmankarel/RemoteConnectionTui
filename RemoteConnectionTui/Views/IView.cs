using Spectre.Console;

namespace RemoteConnectionTui.Views;

internal interface IView
{
    public void Update();
    public Layout GetLayout();
}
