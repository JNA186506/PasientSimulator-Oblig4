namespace Assessment;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();    
        // Register route for CasePage so we can navigate to it by name and pass complex objects
        Microsoft.Maui.Controls.Routing.RegisterRoute(nameof(CasePage), typeof(CasePage));

    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}