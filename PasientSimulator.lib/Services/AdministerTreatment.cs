using PasientSimulator_Oblig4_lib.Models;

namespace PasientSimulator_Oblig4_lib.Services;

public class AdministerTreatment {

    public bool AdministerMedicine(Medication medicine, Patient patient) {

        bool isAllergic = patient.Allergies.Any(m => m.MedicationId == medicine.MedicationId);

        if (isAllergic) {
            patient.Status = Patient.StatusEnum.SeverelySick;
            return false;
        }

        return true;
    }
}