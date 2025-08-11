using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Factories
{
    public interface ITaskFactory
    {
        Tareas CreateHighPriority(string description, string extraData = null, DateTime? dueDate = null);
        Tareas CreateNormalPriority(string description, string extraData = null, DateTime? dueDate = null);
        Tareas CreateLowPriority(string description, string extraData = null, DateTime? dueDate = null);
        Tareas CreateFromTemplate(string templateName, string description = null, string extraData = null, DateTime? dueDate = null);
    }
}
