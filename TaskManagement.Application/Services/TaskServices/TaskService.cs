using TaskManagement.Domain.DTO;
using TaskManagement.Domain.Models;
using TaskManagement.Infrastructure.Repository.Common;

namespace TaskManagement.Application.Services.TaskServices
{
    public class TaskService
    {
        private readonly ICommonProcess<Tareas> _commonsProcess;

        //delegado
        private readonly Func<Tareas, (bool IsValid, string ErrorMessage)> _validateTask;

        //action de notificacion
        private readonly Action<Tareas> _notifyCreation;

        //func para calcular dias restantes
        private readonly Func<Tareas, int> _calculateDaysRemaining;
        public TaskService(ICommonProcess<Tareas> commonsProcess)
        {
            _commonsProcess = commonsProcess;

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
                var validStatuses = new List<string> { "Pendiente","En proceso", "Completada" };
                if (!validStatuses.Contains(tarea.Status))
                {
                    return (false, "El estado debe ser 'Pendiente', 'En proceso' o 'Completada'.");
                }
                return (true, string.Empty);
            };

            _notifyCreation = tarea =>
                Console.WriteLine($"Se ha creado una nueva tarea '{tarea.Description}'. Vence en {_calculateDaysRemaining(tarea)} dia(s).");

            //func para calcular dias restantes
            _calculateDaysRemaining = tarea =>
            {
                var diferencia = tarea.DueDate.Date - DateTime.Now.Date;
                return diferencia.Days > 0 ? diferencia.Days : 0;
            };
        }
        //Gets
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
        // Gets by Id
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
                //action para registrar la creacion dela tarea
                _notifyCreation(tarea);

                tarea.ExtraData = $"{_calculateDaysRemaining(tarea)} dias restantes.";

                var result = await _commonsProcess.AddAsync(tarea);
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

    }
}
