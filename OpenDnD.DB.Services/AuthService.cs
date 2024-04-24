using OpenDnD.Interfaces;

namespace OpenDnD.DB.Services
{
    public class AuthService : IAuthService
    {
        public AuthService(OpenDnDContext openDnDContext)
        {
            OpenDnDContext = openDnDContext;
        }

        public OpenDnDContext OpenDnDContext { get; }

        public AuthToken Authenticate(Uri address, string login, string password)
        {
            return new AuthToken { };
        }
    }
}
