namespace Frontend.Services
{
    public class AuthService
    {
        private bool _isAuthenticated = false;
        private string _role = "";

        public bool IsAuthenticated => _isAuthenticated;
        public string Role => _role;

        // Adicionar esta linha
        public event Action OnChange;

        public void Login(string role)
        {
            _isAuthenticated = true;
            _role = role;
            NotifyStateChanged();
        }

        public void Logout()
        {
            _isAuthenticated = false;
            _role = "";
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}