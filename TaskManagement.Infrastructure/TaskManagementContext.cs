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

    }
}

