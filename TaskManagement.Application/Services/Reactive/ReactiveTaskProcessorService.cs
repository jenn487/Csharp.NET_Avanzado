using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reactive.Linq;
using TaskManagement.Application.Services.TaskServices;
using TaskManagement.Domain.DTO;

namespace TaskManagement.Application.Services.Reactive;

public class ReactiveTaskProcessorService : BackgroundService
{
    private readonly IReactiveTaskQueue _taskQueue;
    private readonly ILogger<ReactiveTaskProcessorService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public ReactiveTaskProcessorService(
        IReactiveTaskQueue taskQueue,
        ILogger<ReactiveTaskProcessorService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _taskQueue = taskQueue;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Procesamiento de tareas iniciado.");

        _taskQueue.TaskQueue
            .SelectMany(async tarea =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var taskService = scope.ServiceProvider.GetRequiredService<TaskService>();
                    _logger.LogInformation($"Procesando tarea: {tarea.Description}");

                    var result = await taskService.AddTaskAsync(tarea);

                    _logger.LogInformation("Tarea procesada correctamente.");
                    return result;
                }
            })
            .Subscribe(
                onNext: response =>
                {
                    if (!response.Successful)
                    {
                        _logger.LogError($"Error en el servicio: {response.Message}");
                    }
                },
                onError: ex => _logger.LogError(ex, "Error en el flujo de procesamiento de tareas."),
                onCompleted: () => _logger.LogInformation("Flujo de tareas completado."));

        return Task.CompletedTask;
    }
}