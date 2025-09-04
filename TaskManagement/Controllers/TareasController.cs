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
        //LOS GETS
        [HttpGet]
        public async Task<ActionResult<Response<Tareas>>> GetAllTasksAsync()
            => await _service.GetAllTasksAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Tareas>>> GetTaskByIdAsync(int id)
            => await _service.GetTaskByIdAsync(id);

        //LOS POSTS
        [HttpPost]
        public async Task<ActionResult<Response<string>>> AddTaskAsync(Tareas tarea)
            => await _service.AddTaskAsync(tarea);

        [HttpPost("high-priority")]
        public async Task<ActionResult<Response<string>>> AddHighPriorityTaskAsync([FromBody] string description)
        {
            var tarea = TaskManagement.Application.Services.TaskServices.TaskFactory.CreateHighPriorityTask(description);
            var result = await _service.AddTaskAsync(tarea);
            return Ok (result);
        }

        [HttpPost("low-priority")]
        public async Task<ActionResult<Response<string>>> AddLowPriorityTaskAsync([FromBody] string description)
        {
            var tarea = TaskManagement.Application.Services.TaskServices.TaskFactory.CreateLowPriorityTask(description);
            var result = await _service.AddTaskAsync(tarea);
            return Ok(result);
        }

        //LOS PUTS
        [HttpPut]
        public async Task<ActionResult<Response<string>>> UpdateTaskAsync(Tareas tarea)
            => await _service.UpdateTaskAsync(tarea);

        //LOS DELETES
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> DeleteTaskAsync(int id)
            => await _service.DeleteTaskAsync(id);

    }

}
