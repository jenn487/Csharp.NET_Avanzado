using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Factories
{
    public class TaskFactory : ITaskFactory
    {
        public Tareas CreateHighPriority(string description, string extraData = null, DateTime? dueDate = null)
            => new Tareas
            {
                Description = description ?? "Tarea Alta Prioridad",
                DueDate = dueDate ?? DateTime.UtcNow.AddDays(2),
                Status = Tareas.TaskStatus.Pendiente,
                Priority = Tareas.PriorityLevel.High,
                ExtraData = extraData ?? "Plantilla: Alta prioridad"
            };

        public Tareas CreateNormalPriority(string description, string extraData = null, DateTime? dueDate = null)
            => new Tareas
            {
                Description = description ?? "Tarea Normal",
                DueDate = dueDate ?? DateTime.UtcNow.AddDays(7),
                Status = Tareas.TaskStatus.Pendiente,
                Priority = Tareas.PriorityLevel.Normal,
                ExtraData = extraData ?? "Plantilla: Normal"
            };

        public Tareas CreateLowPriority(string description, string extraData = null, DateTime? dueDate = null)
            => new Tareas
            {
                Description = description ?? "Tarea Baja Prioridad",
                DueDate = dueDate ?? DateTime.UtcNow.AddDays(30),
                Status = Tareas.TaskStatus.Pendiente,
                Priority = Tareas.PriorityLevel.Low,
                ExtraData = extraData ?? "Plantilla: Baja prioridad"
            };

        public Tareas CreateFromTemplate(string templateName, string description = null, string extraData = null, DateTime? dueDate = null)
        {
            if (string.IsNullOrWhiteSpace(templateName))
                return CreateNormalPriority(description, extraData, dueDate);

            switch (templateName.Trim().ToLowerInvariant())
            {
                case "high":
                case "alta":
                    return CreateHighPriority(description, extraData, dueDate);
                case "low":
                case "baja":
                    return CreateLowPriority(description, extraData, dueDate);
                default:
                    return CreateNormalPriority(description, extraData, dueDate);
            }
        }
    }
}
