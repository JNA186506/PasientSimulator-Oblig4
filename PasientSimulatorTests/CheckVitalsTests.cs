using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace PasientSimulatorTests;

/// <summary>
/// Tests for CheckVitals.
/// CheckVitals has no database dependency — it simply reads properties off
/// a Patient, so these are pure unit tests with no in-memory context needed.
/// </summary>
[TestFixture]
public class CheckVitalsTests
{
    private CheckVitals _service;
    private Patient _patient;

    [SetUp]
    public void Setup()
    {
        _service = new CheckVitals();

        _patient = new Patient
        {
            PatientId        = 1,
            PatientName      = "Vital Signs Patient",
            Age              = 35,
            Weight           = 70,
            Sex              = Patient.SexEnum.Female,
            Status           = Patient.StatusEnum.Sick,
            Heartrate        = 72,
            BloodPressure    = new BloodPressure { Systolic = 120, Diastolic = 80 },
            RespiratoryRate  = 16,
            OxygenSaturation = 98.5,
            Temperature      = 36.8,
            Diagnoses        = new List<Illness>    { },
            MedicalHistory   = new List<Illness>    { },
            Medications      = new List<Medication> { },
            Allergies        = new List<Medication> { }
        };
    }

    [Test]
    public void CheckHR_ReturnsPatientHeartrate()
    {
        var result = _service.CheckHR(_patient);

        Assert.That(result, Is.EqualTo(72));
    }

    [Test]
    public void CheckHR_ReflectsUpdatedValue()
    {
        _patient.Heartrate = 110;

        var result = _service.CheckHR(_patient);

        Assert.That(result, Is.EqualTo(110));
    }

    [Test]
    public void CheckRespiratoryRate_ReturnsCorrectRate()
    {
        var result = _service.CheckRespiratoryRate(_patient);

        Assert.That(result, Is.EqualTo(16));
    }

    [Test]
    public void CheckRespiratoryRate_ReflectsUpdatedValue()
    {
        _patient.RespiratoryRate = 24;

        var result = _service.CheckRespiratoryRate(_patient);

        Assert.That(result, Is.EqualTo(24));
    }

    [Test]
    public void CheckOxygenSaturation_ReturnsCorrectValue()
    {
        var result = _service.CheckOxygenSaturation(_patient);

        Assert.That(result, Is.EqualTo(98.5));
    }

    [Test]
    public void CheckTemperature_ReturnsCorrectValue()
    {
        var result = _service.CheckTemperature(_patient);

        Assert.That(result, Is.EqualTo(36.8));
    }

    [Test]
    public void CheckBloodPressure_ReturnsCorrectSystolic()
    {
        var (systolic, _) = _service.CheckBloodPressure(_patient);

        Assert.That(systolic, Is.EqualTo(120));
    }

    [Test]
    public void CheckBloodPressure_ReturnsCorrectDiastolic()
    {
        var (_, diastolic) = _service.CheckBloodPressure(_patient);

        Assert.That(diastolic, Is.EqualTo(80));
    }

    [Test]
    public void CheckBloodPressure_ReflectsUpdatedValues()
    {
        _patient.BloodPressure = new BloodPressure { Systolic = 160, Diastolic = 100 };

        var (systolic, diastolic) = _service.CheckBloodPressure(_patient);

        Assert.That(systolic,  Is.EqualTo(160));
        Assert.That(diastolic, Is.EqualTo(100));
    }
}
