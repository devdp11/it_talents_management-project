using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Models;
using BusinessLogic.databaseContext;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly databaseContext _dbContext;
        private readonly IConfiguration _configuration;

        public UsersController(databaseContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            var users = _dbContext.Users.ToList();
            return Ok(users);
        }
        
        [HttpPost]
        public ActionResult<User> CreateUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetUserById), new { id = user.Userid }, user);
        }

        [HttpGet("{id}")]
        public ActionResult<UserModel> GetUserById(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Userid == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(new UserModel(){Password = user.Password, Username = user.Username});
        }
        
        [HttpGet("username/{username}")]
        public ActionResult<User> GetUserByUsername(string username)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        
        [HttpGet("role/{roleName}")]
        public ActionResult<IEnumerable<User>> GetUsersByRoleName(string roleName)
        {
            var users = _dbContext.Users.Where(u => u.Role.Name == roleName).ToList();
            if (!users.Any())
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Userid == id);
            if (user == null)
            {
                return NotFound();
            }
            user.Username = updatedUser.Username;
            user.Password = updatedUser.Password;
            user.Roleid = updatedUser.Roleid;
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Userid == id);
            if (user == null)
            {
                return NotFound();
            }
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            return NoContent();
        }
        
        [HttpPost("token")]
        public IActionResult GenerateToken([FromBody] AuthModel login) 
        { 
            if (IsValidUser(login))
            {
                var token = GenerateJwtToken(login.Username); 
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }

        private bool IsValidUser(AuthModel login)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == login.Username);
            if (user != null)
            {
                if (user.Password == login.Password)
                {
                    return true; // ao retornar true, o username e password foram validados
                }
            }
            return false; // ao retornar true, o username e password não foram validados
        }

        private string GenerateJwtToken(string username) 
        { 
            var claims = new[] { new Claim(ClaimTypes.Name, username) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])); 
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); 
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:TokenExpirationTimeInMinutes"]));
            var token = new JwtSecurityToken( _configuration["JwtSettings:Issuer"], _configuration["JwtSettings:Audience"],
                claims, expires: expires, signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}