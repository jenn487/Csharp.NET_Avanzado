using TaskManagement.Application.Services.Memoization;
using TaskManagement.Domain.DTO;
using TaskManagement.Domain.Models;
using TaskManagement.Infrastructure.Repository.Common;
using TaskManagement.Application.Services.Notifications;

namespace TaskManagement.Application.Services.TaskServices
{
    public class TaskService
    {
        private readonly ICommonProcess<Tareas> _commonsProcess;
        private readonly INotificationService _notificationService;

        // delegado
        private readonly Func<Tareas, (bool IsValid, string ErrorMessage)> _validateTask;

        // action de notificacion
        private readonly Action<Tareas> _notifyCreation;

        // func para calcular dias restantes
        private readonly Func<Tareas, int> _calculateDaysRemaining;

        public TaskService(ICommonProcess<Tareas> commonsProcess, INotificationService notificationService)
        {
            _commonsProcess = commonsProcess ?? throw new ArgumentNullException(nameof(commonsProcess));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));

            // Func para calcular dias restantes
            _calculateDaysRemaining = tarea =>
            {
                var diferencia = tarea.DueDate.Date - DateTime.Now.Date;
                return diferencia.Days > 0 ? diferencia.Days : 0;
            };

            _validateTask = (tarea) =>
            {
                if (string.IsNullOrWhiteSpace(tarea.Description) || tarea.Description.Length < 5)
                {
                    return (false, "La descripción debe tener al menos 5 caracteres.");
                }
                if (tarea.DueDate < DateTime.Now)
                {
                    return (false, "La fecha de vencimiento debe ser en el futuro.");
                }
                var validStatuses = new List<string> { "Pendiente", "En proceso", "Completada" };
                if (!validStatuses.Contains(tarea.Status))
                {
                    return (false, "El estado debe ser 'Pendiente', 'En proceso' o 'Completada'.");
                }
                return (true, string.Empty);
            };

            _notifyCreation = tarea =>
                Console.WriteLine($"Se ha creado una nueva tarea '{tarea.Description}'. Vence en {_calculateDaysRemaining(tarea)} dia(s).");
        }

        // Gets
        public async Task<Response<Tareas>> GetAllTasksAsync()
        {
            var response = new Response<Tareas>();
            try
            {
                response.DataList = await _commonsProcess.GetAllAsync();
                response.Successful = true;
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        // Get by Id
        public async Task<Response<Tareas>> GetTaskByIdAsync(int id)
        {
            var response = new Response<Tareas>();
            try
            {
                var result = await _commonsProcess.GetIdAsync(id);
                if (result != null)
                {
                    response.SingleData = result;
                    response.Successful = true;
                }
                else
                {
                    response.Successful = false;
                    response.Errors.Add("Tarea no encontrada.");
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        // Adds
        public async Task<Response<string>> AddTaskAsync(Tareas tarea)
        {
            var response = new Response<string>();
            try
            {
                var validateResult = _validateTask(tarea);
                if (!validateResult.IsValid)
                {
                    response.Successful = false;
                    response.Errors.Add(validateResult.ErrorMessage);
                    return response;
                }

                tarea.ExtraData = $"{_calculateDaysRemaining(tarea)} dia(s) restantes.";

                // el action para registrar la creacion de la tarea
                _notifyCreation(tarea);

                var result = await _commonsProcess.AddAsync(tarea);

                if (result.IsSuccess)
                {
                    try
                    {
                        await _notificationService.NotifyNewTaskAsync(tarea);
                    }
                    catch (Exception notifyEx)
                    {
                        Console.WriteLine($"Error al enviar notificación: {notifyEx.Message}");
                    }
                }

                MemoizationCache.Clear(); // limpiar cache para recalcular

                response.Message = result.Message;
                response.Successful = result.IsSuccess;
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        // Updates
        public async Task<Response<string>> UpdateTaskAsync(Tareas tarea)
        {
            var response = new Response<string>();
            try
            {
                var result = await _commonsProcess.UpdateAsync(tarea);

                MemoizationCache.Clear(); // limpiar cache para recalcular

                response.Message = result.Message;
                response.Successful = result.IsSuccess;
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        // Deletes
        public async Task<Response<string>> DeleteTaskAsync(int id)
        {
            var response = new Response<string>();
            try
            {
                var tarea = await _commonsProcess.GetIdAsync(id);
                if (tarea == null)
                {
                    response.Successful = false;
                    response.Errors.Add("Tarea no encontrada.");
                    return response;
                }

                var result = await _commonsProcess.DeleteAsync(id);
                response.Message = result.Message;
                response.Successful = result.IsSuccess;
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        // memorizacion por tareas completadas 
        public async Task<double> CalculateTaskCompletionRateAsync()
        {
            return await MemoizationCache.GetOrAddAsync("CompletionRate", async () =>
            {
                var allTasks = await _commonsProcess.GetAllAsync();
                var total = allTasks.Count();
                if (total == 0) return 0.0;
                var completed = allTasks.Count(t => t.Status == "Completada");
                return (double)completed / total * 100;
            });
        }

        // filtro por estado con memorizacion 
        public async Task<IEnumerable<Tareas>> GetTasksByStatusAsync(string status)
        {
            return await MemoizationCache.GetOrAddAsync($"Filter:{status}", async () =>
            {
                var allTasks = await _commonsProcess.GetAllAsync();
                return allTasks.Where(t => t.Status == status).ToList();
            });
        }

    }
}