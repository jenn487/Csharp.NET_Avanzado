using TaskManagement.Domain.DTO;
using TaskManagement.Domain.Models;
using TaskManagement.Infrastructure.Repository.Common;
using TaskManagement.Application.Factories;

namespace TaskManagement.Application.Services.TaskServices
{
    public class TaskService
    {
        private readonly ICommonProcess<Tareas> _commonsProcess;
        private readonly TaskValidationService _validationService;
        private readonly TaskFilterService _filterService;
        private readonly TaskTransformService _transformService;

        private readonly Action<Tareas> _onTaskCreated;
        private readonly Action<Tareas> _onTaskUpdated;
        private readonly Action<int> _onTaskDeleted;

        public TaskService(ICommonProcess<Tareas> commonsProcess)
        {
            _commonsProcess = commonsProcess;
            _validationService = new TaskValidationService();
            _filterService = new TaskFilterService();
            _transformService = new TaskTransformService();

            _onTaskCreated = tarea => _validationService.NotificationAction($"Tarea creada: {tarea.Description}");
            _onTaskUpdated = tarea => _validationService.NotificationAction($"Tarea actualizada: {tarea.Description}");
            _onTaskDeleted = id => _validationService.NotificationAction($"Tarea eliminada con ID: {id}");
        }

        public async Task<Response<Tareas>> GetAllTasksAsync(Func<Tareas, bool>? customFilter = null)
        {
            var response = new Response<Tareas>();
            try
            {
                var allTasks = await _commonsProcess.GetAllAsync();

                if (customFilter != null)
                {
                    allTasks = allTasks.Where(customFilter);
                    _filterService.LogFilterResults("Filtro personalizado", allTasks.Count());
                }

                response.DataList = allTasks;
                response.Successful = true;
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        public async Task<Response<Tareas>> GetPendingTasksAsync()
        {
            var response = new Response<Tareas>();
            try
            {
                var allTasks = await _commonsProcess.GetAllAsync();
                var pendingTasks = _filterService.ApplyFilter(allTasks, _filterService.PendingTasksFilter);

                _filterService.LogFilterResults("Tareas Pendientes", pendingTasks.Count());

                response.DataList = pendingTasks;
                response.Successful = true;
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        public async Task<Response<Tareas>> GetOverdueTasksAsync()
        {
            var response = new Response<Tareas>();
            try
            {
                var allTasks = await _commonsProcess.GetAllAsync();
                var overdueTasks = _filterService.ApplyFilter(allTasks, _filterService.OverdueTasksFilter);

                _filterService.LogFilterResults("Tareas Vencidas", overdueTasks.Count());

                response.DataList = overdueTasks;
                response.Successful = true;
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
            }
            return response;
        }

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


        public async Task<Response<string>> AddTaskAsync(Tareas tarea)
        {
            var response = new Response<string>();
            try
            {
                var validationResult = _validationService.ValidateWithMultipleRules(
                    tarea,
                    _validationService.DescriptionValidation,
                    _validationService.DateValidation,
                    _validationService.ExtraDataValidation
                );

                if (!validationResult.IsValid)
                {
                    response.Successful = false;
                    response.Errors.AddRange(validationResult.Errors);
                    return response;
                }

                var result = await _commonsProcess.AddAsync(tarea);
                response.Message = result.Message;
                response.Successful = result.IsSuccess;

                if (result.IsSuccess)
                {
                    _onTaskCreated(tarea);

                    var daysRemaining = _transformService.CalculateDaysRemaining(tarea);
                    response.Message += $" Dias restantes: {daysRemaining}";
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        public async Task<Response<Tareas>> SearchTasksByDescriptionAsync(string searchTerm)
        {
            var response = new Response<Tareas>();
            try
            {
                var allTasks = await _commonsProcess.GetAllAsync();

                var filteredTasks = allTasks.Where(t =>
                    t.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

                _filterService.LogFilterResults($"Busqueda por '{searchTerm}'", filteredTasks.Count());

                response.DataList = filteredTasks;
                response.Successful = true;
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        public async Task<Response<string>> UpdateTaskAsync(Tareas tarea)
        {
            var response = new Response<string>();
            try
            {
                var validationResult = _validationService.CompleteValidation(tarea);
                if (!validationResult.IsValid)
                {
                    response.Successful = false;
                    response.Errors.Add(validationResult.ErrorMessage);
                    return response;
                }

                var result = await _commonsProcess.UpdateAsync(tarea);
                response.Message = result.Message;
                response.Successful = result.IsSuccess;

                if (result.IsSuccess)
                {
                    _onTaskUpdated(tarea);
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        public async Task<Response<string>> DeleteTaskAsync(int id)
        {
            var response = new Response<string>();
            try
            {
                var result = await _commonsProcess.DeleteAsync(id);
                response.Message = result.Message;
                response.Successful = result.IsSuccess;

                if (result.IsSuccess)
                {
                    _onTaskDeleted(id);
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        public async Task<Response<string>> GetTasksSummaryAsync()
        {
            var response = new Response<string>();
            try
            {
                var allTasks = await _commonsProcess.GetAllAsync();

                // Func para transformar
                var summaries = allTasks.Select(t => _transformService.GetTaskSummary(t));
                var urgentCount = allTasks.Count(t => _transformService.IsUrgent(t));

                response.SingleData = string.Join("\n", summaries);
                response.Message = $"Total de tareas: {allTasks.Count()}, Urgentes: {urgentCount}";
                response.Successful = true;
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        public async Task<Response<string>> AddHighPriorityTaskAsync(string description)
        {
            var task = Factories.TaskFactory.CreateHighPriorityTask(description);
            return await AddTaskAsync(task);
        }

    }
}