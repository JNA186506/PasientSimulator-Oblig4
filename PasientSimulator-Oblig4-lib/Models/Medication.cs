using System.Runtime.Intrinsics.X86;

namespace PasientSimulator_Oblig4_lib.Models;

public class Medication {
    public int MedicationId { get; set; }
    public string MedicationName { get; set; }
    
    /**
     * @Dosage are given in mg.
     */
    public int Dosage { get; set; }
    
    public enum AdministrationRoutes { Iv = 0, Oral = 1 }
    
    public List<string> Effects { get; set; }
}