using FitTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Infrastructure.Persistence;

public sealed class FitTrackDbContext(DbContextOptions<FitTrackDbContext> options) : DbContext(options)
{
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<WorkoutPlan> WorkoutPlans => Set<WorkoutPlan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exercise>(e => {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(120).IsRequired();
            e.Property(x => x.MuscleGroup).HasMaxLength(80).IsRequired();
            e.HasIndex(x => x.MuscleGroup);
            e.HasIndex(x => x.Name);
        });
        modelBuilder.Entity<WorkoutPlan>(e => {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).HasMaxLength(120).IsRequired();
            e.Property(x => x.Description).HasMaxLength(1000);
        });
    }
}
