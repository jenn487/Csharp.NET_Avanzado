using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Models;

namespace TaskManagement.Infrastructure.Context
{
    public class TaskManagementContext : DbContext
    {
        public TaskManagementContext(DbContextOptions<TaskManagementContext> options)
                : base(options)
        {
        }

        public DbSet<Tareas<string>> Tarea => Set<Tareas<string>>();

    }
}
