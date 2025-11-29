using System.IdentityModel.Tokens.Jwt;
using JKSamachar.DTO;
using JKSamachar.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JkSamachar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JKNewsController : ControllerBase
    {
        private readonly IJKNewsServices _jKNewsServices;
        public JKNewsController(IJKNewsServices jKNewsServices)
        {
            _jKNewsServices = jKNewsServices;
        }

        [HttpGet("GetAllJkNews")]
        public async Task<IActionResult> GetAllJKNews([FromQuery] string roleName)
        {
            try
            {
                var result = await _jKNewsServices.GetAllJKNews(roleName);
                if (result == null)
                {
                    throw new Exception("News not found");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("GetJkNewsById")]
        public async Task<IActionResult> GetJKNewsById([FromQuery] string id) 
        {
            try
            {
                var result = await _jKNewsServices.GetJKNewsById(id);
                if (result == null)
                {
                    throw new Exception("News not found");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpDelete("DeleteJkNews")]
        public async Task<IActionResult> DeleteJKNews([FromQuery] string id)
        {
            try
            {
                var result = await _jKNewsServices.DeleteJKNews(id);
                if (!result)
                {
                    throw new Exception("News not found");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpPost("AddJKNews")]
        public async Task<IActionResult> AddJkNews([FromBody] JkNewsDto jkNewsDto) 
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                var token = authHeader.StartsWith("Bearer ") ? authHeader.Substring(7) : null;
                var result = await _jKNewsServices.AddJKNews(jkNewsDto, token);
                if (!result)
                {
                    throw new Exception("News not found");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpPut("UpdateJkNews")]
        public async Task<IActionResult> UpdateJKNews([FromQuery] string id, [FromBody] JkNewsDto jkNewsDto)
        {
            try
            {
                var result = await _jKNewsServices.UpdateJKNews(id, jkNewsDto);
                if (!result)
                {
                    throw new Exception("News not found");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("No file uploaded");

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var filePath = Path.Combine(uploadsPath, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { fileName = file.FileName, fileUrl = $"/uploads/{file.FileName}" });
        }
    }
}
