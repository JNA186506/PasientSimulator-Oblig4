using PasientSimulator.lib.Models;

namespace PasientSimulator.lib.Services.Interfaces;

public interface ICheckVitals {
    int CheckHR(Patient patient);
    int CheckRespiratoryRate(Patient patient);
    double CheckOxygenSaturation(Patient patient);
    double CheckTemperature(Patient patient);
    (int Systolic, int Diastolic) CheckBloodPressure(Patient patient);
}
