namespace PasientSimulator_Oblig4_lib.Models;

public class Patient {
    
    public int PatientId { get; set; }
    public string PatientName { get; set; }
    
    /**
     * Status refers to what state the patient is in.
     * Number value from 1 to 3. Where 1 is healthy, 2 is sick, and 3 is dead.
     */
    public int Status { get; set; }
    public int Heartrate { get; set; }
    
    public List<Medication> Medications { get; set; }
}