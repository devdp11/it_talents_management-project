using BusinessLogic.Entities;

namespace Backend.Models;

public class UserModel
{
    public int UserID { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int RoleID { get; set; }
    public string RoleName { get; set; } // Adicionado esta propriedade
}