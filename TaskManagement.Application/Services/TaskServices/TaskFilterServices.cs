using System.Threading.Tasks;
using TaskManagement.Domain.Delegates;
using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Services.TaskServices
{
    public class TaskFilterService
    {
        public Func<IEnumerable<Tareas>, TaskFilterDelegate, IEnumerable<Tareas>> ApplyFilter { get; private set; }

        public Action<string, int> LogFilterResults { get; set; }

        public TaskFilterService()
        {
            ApplyFilter = (tasks, filter) => tasks.Where(t => filter(t));

            LogFilterResults = (filterName, count) =>
                Console.WriteLine($"Filtro '{filterName}' aplicado. Resultados: {count}");
        }

        // Filtros predefinidos con funciones anonimas
        public TaskFilterDelegate PendingTasksFilter => t => t.Status == Tareas.TaskStatus.Pendiente;

        public TaskFilterDelegate CompletedTasksFilter => t => t.Status == Tareas.TaskStatus.Completado;

        public TaskFilterDelegate OverdueTasksFilter => t => t.DueDate < DateTime.Now && t.Status == Tareas.TaskStatus.Pendiente;

        public Func<DateTime, TaskFilterDelegate> CreateDateRangeFilter =>
            date => (tarea => tarea.DueDate.Date == date.Date);

        public Func<string, TaskFilterDelegate> CreateDescriptionContainsFilter =>
            searchTerm => (tarea => tarea.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

        public IEnumerable<Tareas> ApplyMultipleFilters(IEnumerable<Tareas> tasks, params TaskFilterDelegate[] filters)
        {
            var result = tasks;
            foreach (var filter in filters)
            {
                result = ApplyFilter(result, filter);
            }
            return result;
        }
    }
}
