using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Services.TaskServices
{
    public static class TaskFactory
    {
        // tarea de alta prioridad
        public static Tareas CreateHighPriorityTask(string description)
        {
            return new Tareas
            {
                Description = description,
                DueDate = DateTime.Now.AddDays(1),
                Status = "Pendiente",
                Priority = "Alta",
                // ExtraData no lo pongo, ya que TaskService se encarga de inicializarlo
            };
        }

        // tarea de baja prioridad
        public static Tareas CreateLowPriorityTask(string description)
        {
            return new Tareas
            {
                Description = description,
                DueDate = DateTime.Now.AddDays(7),
                Status = "Pendiente",
                Priority = "Baja",
            };
        }
    }
}