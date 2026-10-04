using Microsoft.EntityFrameworkCore;
using Panda.Domain.Entities;

namespace Panda.Infrastructure;

public sealed class PandaDbContext : DbContext
{
    public PandaDbContext(DbContextOptions<PandaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(p => p.NhsNumber);
            entity.Property(p => p.NhsNumber)
                  .IsRequired()
                  .HasMaxLength(10);

            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(p => p.Postcode)
                  .IsRequired()
                  .HasMaxLength(10);
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Status)
                  .HasConversion<string>()
                  .HasMaxLength(10)
                  .IsRequired();

            entity.Property(a => a.PatientNhsNumber)
                  .IsRequired()
                  .HasMaxLength(10);

            entity.Property(a => a.Clinician)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(a => a.Department)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(a => a.Postcode)
                  .IsRequired()
                  .HasMaxLength(10);

            entity.Property(a => a.Duration)
                  .IsRequired()
                  .HasMaxLength(10);

            entity.Property(a => a.Time)
                  .IsRequired();

            entity.HasOne<Patient>()
                  .WithMany()
                  .HasForeignKey(a => a.PatientNhsNumber)
                  .OnDelete(DeleteBehavior.Cascade);
        });

    }
}