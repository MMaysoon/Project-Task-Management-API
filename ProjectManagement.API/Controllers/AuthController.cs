using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Common;
using ProjectManagement.Application.Dtos.Auth;
using ProjectManagement.Application.IServices;

namespace ProjectManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register (RegisterDTO dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(new ApiResponse<AuthResponseDTO>
            {
                Success = true,
                Data = result,
                StatusCode = 200
            });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);

            return Ok(new ApiResponse<AuthResponseDTO>
            {
                Success = true,
                Data = result,
                StatusCode = 200
            });
        }
    }
}
