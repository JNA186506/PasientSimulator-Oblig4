using Microsoft.EntityFrameworkCore;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services.Interfaces;
using System.Linq;

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

    public async Task UpdatePatient(Patient patient)
    {
        ArgumentNullException.ThrowIfNull(patient, nameof(patient));

        // Load the tracked patient including navigation collections we will update
        var existing = await _context.Patients
            .Include(p => p.Diagnoses)
            .Include(p => p.Allergies)
            .Include(p => p.BloodPressure)
            .FirstOrDefaultAsync(p => p.PatientId == patient.PatientId);

        if (existing == null) throw new KeyNotFoundException($"Patient {patient.PatientId} not found");

        // Map scalar and simple properties
        existing.PatientName = patient.PatientName;
        existing.Weight = patient.Weight;
        existing.Age = patient.Age;
        existing.Sex = patient.Sex;
        existing.Status = patient.Status;
        existing.Heartrate = patient.Heartrate;
        existing.RespiratoryRate = patient.RespiratoryRate;
        existing.OxygenSaturation = patient.OxygenSaturation;
        existing.Temperature = patient.Temperature;

        // Owned type: BloodPressure
        if (patient.BloodPressure != null)
        {
            existing.BloodPressure ??= new BloodPressure();
            existing.BloodPressure.Systolic = patient.BloodPressure.Systolic;
            existing.BloodPressure.Diastolic = patient.BloodPressure.Diastolic;
        }

        // Replace Diagnoses with tracked Illness instances fetched by id
        var diagIds = (patient.Diagnoses ?? new List<Illness>()).Select(i => i.IllnessId).Distinct().ToList();
        existing.Diagnoses ??= new List<Illness>();
        existing.Diagnoses.Clear();
        if (diagIds.Any())
        {
            var trackedIllnesses = await _context.Illnesses.Where(i => diagIds.Contains(i.IllnessId)).ToListAsync();
            foreach (var t in trackedIllnesses) existing.Diagnoses.Add(t);
        }

        // Replace Allergies with tracked Medication instances fetched by id
        var medIds = (patient.Allergies ?? new List<Medication>()).Select(m => m.MedicationId).Distinct().ToList();
        existing.Allergies ??= new List<Medication>();
        existing.Allergies.Clear();
        if (medIds.Any())
        {
            var trackedMeds = await _context.Medications.Where(m => medIds.Contains(m.MedicationId)).ToListAsync();
            foreach (var m in trackedMeds) existing.Allergies.Add(m);
        }

        await _context.SaveChangesAsync();
    }
}