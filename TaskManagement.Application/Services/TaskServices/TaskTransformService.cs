using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Delegates;
using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Services.TaskServices
{
    public class TaskTransformService
    {
        public Func<Tareas, int> CalculateDaysRemaining { get; private set; }
        public Func<Tareas, string> GetTaskSummary { get; private set; }
        public Func<Tareas, bool> IsUrgent { get; private set; }

        public Action<string> LogTransformation { get; set; }

        public TaskTransformService()
        {
            CalculateDaysRemaining = tarea => (tarea.DueDate - DateTime.Now).Days;

            GetTaskSummary = tarea => $"{tarea.Description} - Vence: {tarea.DueDate:dd/MM/yyyy} - Estado: {tarea.Status}";

            IsUrgent = tarea => (tarea.DueDate - DateTime.Now).Days <= 3 && tarea.Status == Tareas.TaskStatus.Pendiente;

            LogTransformation = operation => Console.WriteLine($"Transformación aplicada: {operation}");
        }

        public IEnumerable<TResult> TransformTasks<TResult>(IEnumerable<Tareas> tasks, TaskTransformDelegate<TResult> transformer)
        {
            LogTransformation?.Invoke($"Transformando {tasks.Count()} tareas");
            return tasks.Select(task => transformer(task));
        }
    }
}
