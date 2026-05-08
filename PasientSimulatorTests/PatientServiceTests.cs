using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace PasientSimulatorTests;

[TestFixture]
public class PatientServiceTests
{
    private Context _context;
    private PatientService _service;

    [SetUp]
    public void Setup()
    {
        _context = TestContextFactory.Create();
        _service = new PatientService(_context);
    }

    [TearDown]
    public void TearDown() => _context.Dispose();

    // ── Helpers ─────────────────────────────────────────────────────────────

    private static Patient MakePatient(string name = "Test Patient") => new()
    {
        PatientName      = name,
        Age              = 45,
        Weight           = 75,
        Sex              = Patient.SexEnum.Male,
        Status           = Patient.StatusEnum.Sick,
        Heartrate        = 85,
        BloodPressure    = new BloodPressure { Systolic = 128, Diastolic = 84 },
        RespiratoryRate  = 17,
        OxygenSaturation = 96.0,
        Temperature      = 37.8,
        Diagnoses        = new List<Illness>    { },
        MedicalHistory   = new List<Illness>    { },
        Medications      = new List<Medication> { },
        Allergies        = new List<Medication> { }
    };

    // ── GetAllPatients ───────────────────────────────────────────────────────

    [Test]
    public async Task GetAllPatients_WhenEmpty_ReturnsEmptyList()
    {
        var result = await _service.GetAllPatients();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAllPatients_ReturnsAllAddedPatients()
    {
        _context.Patients.Add(MakePatient("Alice"));
        _context.Patients.Add(MakePatient("Bob"));
        await _context.SaveChangesAsync();

        var result = await _service.GetAllPatients();

        Assert.That(result, Has.Count.EqualTo(2));
    }

    // ── AddNewPatient ────────────────────────────────────────────────────────

    [Test]
    public async Task AddNewPatient_ReturnsSavedPatientWithId()
    {
        var patient = MakePatient("New Patient");

        var result = await _service.AddNewPatient(patient);

        Assert.That(result.PatientId, Is.GreaterThan(0));
        Assert.That(result.PatientName, Is.EqualTo("New Patient"));
    }

    [Test]
    public async Task AddNewPatient_WhenBloodPressureIsNull_SetsDefaultBloodPressure()
    {
        var patient = MakePatient();
        patient.BloodPressure = null!;

        var result = await _service.AddNewPatient(patient);

        Assert.That(result.BloodPressure,            Is.Not.Null);
        Assert.That(result.BloodPressure.Systolic,   Is.EqualTo(120));
        Assert.That(result.BloodPressure.Diastolic,  Is.EqualTo(80));
    }

    [Test]
    public async Task AddNewPatient_WhenBloodPressureIsProvided_KeepsOriginalValues()
    {
        var patient = MakePatient();
        patient.BloodPressure = new BloodPressure { Systolic = 145, Diastolic = 95 };

        var result = await _service.AddNewPatient(patient);

        Assert.That(result.BloodPressure.Systolic,  Is.EqualTo(145));
        Assert.That(result.BloodPressure.Diastolic, Is.EqualTo(95));
    }

    [Test]
    public async Task AddNewPatient_PatientIsPersisted()
    {
        var patient = MakePatient("Persisted Patient");

        await _service.AddNewPatient(patient);

        var allPatients = await _service.GetAllPatients();
        Assert.That(allPatients, Has.Count.EqualTo(1));
        Assert.That(allPatients[0].PatientName, Is.EqualTo("Persisted Patient"));
    }

    // ── FindIllness ──────────────────────────────────────────────────────────

    [Test]
    public async Task FindIllness_ValidId_ReturnsIllness()
    {
        var illness = new Illness { IllnessId = 10, IllnessName = "Pneumonia" };
        _context.Illnesses.Add(illness);
        await _context.SaveChangesAsync();

        var result = await _service.FindIllness(10);

        Assert.That(result.IllnessName, Is.EqualTo("Pneumonia"));
    }

    [Test]
    public void FindIllness_InvalidId_ThrowsKeyNotFoundException()
    {
        Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _service.FindIllness(99999));
    }

    // ── GetAllDiagnoses ──────────────────────────────────────────────────────

    [Test]
    public async Task GetAllDiagnoses_WhenEmpty_ReturnsEmptyList()
    {
        var result = await _service.GetAllDiagnoses();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAllDiagnoses_ReturnsSeededIllnesses()
    {
        _context.Illnesses.Add(new Illness { IllnessName = "Flu" });
        _context.Illnesses.Add(new Illness { IllnessName = "Malaria" });
        await _context.SaveChangesAsync();

        var result = await _service.GetAllDiagnoses();

        Assert.That(result, Has.Count.EqualTo(2));
    }

    // ── GetAllAllergies ──────────────────────────────────────────────────────

    [Test]
    public async Task GetAllAllergies_ReturnsAllMedications()
    {
        _context.Medications.Add(new Medication { MedicationName = "Penicillin", Dosage = 500 });
        _context.Medications.Add(new Medication { MedicationName = "Ibuprofen",  Dosage = 400 });
        await _context.SaveChangesAsync();

        var result = await _service.GetAllAllergies();

        Assert.That(result, Has.Count.EqualTo(2));
    }
}
