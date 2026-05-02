using PasientSimulator.lib.Models;

namespace PasientSimulator.lib.Services.Interfaces;

public interface IMedicationService
{
    Task<List<Medication>> GetAllMedications();
    Task<Medication> FindMedication(int id);
    Medication AddMedication(string name);
    bool IsAntidote(Medication medication, Patient patient);
}