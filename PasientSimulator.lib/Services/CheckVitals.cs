using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services.Interfaces;

namespace PasientSimulator.lib.Services;

public class CheckVitals : ICheckVitals {

    public int CheckHR(Patient patient){
    return patient.Heartrate;
    }

    public int CheckRespiratoryRate(Patient patient){
    return patient.RespiratoryRate;
    }

    public double CheckOxygenSaturation(Patient patient){
    return patient.OxygenSaturation;
    }

    public double CheckTemperature(Patient patient){
    return patient.Temperature;
    }

    public (int Systolic, int Diastolic) CheckBloodPressure(Patient patient) {
        return (patient.Bloodpressure.Systolic, patient.Bloodpressure.Diastolic);
    }
}