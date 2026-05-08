using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace PasientSimulatorTests;

/// <summary>
/// Tests for AdministerTreatment business logic.
///
/// NOTE: The service mutates the patient object in-memory and then calls
/// SaveChangesAsync. These tests validate the in-memory state changes and
/// return values. Because the patient is not EF-tracked in the test context,
/// SaveChangesAsync is a no-op here — which intentionally isolates the
/// business logic from persistence concerns.
/// </summary>
[TestFixture]
public class AdministerTreatmentTests
{
    private AdministerTreatment _service;
    private Medication _correctMed;
    private Medication _wrongMed;
    private Medication _noEffectMed;
    private Illness _influenza;
    private Patient _patient;

    [SetUp]
    public void Setup()
    {
        _service = new AdministerTreatment(TestContextFactory.Create());

        _correctMed  = new Medication { MedicationId = 1, MedicationName = "Tamiflu" };
        _wrongMed    = new Medication { MedicationId = 2, MedicationName = "Penicillin" };
        _noEffectMed = new Medication { MedicationId = 3, MedicationName = "Aspirin" };
        _influenza   = new Illness   { IllnessId = 1, IllnessName = "Influenza",
                                       AntidoteId = 1, Antidote = _correctMed };

        _patient = new Patient
        {
            PatientId        = 1,
            PatientName      = "Test Patient",
            Age              = 40,
            Weight           = 80,
            Sex              = Patient.SexEnum.Male,
            Status           = Patient.StatusEnum.Sick,
            Heartrate        = 88,
            BloodPressure    = new BloodPressure { Systolic = 135, Diastolic = 88 },
            RespiratoryRate  = 18,
            OxygenSaturation = 95.0,
            Temperature      = 38.5,
            Diagnoses        = new List<Illness>    { _influenza },
            MedicalHistory   = new List<Illness>    { },
            Medications      = new List<Medication> { },
            Allergies        = new List<Medication> { _wrongMed }
        };
    }

    // ── AdministerMedicine ──────────────────────────────────────────────────

    [Test]
    public async Task AdministerMedicine_PatientIsAllergic_ReturnsFalse()
    {
        var result = await _service.AdministerMedicine(_wrongMed, _patient);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task AdministerMedicine_PatientIsAllergic_StatusBecomesSeverelySick()
    {
        await _service.AdministerMedicine(_wrongMed, _patient);

        Assert.That(_patient.Status, Is.EqualTo(Patient.StatusEnum.SeverelySick));
    }

    [Test]
    public async Task AdministerMedicine_MedicationHasNoEffect_ReturnsFalse()
    {
        var result = await _service.AdministerMedicine(_noEffectMed, _patient);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task AdministerMedicine_MedicationHasNoEffect_StatusUnchanged()
    {
        await _service.AdministerMedicine(_noEffectMed, _patient);

        Assert.That(_patient.Status, Is.EqualTo(Patient.StatusEnum.Sick));
    }

    [Test]
    public async Task AdministerMedicine_CorrectAntidote_ReturnsTrue()
    {
        var result = await _service.AdministerMedicine(_correctMed, _patient);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task AdministerMedicine_CorrectAntidote_StatusBecomesHealthy()
    {
        await _service.AdministerMedicine(_correctMed, _patient);

        Assert.That(_patient.Status, Is.EqualTo(Patient.StatusEnum.Healthy));
    }

    // ── AdministerOxygen ────────────────────────────────────────────────────

    [Test]
    public async Task AdministerOxygen_BelowSaturation_ReturnsTrue()
    {
        _patient.OxygenSaturation = 88.0;

        var result = await _service.AdministerOxygen(_patient);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task AdministerOxygen_BelowSaturation_IncreasesOxygenByTen()
    {
        _patient.OxygenSaturation = 88.0;

        await _service.AdministerOxygen(_patient);

        Assert.That(_patient.OxygenSaturation, Is.EqualTo(98.0));
    }

    [Test]
    public async Task AdministerOxygen_AlreadyFullySaturated_ReturnsFalse()
    {
        _patient.OxygenSaturation = 100.0;

        var result = await _service.AdministerOxygen(_patient);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task AdministerOxygen_AlreadyFullySaturated_OxygenUnchanged()
    {
        _patient.OxygenSaturation = 100.0;

        await _service.AdministerOxygen(_patient);

        Assert.That(_patient.OxygenSaturation, Is.EqualTo(100.0));
    }

    [Test]
    public async Task AdministerOxygen_AtExactThreshold_IsAllowed()
    {
        // Guard is (> 99), so 99.0 should still be administerable
        _patient.OxygenSaturation = 99.0;

        var result = await _service.AdministerOxygen(_patient);

        Assert.That(result, Is.True);
    }

    // ── DecreaseTemperature ─────────────────────────────────────────────────

    [Test]
    public async Task DecreaseTemperature_ReturnsNewTemperature()
    {
        _patient.Temperature = 39.0;

        var returned = await _service.DecreaseTemperature(_patient);

        Assert.That(returned, Is.EqualTo(29.0));
    }

    [Test]
    public async Task DecreaseTemperature_UpdatesPatientObject()
    {
        _patient.Temperature = 39.0;

        await _service.DecreaseTemperature(_patient);

        Assert.That(_patient.Temperature, Is.EqualTo(29.0));
    }

    [Test]
    public async Task DecreaseTemperature_ReturnValueMatchesPatientTemperature()
    {
        _patient.Temperature = 38.5;

        var returned = await _service.DecreaseTemperature(_patient);

        Assert.That(returned, Is.EqualTo(_patient.Temperature));
    }
}
