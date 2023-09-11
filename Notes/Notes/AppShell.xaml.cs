using Notes.Views;

namespace Notes;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(NotePage), typeof(NotePage));
    }
}