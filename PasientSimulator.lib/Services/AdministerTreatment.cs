using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services.Interfaces;

namespace PasientSimulator.lib.Services;

public class AdministerTreatment : IAdministerTreatment
{

    private readonly Context _context;

    public AdministerTreatment(Context context)
    {
        _context = context; 
    }
    public async Task<bool> AdministerMedicine(Medication medicine, Patient patient)
    {
        var isAllergic = patient.Allergies.Any(m => m.MedicationId == medicine.MedicationId);

        if (isAllergic)
        {
            patient.Status = Patient.StatusEnum.SeverelySick;
            await _context.SaveChangesAsync();
            return false;
        }

        var isCured = patient.Diagnoses.Any(m => m.Antidote.MedicationId == medicine.MedicationId);

        if (!isCured) return false;

        patient.Status = Patient.StatusEnum.Healthy;
        await _context.SaveChangesAsync();
        return true;
    }

    /**
     * This method will administer 10 units of oxygen.
     * If the Patient did not get administered oxygen, the method will return false.
     * If the Patient is already saturated with oxygen, it cannot receive more and the method will return false.
     */
    public async Task<bool> AdministerOxygen(Patient patient)
    {
        var patientIsSaturated = patient.OxygenSaturation > 99;

        if (patientIsSaturated) return false;

        patient.OxygenSaturation += 10;
        await _context.SaveChangesAsync();
        return true;
    }

    /**
     * This method decreases the patients' temperature.
     */
    public async Task<double> DecreaseTemperature(Patient patient)
    {
        var newTemp = patient.Temperature -= 10;

        patient.Temperature = newTemp;
        await _context.SaveChangesAsync();
        return patient.Temperature;
    }
}