using Microsoft.AspNetCore.Mvc;
using MedicineStoreAPI.Services;
using MedicineStoreAPI.Models;

namespace MedicineStoreAPI.Controllers
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

        [HttpPost("register")]
        public IActionResult Register([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username and password are required.");

            var result = _authService.RegisterUser(request.Username, request.Password);

            if (!result)
                return Conflict("User already exists.");

            return Ok("Registration successful.");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username and password are required.");

            var isValid = _authService.ValidateUser(request.Username, request.Password);

            if (!isValid)
                return Unauthorized("Invalid username or password.");

            var token = _authService.GenerateJwtToken(request.Username);

            return Ok(new { Token = token });
        }
    }
}



