namespace UI.Services
{
    public class AuthorizationService
    {
        private string? _accessToken;

        public AuthorizationService(string? accessToken)
        {
            _accessToken = accessToken;
        }


        public string? Get()
        {
            return _accessToken;
        }

        public void UpdateAccessToken(string newAccessToken)
        {
            _accessToken = newAccessToken;
        }
    }
}
