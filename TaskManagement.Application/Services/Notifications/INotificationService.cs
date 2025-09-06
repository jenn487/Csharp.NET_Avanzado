
using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Services.Notifications
{
    public interface INotificationService
    {
        Task NotifyNewTaskAsync(Tareas tarea);
    }
}
