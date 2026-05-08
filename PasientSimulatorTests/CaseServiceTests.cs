using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace PasientSimulatorTests;

[TestFixture]
public class CaseServiceTests
{
    private Context _context;
    private CaseService _service;
    private Patient _patient;
    private User _student;

    [SetUp]
    public async Task Setup()
    {
        _context = TestContextFactory.Create();
        _service = new CaseService(_context);

        _student = new User
        {
            Role  = 1,
            Name  = "Student One",
            Cases = new List<Case>()
        };
        _patient = new Patient
        {
            PatientName      = "Jane Doe",
            Age              = 30,
            Weight           = 65,
            Sex              = Patient.SexEnum.Female,
            Status           = Patient.StatusEnum.Sick,
            Heartrate        = 90,
            BloodPressure    = new BloodPressure { Systolic = 130, Diastolic = 85 },
            RespiratoryRate  = 17,
            OxygenSaturation = 96.0,
            Temperature      = 38.0,
            Diagnoses        = new List<Illness>    { },
            MedicalHistory   = new List<Illness>    { },
            Medications      = new List<Medication> { },
            Allergies        = new List<Medication> { }
        };

        _context.Users.Add(_student);
        _context.Patients.Add(_patient);
        await _context.SaveChangesAsync();
    }

    [TearDown]
    public void TearDown() => _context.Dispose();


    [Test]
    public async Task AddNewCase_ReturnsCreatedCase()
    {
        var goals = new List<Goal>
        {
            new Goal { GoalName = "Stabilise vitals", Description = "Get obs normal", TimeLimit = 10 }
        };

        var result = await _service.AddNewCase(_patient, _student, goals);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.CaseId, Is.GreaterThan(0));
    }

    [Test]
    public async Task AddNewCase_CaseHasCorrectPatientAndStudent()
    {
        var result = await _service.AddNewCase(_patient, _student, new List<Goal>());

        Assert.That(result.CasePatient.PatientId, Is.EqualTo(_patient.PatientId));
        Assert.That(result.Student.UserId,        Is.EqualTo(_student.UserId));
    }

    [Test]
    public async Task AddNewCase_CaseGoalsArePersisted()
    {
        var goals = new List<Goal>
        {
            new Goal { GoalName = "Give oxygen",     Description = "O2 > 98%", TimeLimit = 5  },
            new Goal { GoalName = "Lower heart rate", Description = "< 80 bpm", TimeLimit = 10 }
        };

        var result = await _service.AddNewCase(_patient, _student, goals);

        Assert.That(result.Goals, Has.Count.EqualTo(2));
    }


    [Test]
    public async Task GetAllCases_WhenEmpty_ReturnsEmptyList()
    {
        var result = await _service.GetAllCases();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAllCases_ReturnsAllAddedCases()
    {
        await _service.AddNewCase(_patient, _student, new List<Goal>());
        await _service.AddNewCase(_patient, _student, new List<Goal>());

        var result = await _service.GetAllCases();

        Assert.That(result, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task GetAllCases_IncludesPatientData()
    {
        await _service.AddNewCase(_patient, _student, new List<Goal>());

        var result = await _service.GetAllCases();

        Assert.That(result[0].CasePatient, Is.Not.Null);
        Assert.That(result[0].CasePatient.PatientName, Is.EqualTo("Jane Doe"));
    }


    [Test]
    public async Task GetCaseById_ValidId_ReturnsCase()
    {
        var created = await _service.AddNewCase(_patient, _student, new List<Goal>());

        var result = await _service.GetCaseById(created.CaseId);

        Assert.That(result.CaseId, Is.EqualTo(created.CaseId));
    }

    [Test]
    public async Task GetCaseById_ValidId_IncludesPatient()
    {
        var created = await _service.AddNewCase(_patient, _student, new List<Goal>());

        var result = await _service.GetCaseById(created.CaseId);

        Assert.That(result.CasePatient, Is.Not.Null);
    }

    [Test]
    public void GetCaseById_InvalidId_ThrowsKeyNotFoundException()
    {
        Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _service.GetCaseById(99999));
    }

    // ── GetAllCasesByUserId ─────────────────────────────────────────────────

    /// <summary>
    /// Documents a known bug: GetAllCasesByUserId ignores the id parameter
    /// and returns every case in the database instead of filtering by user.
    /// This test will FAIL until the filter is implemented.
    /// </summary>
    [Test]
    public async Task GetAllCasesByUserId_ReturnsOnlyCasesForGivenUser()
    {
        var secondStudent = new User { Role = 1, Name = "Student Two", Cases = new List<Case>() };
        _context.Users.Add(secondStudent);
        await _context.SaveChangesAsync();

        await _service.AddNewCase(_patient, _student,       new List<Goal>());
        await _service.AddNewCase(_patient, secondStudent,  new List<Goal>());

        var result = await _service.GetAllCasesByUserId(_student.UserId);

        Assert.That(result, Has.Count.EqualTo(1),
            "GetAllCasesByUserId should filter by UserId — currently returns all cases (known bug).");
        Assert.That(result[0].UserId, Is.EqualTo(_student.UserId));
    }


    [Test]
    public async Task MakeGoal_CreatesGoalWithCorrectFields()
    {
        var created = await _service.AddNewCase(_patient, _student, new List<Goal>());

        var goal = await _service.MakeGoal(created.CaseId, "Administer oxygen", 5, "Raise O2 above 98%");

        Assert.That(goal.GoalName,    Is.EqualTo("Administer oxygen"));
        Assert.That(goal.TimeLimit,   Is.EqualTo(5));
        Assert.That(goal.Description, Is.EqualTo("Raise O2 above 98%"));
        Assert.That(goal.CaseId,      Is.EqualTo(created.CaseId));
    }

    [Test]
    public async Task MakeGoal_GoalIsPersisted()
    {
        var created = await _service.AddNewCase(_patient, _student, new List<Goal>());

        var goal = await _service.MakeGoal(created.CaseId, "Check BP", 3, "BP under 130/85");

        Assert.That(goal.GoalId, Is.GreaterThan(0));
    }


    [Test]
    public async Task AddEvent_ReturnsPersistentEventWithId()
    {
        var created = await _service.AddNewCase(_patient, _student, new List<Goal>());

        var ev = await _service.AddEvent(new Event
        {
            CaseId      = created.CaseId,
            UserId      = _student.UserId,
            EventType   = EventEnum.Comment,
            Description = "Patient seems calmer"
        });

        Assert.That(ev.EventId, Is.GreaterThan(0));
        Assert.That(ev.Description, Is.EqualTo("Patient seems calmer"));
    }

    [Test]
    public async Task GetEventsById_ReturnsOnlyEventsForThatCase()
    {
        var case1 = await _service.AddNewCase(_patient, _student, new List<Goal>());
        var case2 = await _service.AddNewCase(_patient, _student, new List<Goal>());

        await _service.AddEvent(new Event
            { CaseId = case1.CaseId, UserId = _student.UserId,
              EventType = EventEnum.Comment, Description = "Case 1 comment" });
        await _service.AddEvent(new Event
            { CaseId = case2.CaseId, UserId = _student.UserId,
              EventType = EventEnum.MedicalIntervention, Description = "Case 2 treatment" });

        var events = await _service.GetEventsById(case1.CaseId);

        Assert.That(events, Has.Count.EqualTo(1));
        Assert.That(events[0].CaseId, Is.EqualTo(case1.CaseId));
    }

    [Test]
    public async Task GetEventsById_WhenNoEvents_ReturnsEmptyList()
    {
        var created = await _service.AddNewCase(_patient, _student, new List<Goal>());

        var events = await _service.GetEventsById(created.CaseId);

        Assert.That(events, Is.Empty);
    }

    [Test]
    public async Task GetEventsById_MultipleEventsOnSameCase_ReturnsAll()
    {
        var created = await _service.AddNewCase(_patient, _student, new List<Goal>());

        await _service.AddEvent(new Event
            { CaseId = created.CaseId, UserId = _student.UserId,
              EventType = EventEnum.Comment, Description = "First comment" });
        await _service.AddEvent(new Event
            { CaseId = created.CaseId, UserId = _student.UserId,
              EventType = EventEnum.MedicalIntervention, Description = "Gave oxygen" });

        var events = await _service.GetEventsById(created.CaseId);

        Assert.That(events, Has.Count.EqualTo(2));
    }
}
