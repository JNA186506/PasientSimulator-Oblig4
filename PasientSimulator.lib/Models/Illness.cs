namespace PasientSimulator.lib.Models;

public class Illness {
    public int Id { get; set; }
    public string Name { get; set; }
    
    /**
     * The solution to an illness is being administered the right medication.
     * This will nullify the effect, and curing the given illness.
     */
    public int? AntidoteId { get; set; }
    public Medication? Antidote { get; set; }
}