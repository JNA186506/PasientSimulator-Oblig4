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
}