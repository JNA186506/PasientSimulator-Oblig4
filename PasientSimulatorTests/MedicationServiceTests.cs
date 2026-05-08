using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace PasientSimulatorTests;

[TestFixture]
public class MedicationServiceTests
{
    private Context _context;
    private MedicationService _service;

    [SetUp]
    public void Setup()
    {
        _context = TestContextFactory.Create();
        _service = new MedicationService(_context);
    }

    [TearDown]
    public void TearDown() => _context.Dispose();

    // ── IsAntidote ───────────────────────────────────────────────────────────

    [Test]
    public void IsAntidote_WhenMedicationMatchesDiagnosisAntidote_ReturnsTrue()
    {
        var antidote = new Medication { MedicationId = 1, MedicationName = "Tamiflu" };
        var illness  = new Illness    { IllnessId = 1, IllnessName = "Influenza",
                                        AntidoteId = 1, Antidote = antidote };
        var patient  = new Patient
        {
            Diagnoses  = new List<Illness>    { illness },
            Allergies  = new List<Medication> { }
        };

        var result = _service.IsAntidote(antidote, patient);

        Assert.That(result, Is.True);
    }

    [Test]
    public void IsAntidote_WhenMedicationDoesNotMatchAnyDiagnosis_ReturnsFalse()
    {
        var antidote     = new Medication { MedicationId = 1, MedicationName = "Tamiflu" };
        var otherMed     = new Medication { MedicationId = 2, MedicationName = "Aspirin" };
        var illness      = new Illness    { IllnessId = 1, IllnessName = "Influenza",
                                            AntidoteId = 1, Antidote = antidote };
        var patient      = new Patient
        {
            Diagnoses  = new List<Illness>    { illness },
            Allergies  = new List<Medication> { }
        };

        var result = _service.IsAntidote(otherMed, patient);

        Assert.That(result, Is.False);
    }

    [Test]
    public void IsAntidote_WhenPatientHasNoDiagnoses_ReturnsFalse()
    {
        var med     = new Medication { MedicationId = 1, MedicationName = "Tamiflu" };
        var patient = new Patient
        {
            Diagnoses = new List<Illness>    { },
            Allergies = new List<Medication> { }
        };

        var result = _service.IsAntidote(med, patient);

        Assert.That(result, Is.False);
    }

    [Test]
    public void IsAntidote_WhenPatientHasMultipleDiagnoses_MatchesCorrectOne()
    {
        var flu_antidote = new Medication { MedicationId = 1, MedicationName = "Tamiflu" };
        var mal_antidote = new Medication { MedicationId = 2, MedicationName = "Chloroquine" };
        var patient = new Patient
        {
            Diagnoses = new List<Illness>
            {
                new Illness { IllnessId = 1, IllnessName = "Influenza", AntidoteId = 1 },
                new Illness { IllnessId = 2, IllnessName = "Malaria",   AntidoteId = 2 }
            },
            Allergies = new List<Medication> { }
        };

        Assert.That(_service.IsAntidote(flu_antidote, patient), Is.True);
        Assert.That(_service.IsAntidote(mal_antidote, patient), Is.True);
    }

    // ── GetAllMedications ────────────────────────────────────────────────────

    [Test]
    public async Task GetAllMedications_WhenEmpty_ReturnsEmptyList()
    {
        var result = await _service.GetAllMedications();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAllMedications_ReturnsAllMedications()
    {
        _context.Medications.AddRange(
            new Medication { MedicationName = "Tamiflu",   Dosage = 75  },
            new Medication { MedicationName = "Penicillin", Dosage = 500 },
            new Medication { MedicationName = "Aspirin",   Dosage = 500 }
        );
        await _context.SaveChangesAsync();

        var result = await _service.GetAllMedications();

        Assert.That(result, Has.Count.EqualTo(3));
    }

    // ── FindMedication ───────────────────────────────────────────────────────

    [Test]
    public async Task FindMedication_ValidId_ReturnsMedication()
    {
        var med = new Medication { MedicationId = 5, MedicationName = "Ibuprofen", Dosage = 400 };
        _context.Medications.Add(med);
        await _context.SaveChangesAsync();

        var result = await _service.FindMedication(5);

        Assert.That(result.MedicationName, Is.EqualTo("Ibuprofen"));
    }

    [Test]
    public async Task FindMedication_InvalidId_ReturnsNull()
    {
        // FindMedication uses FindAsync which returns null (not an exception) for missing keys
        var result = await _service.FindMedication(99999);

        Assert.That(result, Is.Null);
    }

    // ── AddMedication ────────────────────────────────────────────────────────

    [Test]
    public void AddMedication_PersistsMedicationAndReturnsIt()
    {
        var result = _service.AddMedication("Morphine");

        Assert.That(result.MedicationName, Is.EqualTo("Morphine"));
    }

    [Test]
    public async Task AddMedication_MedicationAppearsInGetAll()
    {
        _service.AddMedication("Morphine");

        var all = await _service.GetAllMedications();

        Assert.That(all.Any(m => m.MedicationName == "Morphine"), Is.True);
    }
}
