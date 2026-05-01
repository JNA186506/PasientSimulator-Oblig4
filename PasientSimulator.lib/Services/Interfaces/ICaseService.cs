using PasientSimulator.lib.Models;

namespace PasientSimulator.lib.Services.Interfaces;

public interface ICaseService {
    Task<Case> AddNewCase(Patient casePatient, User student, List<Goal> goals);

    Task<List<Case>> GetAllCases();
    Task<List<Case>> GetAllCasesByUserId(int id);
    Task<Case> GetCaseById(int id);
    Task<Case> GetFirstCase(int id);
    Task<Goal> MakeGoal(int currCaseId, string goalName, int timeLimit, string description);
    Task<List<Goal>> GetAllGoals();
    Task<Goal> FindGoal(int id);
    Task<Case?> GetCaseByIdAsync(int id);
}
