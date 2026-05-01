using PasientSimulator.lib.Models;

namespace PasientSimulator.lib.Services.Interfaces;

public interface IPatientService {
    Task<List<Patient>> GetAllPatients();
    Task<Patient> AddNewPatient(Patient patient);
    Task<bool> AddIllness(Illness illness, Patient patient);
    Task<List<Illness>> GetPatientIllesses(Patient patient);
    Task<List<Illness>> GetAllDiagnoses();
    Task<List<Medication>> GetAllAllergies();
    Task<Illness> FindIllness(int id);
}
