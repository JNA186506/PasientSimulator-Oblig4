using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasientSimulator.lib.Models;

public class Goal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GoalId { get; set; }

    public int? CaseId { get; set; }
    public string GoalName { get; set; }

    public int TimeLimit { get; set; }

    public string Description { get; set; }
}