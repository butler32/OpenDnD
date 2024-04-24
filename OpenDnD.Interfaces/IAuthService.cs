namespace OpenDnD.Interfaces
{
    public class AuthToken
    {

    }
    public interface IAuthService
    {
        public AuthToken Authenticate(Uri address, string login, string password);
    }

}
