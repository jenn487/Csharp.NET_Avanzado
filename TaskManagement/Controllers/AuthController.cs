using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Services;
using TaskManagement.Domain.DTO;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {

            if (request.Username == "admin" && request.Password == "1234")
            {
                var token = _authService.GenerateJwtToken(request.Username, "Admin");
                return Ok(new { token });
            }

            if (request.Username == "user" && request.Password == "1234")
            {
                var token = _authService.GenerateJwtToken(request.Username, "User");
                return Ok(new { token });
            }

            return Unauthorized("Usuario o contraseña invalido.");
        }
    }
}
