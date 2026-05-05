using Microsoft.EntityFrameworkCore;

namespace PasientSimulator.lib.Models;

public class Context : DbContext
{
    public Context()
    {
    }

    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    public DbSet<Case> Cases { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<Illness> Illnesses { get; set; }
    public DbSet<Medication> Medications { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql(
                "Host=127.0.0.1;Port=5432;Database=postgres;Username=postgres;Password=");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName()!.ToLower());
            foreach (var property in entity.GetProperties())
                property.SetColumnName(property.GetColumnName().ToLower());
            foreach (var key in entity.GetKeys())
                key.SetName(key.GetName()!.ToLower());
            foreach (var fk in entity.GetForeignKeys())
                fk.SetConstraintName(fk.GetConstraintName()!.ToLower());
        }
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

        modelBuilder.Entity<Case>()
            .HasMany(c => c.Events)
            .WithOne()
            .HasForeignKey(e => e.CaseId);
        
        // Configure Patient entity
        modelBuilder.Entity<Patient>()
            .HasKey(p => p.PatientId);

        modelBuilder.Entity<Patient>()
            .Property(p => p.PatientId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Patient>()
            .OwnsOne(p => p.BloodPressure, bp =>
            {
                bp.Property(b => b.Systolic).HasColumnName("bloodpressure_systolic");
                bp.Property(b => b.Diastolic).HasColumnName("bloodpressure_diastolic");
            });

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Diagnoses)
            .WithMany()
            .UsingEntity(j =>
            {
                j.ToTable("diagnoses");
                j.Property<int>("patientid");
                j.Property<int>("illnessid");
                j.HasKey("patientid", "illnessid");
            });

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.MedicalHistory)
            .WithMany()
            .UsingEntity(j =>
            {
                j.ToTable("medicalhistory");
                j.Property<int>("patientid");
                j.Property<int>("illnessid");
                j.HasKey("patientid", "illnessid");
            });

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Medications)
            .WithMany()
            .UsingEntity(j =>
            {
                j.ToTable("patientmedications");
                j.Property<int>("patientid");
                j.Property<int>("medicationid");
                j.HasKey("patientid", "medicationid");
            });

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Allergies)
            .WithMany()
            .UsingEntity(j =>
            {
                j.ToTable("allergies");
                j.Property<int>("patientid");
                j.Property<int>("medicationid");
                j.HasKey("patientid", "medicationid");
            });

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

        modelBuilder.Entity<Event>().ToTable("event");
    }
}