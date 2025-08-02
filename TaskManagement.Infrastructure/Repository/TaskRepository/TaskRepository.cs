using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Models;
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
         => await _context.Tarea.FirstOrDefaultAsync(x=>x.Id == id);

        public async Task<(bool IsSuccess, string Message)> AddAsync(Tareas entry)
        {
            try
            {
                var exists = _context.Tarea.Any(x => x.Id == entry.Id);
                if (exists)
                {
                    return (false, "Ya existe una tarea con este ID.");
                }
                await _context.Tarea.AddAsync(entry);
                await _context.SaveChangesAsync();
                return (true, "¡Guardada Correctamente!");
            }
            catch (Exception ex)
            {
                return (false, $"Error al guardar: {ex.Message}");
            }
        }
        public async Task<(bool IsSuccess, string Message)> UpdateAsync(Tareas entry)
        {
            try
            {
                _context.Tarea.Update(entry);
                await _context.SaveChangesAsync();
                return (true, "¡Actualizada Correctamente!");
            }
            catch (Exception ex)
            {
                return (false, $"Error al editar: {ex.Message}");
            }
        }

        public async Task<(bool IsSuccess, string Message)> DeleteAsync(int id)
        {
            try
            {

                var tarea = await _context.Tarea.FindAsync(id);
                if (tarea != null)
                {
                    _context.Tarea.Remove(tarea);
                    await _context.SaveChangesAsync();
                    return (true, "¡Eliminada Correctamente!");
                }
                else
                {
                    return (false, "Tarea no encontrada.");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar: {ex.Message}");

            }
        }

    }
}


