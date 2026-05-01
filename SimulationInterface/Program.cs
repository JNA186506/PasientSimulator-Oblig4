using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace SimulationInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PasientSimulator.lib;

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
        services.AddScoped<PatientService>();
        services.AddScoped<CaseService>();
        services.AddTransient<Form1>();

        var provider = services.BuildServiceProvider();

        var mainForm = provider.GetRequiredService<Form1>();
       
        Application.Run(mainForm);
        
    }
}