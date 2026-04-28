namespace PasientSimulator.lib.Models;

public class Patient {
    
    public int PatientId { get; set; }

    /**
     * Status refers to what state the patient is in.
     * Number value from 1 to 3. Where 1 is healthy, 2 is sick, and 3 is dead.
     */
    public enum StatusEnum {
        Healthy = 1,
        Sick = 2,
        SeverelySick = 3,
        Dead = 4
    }

    public enum SexEnum {
        Male = 1,
        Female = 2,
        Other = 3
    }

    public string PatientName { get; set; }
    public int Weight { get; set; }
    public int Age { get; set; }

    public SexEnum Sex { get; set; }
    public StatusEnum Status { get; set; }
    
    
    public int Heartrate { get; set; }
    public BloodPressure Bloodpressure { get; set; }
    public int RespiratoryRate { get; set; }
    public double OxygenSaturation { get; set; }
    public double Temperature { get; set; }
    
    
    public List<Illness> MedicalHistory { get; set; }
    public List<Illness> Diagnoses { get; set; }
    
    public List<Medication> Medications { get; set; }
    public List<Medication> Allergies { get; set; }
}