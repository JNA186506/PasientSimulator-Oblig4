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

    public async Task<Patient> AddNewPatient(
        string patientName, int weight, int age, int sex, int status, int heartrate,
        int respiratoryRate, double temperature, List<Illness> diagnoses, List<Medication> allergies) {

        Patient newPatient = new Patient {
            PatientName = patientName, Weight = weight, Age = age, Sex = (Patient.SexEnum)sex, Status = (Patient.StatusEnum)status, Heartrate = heartrate,
            RespiratoryRate = respiratoryRate, Temperature = temperature, Diagnoses = diagnoses, Allergies = allergies
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

    public async Task<List<Illness>> GetAllDiagnosis() {
        return await _context.Illnesses.ToListAsync();
    }

    public async Task<List<Medication>> GetAllAllergies()
    {
        return await _context.Medications.ToListAsync();
    }
}