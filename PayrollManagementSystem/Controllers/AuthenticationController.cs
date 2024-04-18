using Microsoft.AspNetCore.Mvc;
using PayrollManagementSystem.Services.Authentication;

namespace PayrollManagementSystem.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authenticationService;
        public AuthenticationController(IAuthService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var isAuthenticated = await _authenticationService.AuthenticateUserAsync(model.Username, model.Password);

            if (!isAuthenticated)
                return Unauthorized();

            // Generate JWT token
            var token = _authenticationService.GenerateToken();

            return Ok(new { Token = token });
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
