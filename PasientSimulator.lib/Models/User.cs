namespace PasientSimulator.lib.Models;

public class User {
    public int UserId { get; set; }
    public int Role { get; set; }
    public string Name { get; set; }

    public List<Case> Cases { get; set; }
}