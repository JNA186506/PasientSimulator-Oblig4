namespace PasientSimulator_Oblig4_lib.Models;

public class Medication {
    public int MedicationId { get; set; }
    public string MedicationName { get; set; }
    
    public List<string> Effects { get; set; }
}