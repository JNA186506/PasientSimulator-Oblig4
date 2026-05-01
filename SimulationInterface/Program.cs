using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;
using PasientSimulator.lib.Services.Interfaces;

namespace SimulationInterface;
using Microsoft.Extensions.DependencyInjection;


static class Program {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        var services = new ServiceCollection();

        services.AddDbContext<Context>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<ICaseService, CaseService>();
        services.AddScoped<IUserService, UserService>();
        services.AddTransient<CurrentCaseView>();

        var provider = services.BuildServiceProvider();

        var mainForm = provider.GetRequiredService<CurrentCaseView>();
       
        Application.Run(mainForm);
        
    }
}