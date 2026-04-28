using PasientSimulator_Oblig4_lib.Models;

namespace PasientSimulator_Oblig4_lib.Services;

public class CheckVitals {

    public int checkHR(Patient patient){
    return patient.getHeartrate();
    }

    public int CheckRespiratoryRate(Patient patient){
    return patient.getRespiratoryRate();
    }

    public double CheckOxygenSaturation(Patient patient){
    return patient.getOxygenSaturation();
    }

    public double CheckTemperature(Patient patient){
    return patient.getTemperature();
    }

    public (int Systolic, int Diastolic) CheckBloodPressure(Patient patient){
        Systolic = patient.BloodPressure.getSystolic();
        Diastolic = patient.BloodPressure.GetDiastolic();
        return (Systolic, Diastolic);
    }
}