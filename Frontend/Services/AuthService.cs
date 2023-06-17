namespace Frontend.Services;

public class AuthService
{
    private bool _isAuthenticated = false;
    private string _role = "";

    public bool IsAuthenticated => _isAuthenticated;
    public string Role => _role;

    public void Login(string role)
    {
        _isAuthenticated = true;
        _role = role;
    }

    public void Logout()
    {
        _isAuthenticated = false;
        _role = "";
    }
}