using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Models;
using BusinessLogic.databaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]

public class AuthController : ControllerBase {
    private readonly IConfiguration _configuration;
    public AuthController(IConfiguration configuration) { _configuration = configuration; }
    
    [HttpPost("token")]
    
    public IActionResult GenerateToken([FromBody] AuthModel login) { if (IsValidUser(login))
        { var token = GenerateJwtToken(login.Username); return Ok(new { Token = token });
        }
        return Unauthorized();
    }
    
    private bool IsValidUser(AuthModel login)
    {
        using (var dbContext = new databaseContext())
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Username == login.Username);
            if (user != null)
            {
                if (user.Password == login.Password)
                {
                    return true; // ao retornar true, o username e password foram validados
                }
            }
        }
        return false; // ao retornar true, o username e password não foram validados
    }
    private string GenerateJwtToken(string username) { var claims = new[] { new Claim(ClaimTypes.Name, username) };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])); var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:TokenExpirationTimeInMinutes"]));
        var token = new JwtSecurityToken( _configuration["JwtSettings:Issuer"], _configuration["JwtSettings:Audience"],
            claims, expires: expires, signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
