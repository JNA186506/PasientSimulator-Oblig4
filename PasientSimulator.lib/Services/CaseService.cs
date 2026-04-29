using Microsoft.EntityFrameworkCore;
using PasientSimulator.lib.Models;

namespace PasientSimulator.lib.Services;

public class CaseService {
    private Context _context;

    public CaseService(Context context) {
        _context = context;
    }

    public Case AddNewCase(int id, Patient casePatient, User student, List<Goal> goals) {
        var newCase = new Case { CaseId = id, CasePatient = casePatient, Student = student, Goals = goals };
        _context.Add(newCase);
        _context.SaveChanges();
        return newCase;
    }

    public List<Case> GetAllCases() {
        return _context.Cases.ToList();
    }

    public Goal MakeGoal(int currCaseId, string goalName, int timeLimit, string description) {
        Goal newGoal = new Goal
            { CaseId = currCaseId, GoalName = goalName,
                Description = description, TimeLimit = timeLimit };

        _context.Add(newGoal);
        _context.SaveChanges();
        
        return newGoal;
    }

    public List<Goal> GetAllGoals() {
        return _context.Goals.ToList();
    }
}