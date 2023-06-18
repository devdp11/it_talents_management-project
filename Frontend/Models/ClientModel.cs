namespace Frontend.Models;

public class ClientModel
{
    public int ClientID { get; set; }
    public int UserID { get; set; }
    public string Name { get; set; }
    public UserModel User { get; set; }
}