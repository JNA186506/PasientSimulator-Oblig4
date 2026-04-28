namespace PasientSimulator.lib.Models;

public class Case {
    public int CaseId { get; set; }

    public int PatientId { get; set; }
    public Patient CasePatient { get; set; }
    
    public int StudentId { get; set; }
    public User Student { get; set; }

    public List<Goal> Goals { get; set; }
    
}