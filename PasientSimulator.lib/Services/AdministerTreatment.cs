using PasientSimulator.lib.Models;

namespace PasientSimulator.lib.Services;

public static class AdministerTreatment {

    public static bool AdministerMedicine(Medication medicine, Patient patient) {

        bool isAllergic = patient.Allergies.Any(m => m.MedicationId == medicine.MedicationId);

        if (isAllergic) {
            patient.Status = Patient.StatusEnum.SeverelySick;
            return false;
        }

        bool isCured = patient.Diagnoses.Any(m => m.Antidote.MedicationId == medicine.MedicationId);

        if (!isCured) {
            return false;
        }
        
        patient.Status = Patient.StatusEnum.Healthy;
        return true;
    }

    /**
     * This method will administer 10 units of oxygen.
     * If the Patient did not get administered oxygen, the method will return false.
     * If the Patient is already saturated with oxygen, it cannot receive more and the method will return false.
     */
    public static bool AdministerOxygen(Patient patient) {
        bool patientIsSaturated = patient.OxygenSaturation > 99;

        if (patientIsSaturated) {
            return false;
        }

        patient.OxygenSaturation += 10;
        return true;
    }

    /**
     * This method decreases the patients' temperature.
     */
    public static double DecreaseTemperature(Patient patient) {
        double newTemp = patient.Temperature -= 10;

        patient.Temperature = newTemp;

        return patient.Temperature;
    }
    
}