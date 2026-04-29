using Microsoft.EntityFrameworkCore;
using PasientSimulator.lib.Models;

namespace PasientSimulator.lib.Services;

public class CaseService {
    private Context _context;

    public CaseService(Context context) {
        _context = context;
    }

    public async Task<Case> AddNewCase(int id, Patient casePatient, User student, List<Goal> goals) {
        var newCase = new Case { CaseId = id, CasePatient = casePatient, Student = student, Goals = goals };
        _context.Add(newCase);
        await _context.SaveChangesAsync();
        return newCase;
    }

    public async Task<List<Case>> GetAllCases() {
        return await _context.Cases
            .Include(p => p.CasePatient)
            .Include(s => s.Student)
            .ToListAsync();
    }

    public async Task<Goal> MakeGoal(int currCaseId, string goalName, int timeLimit, string description) {
        Goal newGoal = new Goal
            { CaseId = currCaseId, GoalName = goalName,
                Description = description, TimeLimit = timeLimit };

        _context.Add(newGoal);
        await _context.SaveChangesAsync();
        
        return newGoal;
    }

    public async Task<List<Goal>> GetAllGoals() {
        return await _context.Goals.ToListAsync();
    }
}