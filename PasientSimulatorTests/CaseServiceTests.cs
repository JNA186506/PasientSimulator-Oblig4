using Microsoft.EntityFrameworkCore;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace PasientSimulatorTests;

public class CaseServiceTests {
    private Context _context;
    private CaseService _caseService;

    [SetUp]
    public void Setup() {
        var options = new DbContextOptionsBuilder<Context>()
            .UseSqlite("DataSource=:memory:")
            .Options;
        _context = new Context(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
        _caseService = new CaseService(_context);
    }

    [TearDown]
    public void TearDown() {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetAllCases_ShouldNotThrowInvalidCast() {
        // Arrange
        var patient = new Patient { 
            PatientId = 1, 
            PatientName = "Test Patient",
            Bloodpressure = new BloodPressure { Systolic = 120, Diastolic = 80 }
        };
        var student = new User { UserId = 1, Name = "Test Student" };
        var goals = new List<Goal> { new Goal { GoalName = "Goal 1" } };
        
        await _caseService.AddNewCase(1, patient, student, goals);

        // Act & Assert
        var cases = await _caseService.GetAllCases();
        Assert.That(cases, Has.Count.EqualTo(1));
    }
}
