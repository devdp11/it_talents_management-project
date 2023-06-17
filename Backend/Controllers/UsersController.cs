using Backend.Models;
using BusinessLogic.databaseContext;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly databaseContext _dbContext;

        public UsersController(databaseContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserModel>> GetUsers()
        {
            var users = _dbContext.Users
                .Include(u => u.Role) 
                .AsEnumerable()
                .Select(u => new UserModel 
                { 
                    UserID = u.Userid,
                    Password = u.Password, 
                    Username = u.Username, 
                    RoleID = u.Roleid ?? 0,
                    RoleName = u.Role.Name 
                })
                .ToList();

            return Ok(users);
        }

        [HttpPost]
        public ActionResult<UserModel> CreateUser(UserModel userModel)
        {
            // Verifique se o nome de usuário já existe
            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Username == userModel.Username);

            // Se o usuário já existir, retorne um erro
            if (existingUser != null)
            {
                return BadRequest("Username already exists");
            }

            // Encontre a função pelo nome
            var role = _dbContext.Roles.FirstOrDefault(r => r.Name == userModel.RoleName);

            // Se a função não existir, retorne um erro
            if (role == null)
            {
                return BadRequest("Role does not exist");
            }

            // Crie o novo usuário e defina seu RoleID como o RoleID da função encontrada
            var user = new User 
            { 
                Username = userModel.Username, 
                Password = userModel.Password,
                Roleid = role.Roleid
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            userModel.UserID = user.Userid;
            userModel.RoleID = user.Roleid ?? 0;
            return CreatedAtAction(nameof(GetUserById), new { id = userModel.UserID }, userModel);
        }

        [HttpGet("{id}")]
        public ActionResult<UserModel> GetUserById(int id)
        {
            var user = _dbContext.Users.Include(u => u.Role).FirstOrDefault(u => u.Userid == id);
            if (user == null)
            {
                return NotFound();
            }

            var userModel = new UserModel 
            { 
                UserID = user.Userid, 
                Password = user.Password, 
                Username = user.Username,
                RoleID = user.Roleid ?? 0, // Adicionado esta linha
                RoleName = user.Role.Name 
            };
            return Ok(userModel);
        }

        [HttpGet("username/{username}")]
        public ActionResult<UserModel> GetUserByUsername(string username)
        {
            var user = _dbContext.Users.Include(u => u.Role).FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return NotFound();
            }

            var userModel = new UserModel 
            { 
                UserID = user.Userid, 
                Username = user.Username,
                Password = user.Password, 
                RoleID = user.Roleid ?? 0,
                RoleName = user.Role.Name 
            };
            return Ok(userModel);
        }

        [HttpGet("role/{roleName}")]
        public ActionResult<IEnumerable<UserModel>> GetUsersByRoleName(string roleName)
        {
            var users = _dbContext.Users
                .Include(u => u.Role)
                .Where(u => u.Role.Name == roleName)
                .ToList();

            if (!users.Any())
            {
                return NotFound();
            }

            var userModels = users.Select(u => new UserModel
            {
                UserID = u.Userid,
                Username = u.Username,
                Password = u.Password,
                RoleID = u.Roleid ?? 0,
                RoleName = u.Role.Name
            }).ToList();

            return Ok(userModels);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UserModel updatedUser)
        {
            var user = _dbContext.Users.Include(u => u.Role).FirstOrDefault(u => u.Userid == id);
            if (user == null)
            {
                return NotFound();
            }

            // Verifique se o usuário é um administrador
            if (user.Role.Name.ToLower() == "admin")
            {
                return BadRequest("Admin users cannot be updated");
            }

            // Para atualizar a RoleID, primeiro certifique-se de que a nova RoleID corresponde a um Role existente
            var role = _dbContext.Roles.FirstOrDefault(r => r.Roleid == updatedUser.RoleID);
            if (role == null)
            {
                return BadRequest("Role does not exist");
            }

            // Verifique se o Role é 'admin'
            if (role.Name.ToLower() == "admin")
            {
                return BadRequest("Users cannot be updated to admin role");
            }

            user.Username = updatedUser.Username;
            user.Password = updatedUser.Password;
            user.Roleid = updatedUser.RoleID;
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