using System.Runtime.InteropServices.ComTypes;
using PasientSimulator_Oblig4_lib.Models;


namespace PasientSimulatorTests;



public class AdministerTreatmentTests {
    private Patient p1;
    [SetUp]
    public void Setup() {
        p1 = new Patient { PatientId = 1, Age = 30, PatientName = "Johannes", 
            Bloodpressure = new BloodPressure { Systolic = 150, Diastolic = 80 },
            
        };
    }

    [Test]
    public void AdministerMedicineTest() {
        Assert.Pass();
    }
}