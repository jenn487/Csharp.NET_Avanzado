using Microsoft.AspNetCore.SignalR;
using TaskManagement.Application.Services.Notifications;
using TaskManagement.Domain.Models;
using TaskManagement.API.Hubs;

namespace TaskManagement.API.Services
{
    public class SignalRNotificationService : INotificationService
    {
        private readonly IHubContext<TaskHub> _hubContext;

        public SignalRNotificationService(IHubContext<TaskHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyNewTaskAsync(Tareas tarea)
        {
            var dto = new
            {
               // tarea.Id,
                tarea.Description,
               // DueDate = tarea.DueDate.ToString("dd-MM-yyyy hh:mm tt"),
                tarea.Status,
                tarea.Priority,
                tarea.ExtraData
            };

            // los clientes conectados
            await _hubContext.Clients.All.SendAsync("TaskCreated", dto);
        }
    }
}
