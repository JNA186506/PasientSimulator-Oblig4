using Microsoft.EntityFrameworkCore;
using PasientSimulator.lib.Models;

namespace PasientSimulator.lib.Services;

public class PatientService {
    private Context _context;

    public PatientService(Context context) {
        _context = context;
    }

    public async Task<List<Patient>> GetAllPatients() {
        return await _context.Patients.ToListAsync();
    }

    public async Task<Patient> AddNewPatient(string name, int weight, int age, int sex, int status, int heartrate, BloodPressure bloodPressure, int respiratoryrate, int temperature, List<Illness> diagnoses, List<Medication> allergies) {
        foreach (var illness in diagnoses)
            _context.Illnesses.Attach(illness);

        foreach (var medication in allergies)
            _context.Medications.Attach(medication); 
        Patient newPatient = new Patient {
            PatientName = name, Weight = weight, Age = age, Sex = (Patient.SexEnum)sex, Status = (Patient.StatusEnum) status, Heartrate = heartrate, BloodPressure = bloodPressure, RespiratoryRate = respiratoryrate, Temperature = temperature, Diagnoses = diagnoses, Allergies = allergies
        };
        
        _context.Add(newPatient);
        await _context.SaveChangesAsync();
        
        return newPatient;
    }

    public async Task<bool> AddIllness(Illness illness, Patient patient) {
        patient.Diagnoses.Add(illness);
        _context.Update(patient);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Illness>> GetPatientIllesses(Patient patient) {
        Patient? p = await _context.Patients
            .Include(p => p.Diagnoses)
            .FirstOrDefaultAsync(p => p.PatientId == patient.PatientId);
        
        if (p == null) {
            throw new KeyNotFoundException();
        }

        return patient.Diagnoses;
    }

    public async Task<List<Illness>> GetAllDiagnoses() {
        return await _context.Illnesses.ToListAsync();
    }

    public async Task<List<Medication>> GetAllAllergies()
    {
        return await _context.Medications.ToListAsync();
    }

    public async Task<Illness> FindIllness(int id)
    {
        return await _context.Illnesses.FindAsync(id);
    }
}