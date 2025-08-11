using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Factories;
using TaskManagement.Application.Services.TaskServices;
using TaskManagement.Domain.DTO;
using TaskManagement.Domain.Models;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly TaskService _service;

        public TareasController(TaskService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<Response<Tareas>>> GetAllTasksAsync()
                => await _service.GetAllTasksAsync();

        [HttpGet("pending")]
        public async Task<ActionResult<Response<Tareas>>> GetPendingTasksAsync()
            => await _service.GetPendingTasksAsync();

        [HttpGet("overdue")]
        public async Task<ActionResult<Response<Tareas>>> GetOverdueTasksAsync()
            => await _service.GetOverdueTasksAsync();

        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<Response<Tareas>>> SearchTasksAsync(string searchTerm)
            => await _service.SearchTasksByDescriptionAsync(searchTerm);

        [HttpGet("summary")]
        public async Task<ActionResult<Response<string>>> GetTasksSummaryAsync()
            => await _service.GetTasksSummaryAsync();

        [HttpGet("filter-completed")]
        public async Task<ActionResult<Response<Tareas>>> GetCompletedTasksAsync()
            => await _service.GetAllTasksAsync(t => t.Status == Tareas.TaskStatus.Completado);

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Tareas>>> GetTaskByIdAsync(int id)
            => await _service.GetTaskByIdAsync(id);

        [HttpPost]
        public async Task<ActionResult<Response<string>>> AddTaskAsync(Tareas tarea)
            => await _service.AddTaskAsync(tarea);

        [HttpPost("create-high-priority")]
        public async Task<ActionResult<Response<string>>> CreateHighPriority([FromBody] string description)
        {
            return await _service.AddHighPriorityTaskAsync(description);
        }

        [HttpPut]
        public async Task<ActionResult<Response<string>>> UpdateTaskAsync(Tareas tarea)
            => await _service.UpdateTaskAsync(tarea);

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> DeleteTaskAsync(int id)
            => await _service.DeleteTaskAsync(id);
    }
}

