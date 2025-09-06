using Microsoft.AspNetCore.SignalR;
using TaskManagement.Domain.Models;

namespace TaskManagement.API.Hubs
{
    public class TaskHub : Hub
    {
        public async Task SendNewTaskNotification(Tareas tarea)
        {
            await Clients.All.SendAsync("ReceiveNewTaskNotification", tarea);
        }
    }
}