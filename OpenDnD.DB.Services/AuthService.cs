using Microsoft.Extensions.DependencyInjection;
using OpenDnD.Interfaces;

namespace OpenDnD.DB.Services
{
    public class AuthService : IAuthService
    {
        public AuthService(OpenDnDContext openDnDContext, Secret secret)
        {
            OpenDnDContext = openDnDContext;
            Secret = secret;
        }

        OpenDnDContext OpenDnDContext { get; }
        Secret Secret { get; }

        public AuthToken Register(Uri address, string login, string password)
        {
            if (OpenDnDContext.Players.Any(x => x.UserName == login))
                throw new Exception("User already exist");
            var (hash, salt) = CryptoService.GetHashSaltPair(password);

            var user = new Player
            {
                PasswordHash = hash,
                PasswordSalt = salt,
                PlayerId = Guid.NewGuid(),
                UserName = login,
            };

            OpenDnDContext.Players.Add(user);
            OpenDnDContext.SaveChanges();

            var token = CryptoService.GetAuthToken(user.PlayerId, Secret.SecretKey);

            return new AuthToken { 
                PlayerId = user.PlayerId,
                TokenValue = token,
            };

        }

        public AuthToken Authenticate(Uri address, string login, string password)
        {
            var player = OpenDnDContext.Players.FirstOrDefault(x => x.UserName == login);
            if (player is null)
                throw new Exception("User doesn't exist");

            if (!CryptoService.ValidatePasswordSaltHash(password, player.PasswordSalt, player.PasswordHash))
                throw new Exception("Invalid password");

            var token = CryptoService.GetAuthToken(player.PlayerId, Secret.SecretKey);

            return new AuthToken
            {
                PlayerId = player.PlayerId,
                TokenValue = token,
            };
        }

        public bool ValidateAuthToken(AuthToken authToken)
            => CryptoService.ValidateAuthToken(authToken, Secret.SecretKey);
    }
}
