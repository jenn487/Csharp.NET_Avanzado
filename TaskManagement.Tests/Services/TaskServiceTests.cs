using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Application.Services.TaskServices;
using TaskManagement.Domain.Models;
using TaskManagement.Infrastructure.Repository.Common;
using Xunit;

namespace TaskManagement.Tests.Services // pruebas unitarias del servicio de tareas
{
    public class TaskServiceTests
    {
        private readonly Mock<ICommonProcess<Tareas>> _repoMock;
        private readonly TaskService _service;

        public TaskServiceTests()
        {
            _repoMock = new Mock<ICommonProcess<Tareas>>();
            _service = new TaskService(_repoMock.Object);
        }

        [Fact]
        public async Task AddTask_WithValidData_ShouldBeSuccessful()
        {
            var tarea = new Tareas
            {
                Description = "Tarea de prueba válida",
                DueDate = DateTime.Now.AddDays(2),
                Status = "Pendiente"
            };

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Tareas>()))
                     .ReturnsAsync((true, "¡Guardada Correctamente!"));

            var result = await _service.AddTaskAsync(tarea);

            Assert.True(result.Successful);
            Assert.Equal("¡Guardada Correctamente!", result.Message);
        }

        [Fact]
        public async Task AddTask_WithInvalidDescription_ShouldFail()
        {
            var tarea = new Tareas
            {
                Description = "Hey",
                DueDate = DateTime.Now.AddDays(2),
                Status = "Pendiente"
            };

            var result = await _service.AddTaskAsync(tarea);

            Assert.False(result.Successful);
            Assert.Contains("al menos 5 caracteres", result.Errors.First());
        }

        [Fact]
        public async Task GetTaskById_WhenExists_ReturnsTask()
        {
            var tarea = new Tareas { Id = 1, Description = "Test tarea", DueDate = DateTime.Now.AddDays(2), Status = "Pendiente" };

            _repoMock.Setup(r => r.GetIdAsync(1)).ReturnsAsync(tarea);

            var result = await _service.GetTaskByIdAsync(1);

            Assert.True(result.Successful);
            Assert.Equal("Test tarea", result.SingleData.Description);
        }

        [Fact]
        public async Task GetTaskById_WhenNotExists_ReturnsError()
        {
            _repoMock.Setup(r => r.GetIdAsync(99)).ReturnsAsync((Tareas)null);

            var result = await _service.GetTaskByIdAsync(99);

            Assert.False(result.Successful);
            Assert.Contains("Tarea no encontrada", result.Errors.First());
        }

        [Fact]
        public async Task UpdateTask_WhenExists_ShouldBeSuccessful()
        {
            var tarea = new Tareas { Id = 1, Description = "Update", DueDate = DateTime.Now.AddDays(2), Status = "Pendiente" };

            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Tareas>()))
                     .ReturnsAsync((true, "¡Actualizada Correctamente!"));

            var result = await _service.UpdateTaskAsync(tarea);

            Assert.True(result.Successful);
            Assert.Equal("¡Actualizada Correctamente!", result.Message);
        }

        [Fact]
        public async Task DeleteTask_WhenExists_ShouldBeSuccessful()
        {
            _repoMock.Setup(r => r.DeleteAsync(1))
                     .ReturnsAsync((true, "¡Eliminada Correctamente!"));

            var result = await _service.DeleteTaskAsync(1);

            Assert.True(result.Successful);
            Assert.Equal("¡Eliminada Correctamente!", result.Message);
        }

        [Fact]
        public async Task CalculateTaskCompletionRate_WithTasks_ReturnsCorrectPercentage()
        {
            var tareas = new List<Tareas>
            {
                new Tareas { Id = 1, Description = "T1", DueDate = DateTime.Now.AddDays(1), Status = "Completada" },
                new Tareas { Id = 2, Description = "T2", DueDate = DateTime.Now.AddDays(2), Status = "Pendiente" }
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tareas);

            var result = await _service.CalculateTaskCompletionRateAsync();

            Assert.Equal(50, result);
        }

        [Fact]
        public async Task GetTasksByStatus_ReturnsFilteredTasks()
        {
            var tareas = new List<Tareas>
            {
                new Tareas { Id = 1, Description = "T1", DueDate = DateTime.Now.AddDays(1), Status = "Pendiente" },
                new Tareas { Id = 2, Description = "T2", DueDate = DateTime.Now.AddDays(2), Status = "Completada" }
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tareas);

            var result = await _service.GetTasksByStatusAsync("Completada");

            Assert.Single(result);
            Assert.Equal("Completada", result.First().Status);
        }
    }
}
