using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("[controller]")]
public class TokenController : ControllerBase
{
    public IConfiguration _configuration;
    private readonly MySQLDBContext _context;

    public TokenController(IConfiguration config, MySQLDBContext context)
    {
        _configuration = config;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateToken(string userName, string password)
    {
        if (userName != null && password != null)
        {
            UserModel user = await GetUser(userName, password);

            if (user != null)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("FullName", user.FullName),
                    new Claim("UserName", user.UserName),
                    new Claim("Email", user.Email)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            else
            {
                return BadRequest("Invalid credentials");
            }
        }
        else
        {
            return BadRequest();
        }
    }

    private async Task<UserModel> GetUser(string userName, string password)
    {
        return await _context.User.FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password);
    }
}