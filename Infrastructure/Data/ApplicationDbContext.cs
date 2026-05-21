using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
     
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Description).HasMaxLength(500);
                entity.Property(p => p.UserId).IsRequired();

                entity.HasMany(p => p.Tasks)
                      .WithOne(t => t.Project)
                      .HasForeignKey(t => t.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade); 
            });

            modelBuilder.Entity<ProjectTask>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title).IsRequired().HasMaxLength(150);
                entity.Property(t => t.Description).HasMaxLength(100);

                entity.Property(t => t.Status)
                      .HasConversion<string>()
                      .HasDefaultValue(Domain.Enums.TaskStatus.ToDo);

                entity.Property(t => t.Priority)
                      .HasConversion<string>()
                      .HasDefaultValue(TaskPriority.Medium);
            });
        }



        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
    }
}
