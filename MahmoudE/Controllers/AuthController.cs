using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EFilmStore.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EFilmStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // Enkel lista i minnet – ingen databas
        private static List<AppUser> _users = new();

        private readonly JwtSettings _jwtSettings;

        public AuthController(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] AppUser newUser)
        {
            if (_users.Any(u => u.Username == newUser.Username))
                return BadRequest("Användarnamnet är redan registrerat.");

            _users.Add(newUser);
            return Ok("Registreringen lyckades.");
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] AppUser login)
        {
            var user = _users.FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);
            if (user == null)
                return Unauthorized("Fel användarnamn eller lösenord.");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }

    public class AppUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
