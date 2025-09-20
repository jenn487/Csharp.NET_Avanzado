using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TaskManagement.API.Controllers;
using TaskManagement.Application.Services;
using TaskManagement.Domain.DTO;
using Xunit;

namespace TaskManagement.Tests.Controllers // login y autenticacion de las pruebas
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Jwt:Key", "clave-clave-clave-clave-clave-1234567890"},
                    {"Jwt:Issuer", "TaskManagementAPI"},
                    {"Jwt:Audience", "TaskManagementClient"},
                    {"Jwt:ExpireMinutes", "60"}
                })
                .Build();

            var authService = new AuthService(config);
            _controller = new AuthController(authService);
        }

        [Fact]
        public void Login_WithValidAdmin_ReturnsToken()
        {
            var request = new LoginRequest { Username = "admin", Password = "1234" };

            var result = _controller.Login(request) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Contains("token", result.Value!.ToString()!);
        }

        [Fact]
        public void Login_WithValidUser_ReturnsToken()
        {
            var request = new LoginRequest { Username = "user", Password = "1234" };

            var result = _controller.Login(request) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Contains("token", result.Value!.ToString()!);
        }

        [Fact]
        public void Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            var request = new LoginRequest { Username = "fake", Password = "wrong" };

            var result = _controller.Login(request);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}
