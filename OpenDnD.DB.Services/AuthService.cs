using Microsoft.Extensions.DependencyInjection;
using OpenDnD.Interfaces;

namespace OpenDnD.DB.Services
{
    public class AuthService : IAuthService
    {
        public AuthService(OpenDnDContext openDnDContext, Secret secret, IPlayerService playerService, ApplicationAuthToken applicationAuthToken)
        {
            OpenDnDContext = openDnDContext;
            Secret = secret;
            PlayerService = playerService;
            ApplicationAuthToken = applicationAuthToken;
        }

        OpenDnDContext OpenDnDContext { get; }
        Secret Secret { get; }
        public IPlayerService PlayerService { get; }
        public ApplicationAuthToken ApplicationAuthToken { get; }

        public AuthToken Register(Uri address, string login, string password)
        {
            if (OpenDnDContext.Players.Any(x => x.UserName == login))
                throw new Exception("User already exist");
            var (hash, salt) = CryptoService.GetHashSaltPair(password);

            var playerId = PlayerService.Create(ApplicationAuthToken.AuthToken, new PlayerRequest
            {
                PasswordSalt = salt,
                PasswordHash = hash,
                UserName = login,
            });

            var token = CryptoService.GetAuthToken(playerId, Secret.SecretKey);

            return new AuthToken { 
                PlayerId = playerId,
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
