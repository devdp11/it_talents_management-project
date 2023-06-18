using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.databaseContext;
using BusinessLogic.Entities;
using BusinessLogic.Models;

namespace Backend.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly databaseContext _context;

        public AuthController(databaseContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserModel>> HandleLogin(string username, string password)
        {
            // Check if the username and password match the records in the database
            var user = await _context.Users
                .Where(u => u.Username == username && u.Password == password)
                .Select(u => new UserModel
                {
                    UserID = u.Userid,
                    Username = u.Username,
                    RoleID = u.Role.Roleid,
                    RoleName = u.Role.Name
                })
                .FirstOrDefaultAsync();

            if (user != null)
            {
                // Authentication successful
                return user;
            }
            else
            {
                // Authentication failed
                return Unauthorized("Username or Password incorrect!");
            }
        }
        
    }
}