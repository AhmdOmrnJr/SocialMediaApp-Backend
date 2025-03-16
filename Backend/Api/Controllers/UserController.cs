using Api.DTOs;
using Api.Entities;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class UserController(IAuthService authService) : ApiBaseController
    {

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return BadRequest("Please fill all the required fields correctly");

            var result = await authService.RegisterAsync(registerDto);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginAsync(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please fill all the required fields");
            }
            var result = await authService.LoginAsync(loginDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return Unauthorized(result);
        }

        [HttpGet]
        public async Task<ActionResult<ResultDto<IEnumerable<AppUser?>>>> GetAllUsers()
        {
            var result = await authService.GetAllUsers();

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{Id:guid}")]
        public async Task<ActionResult<ResultDto<IEnumerable<AppUser?>>>> GetUser(Guid Id)
        {
            var result = await authService.GetUser(Id);

            if (result is null)
                return NotFound();

            return Ok(result);
        }
    }
}
