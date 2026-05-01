using Microsoft.EntityFrameworkCore;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services.Interfaces;

namespace PasientSimulator.lib.Services;

public class CaseService : ICaseService {
    private readonly Context _context;

    public CaseService(Context context) {
        _context = context;
    }

    public async Task<Case> AddNewCase(Patient casePatient, User student, List<Goal> goals) {
        var newCase = new Case { CasePatient = casePatient, Student = student, Goals = goals };
        _context.Add(newCase);
        await _context.SaveChangesAsync();
        return newCase;
    }

    public async Task<List<Case>> GetAllCases() {
        return await _context.Cases
            .Include(c => c.CasePatient)
                .ThenInclude(p => p.Diagnoses)
            .Include(c => c.CasePatient)
                .ThenInclude(p => p.MedicalHistory)
            .Include(c => c.CasePatient)
                .ThenInclude(p => p.Medications)
            .Include(c => c.CasePatient)
                .ThenInclude(p => p.Allergies)
            .Include(c => c.CasePatient)
                .ThenInclude(p => p.BloodPressure)
            .Include(c => c.Student)
            .Include(c => c.Goals)
            .ToListAsync();
    }

    public async Task<List<Case>> GetAllCasesByUserId(int id) {
        return await _context.Cases
            .Include(c => c.CasePatient)
            .Include(c => c.Student)
            .ToListAsync();
    }

    public async Task<Case> GetCaseById(int id) {
        Case? currCase = await _context.Cases
            .Include(c => c.CasePatient)
                .ThenInclude(p => p.Diagnoses)
            .Include(c => c.CasePatient)
                .ThenInclude(p => p.MedicalHistory)
            .Include(c => c.CasePatient)
                .ThenInclude(p => p.Medications)
            .Include(c => c.CasePatient)
                .ThenInclude(p => p.Allergies)
            .Include(c => c.CasePatient)
                .ThenInclude(p => p.BloodPressure)
            .Include(c => c.Student)
            .Include(c => c.Goals)
            .FirstOrDefaultAsync(c => c.CaseId == id);

        if (currCase == null) {
            throw new KeyNotFoundException($"Could not find case with id {id}");
        }

        return currCase;
    }

    public async Task<Case> GetFirstCase(int id) {
        return await _context.Cases.FirstAsync(c => c.UserId == id);
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

    public async Task<Goal> FindGoal(int id) {
        return await _context.Goals.FindAsync(id);
    }

    public Task<Case?> GetCaseByIdAsync(int id)
    {
        return _context.Cases.FirstOrDefaultAsync(c => c.CaseId == id);
    }

}