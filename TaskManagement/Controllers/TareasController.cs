using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Tareas>>> GetTaskByIdAsync(int id)
            => await _service.GetTaskByIdAsync(id);

        [HttpPost]
        public async Task<ActionResult<Response<string>>> AddTaskAsync(Tareas tarea)
            => await _service.AddTaskAsync(tarea);

        [HttpPut]
        public async Task<ActionResult<Response<string>>> UpdateTaskAsync(Tareas tarea)
            => await _service.UpdateTaskAsync(tarea);

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> DeleteTaskAsync(int id)
            => await _service.DeleteTaskAsync(id);

    }

}
