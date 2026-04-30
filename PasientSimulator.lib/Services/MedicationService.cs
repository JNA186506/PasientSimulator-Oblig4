using PasientSimulator.lib.Models;

namespace PasientSimulator.lib.Services;

public class MedicationService {
    private readonly Context _context;

    public MedicationService(Context context) {
        _context = context;
    }

    public List<Medication> GetAllMedications() {
        return _context.Medications.ToList();
    }

    public async Task<Medication> FindMedication(int id) {
        return await _context.Medications.FindAsync(id);
    }

    public Medication AddMedication(string name) {
        Medication newMedication = new Medication { MedicationName = name };

        _context.Add(newMedication);
        _context.SaveChanges();

        return newMedication;
    }

    public bool IsAntidote(Medication medication, Patient patient) {
        List<Illness> patientDiagnoses = patient.Diagnoses;

        return patientDiagnoses.Any(m => m.AntidoteId == medication.MedicationId);
    }

}