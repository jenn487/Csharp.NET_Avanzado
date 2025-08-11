using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace TaskManagement.Infrastructure.Context
{
    public class TaskManagementContextFactory : IDesignTimeDbContextFactory<TaskManagementContext>
    {
        public TaskManagementContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaskManagementContext>();
            optionsBuilder.UseSqlServer("Data Source=TaskManagement.db");

            return new TaskManagementContext(optionsBuilder.Options);
        }
    }
}
