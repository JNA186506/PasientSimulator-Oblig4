using PasientSimulator.lib.Models;

namespace PasientSimulator.lib.Services.Interfaces;

public interface IAdministerTreatment
{
    Task<bool> AdministerMedicine(Medication medicine, Patient patient);
    Task<bool> AdministerOxygen(Patient patient);
    Task<double> DecreaseTemperature(Patient patient);
}