using Microsoft.EntityFrameworkCore;
using PasientSimulator.lib.Models;

namespace PasientSimulator.lib.Services;

public class PatientService {
    private Context _context;

    public PatientService(Context context) {
        _context = context;
    }

    public List<Patient> GetAllPatients() {
        return _context.Patients.ToList();
    }

    public Patient AddNewPatient(
        string patientName, int weight, int age, int sex, int status, int heartrate,
        int respiratoryRate, double temperature, List<Illness> diagnoses, List<Medication> allergies) {

        Patient newPatient = new Patient {
            PatientName = patientName, Weight = weight, Age = age, Sex = (Patient.SexEnum)sex, Status = (Patient.StatusEnum)status, Heartrate = heartrate,
            RespiratoryRate = respiratoryRate, Temperature = temperature, Diagnoses = diagnoses, Allergies = allergies
        };

        _context.Add(newPatient);
        _context.SaveChanges();
        
        return newPatient;
    }

    public bool AddIllness(Illness illness, Patient patient) {
        patient.Diagnoses.Add(illness);
        _context.Update(patient);
        _context.SaveChanges();
        return true;
    }

    public List<Illness> GetPatientIllesses(Patient patient) {
        Patient? p = _context.Patients
            .Include(p => p.Diagnoses)
            .FirstOrDefault(p => p.PatientId == patient.PatientId);
        
        if (p == null) {
            throw new KeyNotFoundException();
        }

        return patient.Diagnoses;
    }
}