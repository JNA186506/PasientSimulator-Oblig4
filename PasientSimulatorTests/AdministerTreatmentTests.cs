using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;


namespace PasientSimulatorTests;

public class AdministerTreatmentTests {
    private Patient p1;
    private Illness influenza;
    private Medication influenzaMedication;
    private Medication wrongMedication;
    private Medication noEffectMedication;
    private AdministerTreatment _administerTreatment;
    
    [SetUp]
    public void Setup() {
        influenzaMedication = new Medication { MedicationId = 1, MedicationName = "influenzaMedication" };
        influenza = new Illness { IllnessId = 1, Antidote = influenzaMedication };
        wrongMedication = new Medication { MedicationId = 2 };
        noEffectMedication = new Medication { MedicationId = 3 };
        
        p1 = new Patient {
            PatientId = 1,
            PatientName = "John Doe",
            Age = 45,
            Weight = 80,
            Sex = Patient.SexEnum.Male,
            Status = Patient.StatusEnum.Sick,
            Heartrate = 88,
            Bloodpressure = new BloodPressure { Systolic = 140, Diastolic = 90 },
            RespiratoryRate = 18,
            OxygenSaturation = 96.5,
            Temperature = 38.2,
            MedicalHistory = new List<Illness>(),
            Diagnoses = new List<Illness> { influenza },
            Medications = new List<Medication>(),
            Allergies = new List<Medication> { wrongMedication }
        };
    }

    [Test]
    public void AdministerMedicineTest() {

        bool allergicAdministeration = _administerTreatment.AdministerMedicine(wrongMedication, p1);

        Assert.That(allergicAdministeration, Is.False, "The wrong medication was administered, the patient was allergic and is now severely sick"); 
        
        Assert.That(p1.Status, Is.EqualTo(Patient.StatusEnum.SeverelySick));

        bool wrongAdministration = _administerTreatment.AdministerMedicine(noEffectMedication, p1);
        
        Assert.That(wrongAdministration, Is.False, "The wrong medication was administered, it had no effect...");

        bool correctAdministration = _administerTreatment.AdministerMedicine(influenzaMedication, p1);
        
        Assert.That(correctAdministration, Is.True, "The correct medicine was administered, the patient is now healthy!");

        Assert.That(p1.Status, Is.EqualTo(Patient.StatusEnum.Healthy));

    }
}