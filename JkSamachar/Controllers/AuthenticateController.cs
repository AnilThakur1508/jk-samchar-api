using JKSamachar.DTO;
using JKSamachar.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JkSamachar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService _authService;
        public AuthenticateController(IAuthenticateService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }
            try
            {
                var result = await _authService.RegisterUser(registerDto);
                if (result)
                {
                    return Ok(result);
                }
                else
                {
                    throw new Exception("Registration failed.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });

            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }
            try
            {
                var token = await _authService.LoginUser(loginDto);
                if (token != null)
                {
                    return Ok(new { Token = token });
                }
                else
                {
                    throw new Exception("Login failed.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });

            }
        }
        [HttpGet("GetRoles")]
        public async Task<ActionResult<List<RoleResponseDto>>> GetAllRoles()
        {
            try
            {
                var roles = await _authService.GetAllRoleAsync();

                if (roles == null )
                {
                    throw new Exception ("Role is null");
                }

                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
