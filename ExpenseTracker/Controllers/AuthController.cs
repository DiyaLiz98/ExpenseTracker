using ExpenseTracker.Interfaces;
using ExpenseTracker.Models.DTO;
using ExpenseTracker.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository; 

        public AuthController(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if(string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return BadRequest("Email and Password are required.");
            }

            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
                return Unauthorized("User not found.");

            if (user.Password != loginDto.Password)
                return Unauthorized("Invalid password.");

            var secretKey = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expireMinutes = Convert.ToInt32(_config["Jwt:ExpireMinutes"]);

            var token = JwtTokenGenerator.GenerateToken(user.Id.ToString(),user.Role,secretKey,issuer,audience,expireMinutes);

            return Ok(new { token });
        }

    }
}
