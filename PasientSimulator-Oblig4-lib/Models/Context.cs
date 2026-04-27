using Microsoft.EntityFrameworkCore;

namespace PasientSimulator_Oblig4_lib.Models;

public class Context : DbContext {
    
    public Context() { }
    
    public Context(DbContextOptions<Context> options) : base(options) {}
    
    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            optionsBuilder.UseSqlServer("Server=tcp:dat154-rubylite.database.windows.net,1433;Initial Catalog=DAT154-RUBYlite;Persist Security Info=False;User ID=rubyadmin;Password=magneogjohannespaaeventyr1#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
    
}