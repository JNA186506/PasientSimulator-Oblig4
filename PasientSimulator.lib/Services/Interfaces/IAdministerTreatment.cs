using PasientSimulator.lib.Models;

namespace PasientSimulator.lib.Services.Interfaces;

public interface IAdministerTreatment {
    bool AdministerMedicine(Medication medicine, Patient patient);
    bool AdministerOxygen(Patient patient);
    double DecreaseTemperature(Patient patient);
}
