using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;
using PasientSimulator.lib.Services.Interfaces;

namespace SimulationInterface;

internal static class Program
{
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        var services = new ServiceCollection();

        services.AddDbContext<Context>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<ICaseService, CaseService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAdministerTreatment, AdministerTreatment>();
        services.AddScoped<IMedicationService, MedicationService>();
        services.AddTransient<CurrentCaseView>();
        services.AddTransient<AdministerTreatmentView>();

        services.AddSingleton<HubConnection>(_ =>
            new HubConnectionBuilder()
                .WithUrl("http://localhost:5179/caseHub")
                .WithAutomaticReconnect()
                .Build());

        var provider = services.BuildServiceProvider();

        var mainForm = provider.GetRequiredService<CurrentCaseView>();

        Application.Run(mainForm);
    }
}