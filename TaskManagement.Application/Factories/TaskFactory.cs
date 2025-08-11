using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Factories
{
    public class TaskFactory 
    {
        public static Tareas CreateHighPriorityTask(string description)
        {
            return new Tareas
            {
                Description = description,
                DueDate = DateTime.Now.AddDays(1),
                Status = Tareas.TaskStatus.Pendiente,
                Priority = Tareas.PriorityLevel.High,
                ExtraData = "High Priority"
            };
        }

        public static Tareas CreateNormalPriorityTask(string description)
        {
            return new Tareas
            {
                Description = description,
                DueDate = DateTime.Now.AddDays(7),
                Status = Tareas.TaskStatus.Pendiente,
                Priority = Tareas.PriorityLevel.Normal,
                ExtraData = "Normal Priority"
            };
        }

        public static Tareas CreateLowPriorityTask(string description)
        {
            return new Tareas
            {
                Description = description,
                DueDate = DateTime.Now.AddDays(30),
                Status = Tareas.TaskStatus.Pendiente,
                Priority = Tareas.PriorityLevel.Low,
                ExtraData = "Low Priority"
            };
        }

    }
}
