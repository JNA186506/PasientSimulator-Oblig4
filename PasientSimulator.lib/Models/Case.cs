using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasientSimulator.lib.Models;

public class Case {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CaseId { get; set; }

    public int PatientId { get; set; }
    public Patient CasePatient { get; set; }
    
    public int UserId { get; set; }
    public User Student { get; set; }

    public List<Goal> Goals { get; set; }
    
}