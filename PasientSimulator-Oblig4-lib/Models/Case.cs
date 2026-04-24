namespace PasientSimulator_Oblig4_lib.Models;

public class Case {
    public int CaseId { get; set; }

    public Patient CasePatient { get; set; }

    public int TimeLimit { get; set; }
    
    public User Student { get; set; }
    
    public List<Goal> Goals { get; set; }
    
}