using Application.DTOS.Api; 
using Application.DTOS.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assessment.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.FailureResponse("Invalid registration inputs."));

            var result = await authService.Register(registerDto);

            if (!result.IsAuthenticated)
                return BadRequest(ApiResponse<object>.FailureResponse(result.Message));

            var response = ApiResponse<object>.SuccessResponse(result, "User registered successfully.");
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogienDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.FailureResponse("Invalid login inputs."));

            var result = await authService.Login(loginDto);

            if (!result.IsAuthenticated)
                return Unauthorized(ApiResponse<object>.FailureResponse(result.Message));

            var response = ApiResponse<object>.SuccessResponse(result, "Logged in successfully.");
            return Ok(response);
        }
    }
}