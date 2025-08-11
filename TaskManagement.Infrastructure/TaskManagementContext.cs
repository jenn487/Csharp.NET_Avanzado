using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Models;

namespace TaskManagement.Infrastructure
{
    public class TaskManagementContext : DbContext
    {
        public TaskManagementContext(DbContextOptions options)
                : base(options)
        {
        }

        public DbSet<Tareas> Tarea { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // e enum a string para la base de datos
            modelBuilder.Entity<Tareas>()
                .Property(t => t.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Tareas>()
                .Property(t => t.Priority)
                .HasConversion<string>()
                .HasDefaultValue(Tareas.PriorityLevel.Normal);

            base.OnModelCreating(modelBuilder);
        }

    }
}