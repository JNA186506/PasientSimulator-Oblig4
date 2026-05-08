using Microsoft.EntityFrameworkCore;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services.Interfaces;

namespace PasientSimulator.lib.Services;

public class CaseService : ICaseService
{
    private readonly Context _context;

    public CaseService(Context context)
    {
        _context = context;
    }

    public async Task<Case> AddNewCase(Patient casePatient, User student, List<Goal> goals)
    {
        var newCase = new Case { CasePatient = casePatient, Student = student, Goals = goals };
        _context.Add(newCase);
        await _context.SaveChangesAsync();
        return newCase;
    }

    public async Task<List<Case>> GetAllCases()
    {
        return await _context.Cases
            .AsNoTracking()
            .Include(c => c.CasePatient)
            .Include(c => c.Student)
            .Include(c => c.Goals)
            .ToListAsync();
    }

    public async Task<List<Case>> GetAllCasesByUserId(int id)
    {
        return await _context.Cases
            .Include(c => c.CasePatient)
            .Include(c => c.Student)
            .ToListAsync();
    }

    public async Task<Case> GetCaseById(int id)
    {
        var currCase = await _context.Cases
            .AsNoTracking()
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

        if (currCase == null) throw new KeyNotFoundException($"Could not find case with id {id}");

        return currCase;
    }

    public async Task<Case> GetFirstCase(int id)
    {
        return await _context.Cases.FirstAsync(c => c.UserId == id);
    }

    public async Task<Goal> MakeGoal(int currCaseId, string goalName, int timeLimit, string description)
    {
        var newGoal = new Goal
        {
            CaseId = currCaseId, GoalName = goalName,
            Description = description, TimeLimit = timeLimit
        };

        _context.Add(newGoal);
        await _context.SaveChangesAsync();

        return newGoal;
    }

    public async Task<List<Goal>> GetAllGoals()
    {
        return await _context.Goals.ToListAsync();
    }

    public async Task<Goal> FindGoal(int id)
    {
        return await _context.Goals.FindAsync(id);
    }

    public async Task<Event> AddEvent(Event newEvent)
    {
        _context.Add(newEvent);

        await _context.SaveChangesAsync();

        return newEvent;
    }

    public async Task<List<Event>> GetEventsById(int caseId)
    {
        return await _context.Events
            .Where(e => e.CaseId == caseId)
            .ToListAsync();
    }

    public Task<Case?> GetCaseByIdAsync(int id)
    {
        return _context.Cases.FirstOrDefaultAsync(c => c.CaseId == id);
    }

    public async Task UpdateCase(Case Case)
    {
        ArgumentNullException.ThrowIfNull(Case, nameof(Case));

        var existingCase = await _context.Cases
            .Include(c => c.CasePatient)
                .ThenInclude(p => p.Allergies)
            .Include(c => c.Student)
            .Include(c => c.Goals)
            .FirstOrDefaultAsync(p => p.CaseId == Case.CaseId);

        if (existingCase == null) throw new KeyNotFoundException($"Case {Case.CaseId} not found");

        existingCase.PatientId = Case.PatientId;
        existingCase.UserId = Case.UserId;

        // Attach tracked patient if it exists
        if (Case.CasePatient?.PatientId > 0)
        {
            var trackedPatient = await _context.Patients
                .Include(p => p.Allergies)
                .FirstOrDefaultAsync(p => p.PatientId == Case.CasePatient.PatientId);
            if (trackedPatient != null) existingCase.CasePatient = trackedPatient;
        }

        // Attach tracked student if present
        if (Case.Student?.UserId > 0)
        {
            var trackedStudent = await _context.Users.FindAsync(Case.Student.UserId);
            if (trackedStudent != null) existingCase.Student = trackedStudent;
        }

        // Goals (keep your existing safe pattern)
        var goalsId = (Case.Goals ?? new List<Goal>()).Select(g => g.GoalId).Distinct().ToList();
        existingCase.Goals ??= new List<Goal>();
        existingCase.Goals.Clear();
        if (goalsId.Any())
        {
            var trackedGoals = await _context.Goals.Where(g => goalsId.Contains(g.GoalId)).ToListAsync();
            foreach (var g in trackedGoals) existingCase.Goals.Add(g);
        }

        await _context.SaveChangesAsync();
    }
    }