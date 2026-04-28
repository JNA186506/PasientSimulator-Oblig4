namespace PasientSimulator.lib.Models;

public class Illness {
    public int Id { get; set; }
    public string Name { get; set; }
    
    /**
     * This variable should hold a positive or negative value, that effects the Oxygen level of the patient.
     */
    public double OxygenEffect { get; set; }
    
    /**
     * @SystolicEffect how much the illness affects the Systolic pressure.
     * @DiastolicEffect how much the illness affects the Diastolic pressure.
     */
    public BloodPressure Bloodpressure { get; set; }
    
    /**
     * The solution to an illness is being administered the right medication.
     * This will nullify the effect, and curing the given illness.
     */
    public Medication? Antidote { get; set; }
}