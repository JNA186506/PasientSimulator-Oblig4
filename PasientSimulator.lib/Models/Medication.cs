namespace PasientSimulator.lib.Models;

public class Medication {
    public int MedicationId { get; set; }
    public string MedicationName { get; set; }
    
    /**
     * Dosage is given in mg.
     */
    public int Dosage { get; set; }
    
    public enum AdministrationRoutes { Iv = 0, Oral = 1 }
    
}