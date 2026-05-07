using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace Assessment;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddDbContext<Context>();
        builder.Services.AddScoped<CaseService>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddSingleton<HubConnection>(_ => 
            new HubConnectionBuilder()
                .WithUrl("http://localhost:5179/caseHub")
                .WithAutomaticReconnect()
                .Build()
        );
        

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}