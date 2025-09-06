using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Services.Reactive;
using TaskManagement.Application.Services.TaskServices;
using TaskManagement.Domain.DTO;
using TaskManagement.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using TaskManagement.Application.Services.Notifications;


namespace TaskManagement.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly TaskService _service;
        private readonly IReactiveTaskQueue _taskQueue;
        private readonly INotificationService _notificationService;
        public TareasController(TaskService service, IReactiveTaskQueue taskQueue, INotificationService notificationService)
        {
            _service = service;
            _taskQueue = taskQueue;
            _notificationService = notificationService;
        }

        //MEMORIZACION 

        // porciento tareas completadas
        [HttpGet("completion-rate")]
        public async Task<ActionResult<double>> GetCompletionRateAsync()
        {
            var rate = await _service.CalculateTaskCompletionRateAsync();
            return Ok($"{Math.Round(rate, 2)}% completadas"); 
        }

        //filtro tareas por estado
        [HttpGet("by-status/{status}")]
        public async Task<ActionResult<IEnumerable<Tareas>>> GetTasksByStatusAsync(string status)
        {
            var tasks = await _service.GetTasksByStatusAsync(status);
            return Ok(tasks);
        }


        //LOS GETS
        [HttpGet]
        public async Task<ActionResult<Response<Tareas>>> GetAllTasksAsync()
            => await _service.GetAllTasksAsync();

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Tareas>>> GetTaskByIdAsync(int id)
            => await _service.GetTaskByIdAsync(id);


        //LOS POSTS
        [HttpPost]
        public ActionResult AddTaskAsync(Tareas tarea)
        {
            var response = new Response<string>
            {
                Successful = true,
                Message = "Tarea agregada a la cola."
            };
            _taskQueue.EnqueueTask(tarea);
            _notificationService.NotifyNewTaskAsync(tarea).ConfigureAwait(false);
            return Ok(response);
        }


        [HttpPost("high-priority")]
        public ActionResult AddHighPriorityTaskAsync([FromBody] string description)
        {
            var tarea = Application.Services.TaskServices.TaskFactory.CreateHighPriorityTask(description);
            var response = new Response<string>
            {
                Successful = true,
                Message = "Tarea de alta prioridad agregada a la cola."
            };
            _taskQueue.EnqueueTask(tarea);
            return Ok(response);
        }

        [HttpPost("low-priority")]
        public ActionResult AddLowPriorityTaskAsync([FromBody] string description)
        {
            var tarea = Application.Services.TaskServices.TaskFactory.CreateLowPriorityTask(description);
            var response = new Response<string>
            {
                Successful = true,
                Message = "Tarea de baja prioridad agregada a la cola."
            };
            _taskQueue.EnqueueTask(tarea);
            return Ok(response);
        }


        //LOS PUTS
        [HttpPut]
        public async Task<ActionResult<Response<string>>> UpdateTaskAsync(Tareas tarea)
            => await _service.UpdateTaskAsync(tarea);


        //LOS DELETES
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> DeleteTaskAsync(int id)
            => await _service.DeleteTaskAsync(id);

    }

}
