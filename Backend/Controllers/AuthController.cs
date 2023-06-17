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
        public async Task<IActionResult> HandleLogin(string username, string password)
        {
            // Check if the username and password match the records in the database
            var users =
                await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (users != null)
            {
                // Authentication successful
                return Ok("Logged in Sucessfully!");
            }

            if (password.Length < 6)
            {
                return Unauthorized("Password must be over 6 digits!");
            }
            else
            {
                // Authentication failed
                return Unauthorized("Wrong Username or Password!");
            }
        }
    }
}
