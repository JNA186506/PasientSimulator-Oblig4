using Microsoft.EntityFrameworkCore;

namespace PasientSimulator.lib.Models;

public class Context : DbContext {

    public Context() { }

    public Context(DbContextOptions<Context> options) : base(options) {}

    public DbSet<Case> Cases { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<Illness> Illnesses { get; set; }
    public DbSet<Medication> Medications { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            optionsBuilder.UseSqlServer("Server=tcp:dat154-rubylite.database.windows.net,1433;Initial Catalog=rubylite;Persist Security Info=False;User ID=rubyadmin;Password=johannes123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Case>()
            .HasKey(c => c.CaseId);

        modelBuilder.Entity<Case>()
            .Property(c => c.CaseId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Case>()
            .HasOne(c => c.CasePatient)
            .WithMany()
            .HasForeignKey(c => c.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Case>()
            .HasOne(c => c.Student)
            .WithMany(u => u.Cases)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Case>()
            .HasMany(c => c.Goals)
            .WithOne()
            .HasForeignKey(g => g.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Patient entity
        modelBuilder.Entity<Patient>()
            .HasKey(p => p.PatientId);

        modelBuilder.Entity<Patient>()
            .Property(p => p.PatientId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Patient>()
            .OwnsOne(p => p.Bloodpressure);

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.MedicalHistory)
            .WithMany();

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Diagnoses)
            .WithMany();

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Medications)
            .WithMany();

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Allergies)
            .WithMany();

        // Configure Illness entity
        modelBuilder.Entity<Illness>()
            .HasKey(i => i.IllnessId);

        modelBuilder.Entity<Illness>()
            .Property(i => i.IllnessId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Illness>()
            .HasOne(i => i.Antidote)
            .WithMany()
            .HasForeignKey(i => i.AntidoteId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure Medication entity
        modelBuilder.Entity<Medication>()
            .HasKey(m => m.MedicationId);

        modelBuilder.Entity<Medication>()
            .Property(m => m.MedicationId)
            .ValueGeneratedOnAdd();

        // Configure Goal entity
        modelBuilder.Entity<Goal>()
            .HasKey(g => g.GoalId);

        modelBuilder.Entity<Goal>()
            .Property(g => g.GoalId)
            .ValueGeneratedOnAdd();
        
        // Configure User entity
        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId);

        modelBuilder.Entity<User>()
            .Property(u => u.UserId)
            .ValueGeneratedOnAdd();
    }
}