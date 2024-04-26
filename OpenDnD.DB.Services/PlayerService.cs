using Microsoft.EntityFrameworkCore;
using OpenDnD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDnD.DB.Services
{
    public class PlayerService : IPlayerService, IAuthService
    {
        public OpenDnDContext OpenDnDContext { get; }
        public Secret Secret { get; }
        public ApplicationAuthToken ApplicationAuthToken { get; }
        public IAuthService AuthService { get; }

        public PlayerService(OpenDnDContext openDnDContext, Secret secret, ApplicationAuthToken applicationAuthToken, IAuthService authService)
        {
            OpenDnDContext = openDnDContext;
            Secret = secret;
            ApplicationAuthToken = applicationAuthToken;
            AuthService = authService;
        }

        public Guid Create(AuthToken authToken, PlayerRequest request)
        {
            AuthService.ValidateAuthTokenAndThrowExceptionOnError(authToken);

            var player = new Player
            {
                UserName = request.UserName,
                PlayerId = Guid.NewGuid(),
                PasswordHash = request.PasswordHash,
                PasswordSalt = request.PasswordSalt
            };
            OpenDnDContext.Players.Add(player);
            OpenDnDContext.SaveChanges();
            return player.PlayerId;
        }

        public void Delete(AuthToken authToken, Guid id)
        {
            AuthService.ValidateAuthTokenAndThrowExceptionOnError(authToken);

            OpenDnDContext.Players.Where(x => x.PlayerId == id).ExecuteDelete();
        }

        public Interfaces.Player Get(AuthToken authToken, Guid id)
        {
            AuthService.ValidateAuthTokenAndThrowExceptionOnError(authToken);

            var player = OpenDnDContext.Players.FirstOrDefault(x => x.PlayerId == id);
            return new Interfaces.Player(player.PlayerId, player.UserName);
        }

        public List<Interfaces.Player> GetList(AuthToken authToken)
        {
            AuthService.ValidateAuthTokenAndThrowExceptionOnError(authToken);

            return OpenDnDContext.Players.Select(x => new Interfaces.Player(x.PlayerId, x.UserName)).ToList();
        }

        public void Update(AuthToken authToken, Guid id, PlayerRequest request)
        {
            AuthService.ValidateAuthTokenAndThrowExceptionOnError(authToken);

            var player = OpenDnDContext.Players.FirstOrDefault(x => x.PlayerId == id);

            if (player is null)
            {
                throw new Exception("Player not found");
            }

            if (request.UserName is not null)
            {
                player.UserName = request.UserName;
            }

            if (request.PasswordSalt is not null && request.PasswordHash is not null)
            {
                player.PasswordSalt = request.PasswordSalt;
                player.PasswordHash = request.PasswordHash;
            }

            OpenDnDContext.SaveChanges();
        }

        public AuthToken Register(Uri address, string login, string password)
        {
            if (OpenDnDContext.Players.Any(x => x.UserName == login))
                throw new Exception("User already exist");
            var (hash, salt) = CryptoService.GetHashSaltPair(password);

            var playerId = Create(ApplicationAuthToken.AuthToken, new PlayerRequest
            {
                PasswordSalt = salt,
                PasswordHash = hash,
                UserName = login,
            });

            var token = CryptoService.GetAuthToken(playerId, Secret.SecretKey);

            return new AuthToken
            {
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

        public List<Interfaces.Player> GetPlayerListFromSession(AuthToken authToken, List<Guid> P)
        {
            AuthService.ValidateAuthTokenAndThrowExceptionOnError(authToken);

            return OpenDnDContext.Players.Where(x => P.Contains(x.PlayerId))
                .Select(x => new Interfaces.Player(x.PlayerId, x.UserName)).ToList();
        }

        public Interfaces.Player GetPlayerByName(AuthToken authToken, string name)
        {
            AuthService.ValidateAuthTokenAndThrowExceptionOnError(authToken);

            var player = OpenDnDContext.Players.FirstOrDefault(x => x.UserName == name);

            if (player is null)
            {
                throw new Exception("Player not found");
            }

            return new Interfaces.Player(player.PlayerId, player.UserName);
        }
    }
}
