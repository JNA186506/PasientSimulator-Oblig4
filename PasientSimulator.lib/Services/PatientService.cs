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
            PatientName = patientName, Weight = weight, Sex = (Patient.SexEnum)sex, Status = (Patient.StatusEnum)status, Heartrate = heartrate,
            RespiratoryRate = respiratoryRate, Temperature = temperature, Diagnoses = diagnoses, Allergies = allergies
        };

        _context.Add(newPatient);
        _context.SaveChanges();
        
        return newPatient;
    }
}