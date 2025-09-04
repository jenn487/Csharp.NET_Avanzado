using TaskManagement.Domain.DTO;
using TaskManagement.Domain.Models;
using TaskManagement.Infrastructure.Repository.Common;

namespace TaskManagement.Application.Services.TaskServices
{
    public class TaskService
    {
        private readonly ICommonProcess<Tareas> _commonsProcess;
        public TaskService(ICommonProcess<Tareas> commonsProcess)
        {
            _commonsProcess = commonsProcess;
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
