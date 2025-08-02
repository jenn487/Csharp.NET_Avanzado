using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Models;
using TaskManagement.Infrastructure.Context;
using TaskManagement.Infrastructure.Repository.Common;

namespace TaskManagement.Infrastructure.Repository.TaskRepository
{
    public class TaskRepository<T> : ICommonProcess<Tareas>
    {
        private readonly TaskManagementContext _context;

        public TaskRepository(TaskManagementContext taskManagementContext)
        {
            _context = taskManagementContext;
        }
        public async Task<IEnumerable<Tareas>> GetAllAsync()
         => await _context.Tarea.ToListAsync();
        public async Task<Tareas> GetIdAsync(int id)
         => await _context.Tarea.FirstOrDefaultAsync(x => x.Id == id);
        public Task<(bool IsSuccess, string Message)> AddAsync(Tareas entry)
        {
            throw new NotImplementedException();
        }

        public Task<(bool IsSuccess, string Message)> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<(bool IsSuccess, string Message)> UpdateTask(Tareas entry)
        {
            throw new NotImplementedException();
        }
    }
}


