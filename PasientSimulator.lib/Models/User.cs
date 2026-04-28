namespace PasientSimulator.lib.Models;

public class User {
    public int Id { get; set; }
    public int Role { get; set; }
    public string Name { get; set; }

    public List<Case> Cases { get; set; }
}