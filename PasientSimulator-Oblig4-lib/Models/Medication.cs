namespace PasientSimulator_Oblig4_lib.Models;

public class Medication {
    public int MedicationId { get; set; }
    public string MedicationName { get; set; }
    
    /**
     * @Dosage are given in mg.
     */
    public int Dosage { get; set; }
    public string AdministrationRoots { get; set; }
    
    public List<string> Effects { get; set; }
}