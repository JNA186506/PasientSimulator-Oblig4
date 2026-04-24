namespace PasientSimulator_Oblig4_lib.Models;

public class Patient {
    
    public int PatientId { get; set; }
    
    /**
     * Status refers to what state the patient is in.
     * Number value from 1 to 3. Where 1 is healthy, 2 is sick, and 3 is dead.
     */
    public int Status { get; set; }
    public string PatientName { get; set; }
    public int Weight { get; set; }
    public int Age { get; set; }
    public string Sex { get; set; }
    
    public int Heartrate { get; set; }
    public int Bloodpressure { get; set; }
    public int RespiratoryRate { get; set; }
    public double OxygenSaturation { get; set; }
    public double Temperature { get; set; }
    
    
    public List<Illness> MedicalHistory { get; set; }
    public List<Illness> Diagnoses { get; set; }
    
    public List<Medication> Medications { get; set; }
    public List<Medication> Allergies { get; set; }
}