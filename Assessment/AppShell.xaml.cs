namespace Assessment;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(CasePage), typeof(CasePage));

    }
}