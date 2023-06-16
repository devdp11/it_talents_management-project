using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Entities;
using BusinessLogic.databaseContext;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly databaseContext _dbContext;

        public UsersController(databaseContext dbContext)
        {
            _dbContext = dbContext;
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
        public ActionResult<User> GetUserById(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Userid == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
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
    }
}
