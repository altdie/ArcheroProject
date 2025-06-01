namespace Project.Scripts.Auth
{
    public class AuthPresenter
    {
        private readonly AuthManager _authManager;

        public AuthPresenter(AuthManager authManager)
        {
            _authManager = authManager;
        }

        public void OnSignInClicked()
        {
            _authManager.SignIn();
        }
    }
}