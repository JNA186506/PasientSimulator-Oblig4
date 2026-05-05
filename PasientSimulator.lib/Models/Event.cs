using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasientSimulator.lib.Models;

public enum EventEnum
{
    Comment = 0, MedicalIntervention = 1
}
public class Event
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EventId { get; set; }
    
    public int CaseId { get; set; }
    
    public EventEnum EventType { get; set; }

    public string? Description { get; set; }
    public DateTime Timeadded { get; set; } = DateTime.UtcNow;
    public int UserId { get; set; }
}