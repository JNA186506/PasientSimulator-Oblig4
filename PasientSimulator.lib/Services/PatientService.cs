using Microsoft.EntityFrameworkCore;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services.Interfaces;

namespace PasientSimulator.lib.Services;

public class PatientService : IPatientService
{
    private readonly Context _context;

    public PatientService(Context context)
    {
        _context = context;
    }

    public async Task<List<Patient>> GetAllPatients()
    {
        return await _context.Patients.ToListAsync();
    }

    public async Task<Patient> AddNewPatient(Patient patient)
    {
        foreach (var illness in patient.Diagnoses)
            _context.Illnesses.Attach(illness);

        foreach (var medication in patient.Allergies)
            _context.Medications.Attach(medication);

        patient.BloodPressure ??= new BloodPressure
        {
            Systolic = 120, Diastolic = 80
        };

        _context.Add(patient);
        await _context.SaveChangesAsync();

        return patient;
    }

    public async Task<bool> AddIllness(Illness illness, Patient patient)
    {
        patient.Diagnoses.Add(illness);
        _context.Update(patient);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Illness>> GetPatientIllesses(Patient patient)
    {
        var p = await _context.Patients
            .Include(p => p.Diagnoses)
            .FirstOrDefaultAsync(p => p.PatientId == patient.PatientId);

        if (p == null) throw new KeyNotFoundException("Something went wrong while trying to fetch diagnoses");

        return patient.Diagnoses;
    }

    public async Task<List<Illness>> GetAllDiagnoses()
    {
        return await _context.Illnesses.ToListAsync();
    }

    public async Task<List<Medication>> GetAllAllergies()
    {
        return await _context.Medications.ToListAsync();
    }

    public async Task<Illness> FindIllness(int id)
    {
        var illness = await _context.Illnesses.FindAsync(id);

        if (illness == null)
            throw new KeyNotFoundException($"Illness with {id} was not found");

        return illness;
    }
}