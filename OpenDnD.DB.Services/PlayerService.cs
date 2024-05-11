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

        public PlayerService(OpenDnDContext openDnDContext, Secret secret, ApplicationAuthToken applicationAuthToken)
        {
            OpenDnDContext = openDnDContext;
            Secret = secret;
            ApplicationAuthToken = applicationAuthToken;
        }

        public Guid Create(AuthToken authToken, PlayerRequest request)
        {
            this.CheckAuthTokenOrThrowException(authToken);

            ArgumentException.ThrowIfNullOrEmpty(request.UserName, nameof(PlayerRequest.UserName));
            ArgumentException.ThrowIfNullOrEmpty(request.PasswordHash, nameof(PlayerRequest.PasswordHash));
            ArgumentException.ThrowIfNullOrEmpty(request.PasswordSalt, nameof(PlayerRequest.PasswordSalt));

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
            this.CheckAuthTokenOrThrowException(authToken);

            OpenDnDContext.Players.Where(x => x.PlayerId == id).ExecuteDelete();
        }

        public Interfaces.Player Get(AuthToken authToken, Guid id)
        {
            this.CheckAuthTokenOrThrowException(authToken);

            var player = OpenDnDContext.Players.FirstOrDefault(x => x.PlayerId == id);
            if (player is null)
                throw new NoEntryWithRequiredIdException<Player>(id);

            return new Interfaces.Player(player.PlayerId, player.UserName);
        }

        public List<Interfaces.Player> GetList(AuthToken authToken)
        {
            this.CheckAuthTokenOrThrowException(authToken);

            return OpenDnDContext.Players.Select(x => new Interfaces.Player(x.PlayerId, x.UserName)).ToList();
        }

        public void Update(AuthToken authToken, Guid id, PlayerRequest request)
        {
            this.CheckAuthTokenOrThrowException(authToken);

            var player = OpenDnDContext.Players.FirstOrDefault(x => x.PlayerId == id);

            if (player is null)
                throw new NoEntryWithRequiredIdException<Player>(id);

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

            ArgumentException.ThrowIfNullOrEmpty(password, nameof(password));

            var (hash, salt) = CryptoService.GetHashSaltPair(password);

            var playerId = Create(ApplicationAuthToken.AuthToken, new PlayerRequest
            {
                PasswordSalt = salt,
                PasswordHash = hash,
                UserName = login,
            });

            var token = CryptoService.GetAuthToken(playerId, Secret.SecretKey);

            return new AuthToken(playerId, token);

        }

        public AuthToken Authenticate(Uri address, string login, string password)
        {
            var player = OpenDnDContext.Players.FirstOrDefault(x => x.UserName == login);
            if (player is null)
                throw new Exception("User doesn't exist");

            if (!CryptoService.ValidatePasswordSaltHash(password, player.PasswordSalt, player.PasswordHash))
                throw new Exception("Invalid password");

            var token = CryptoService.GetAuthToken(player.PlayerId, Secret.SecretKey);

            return new AuthToken(player.PlayerId, token);
        }

        public bool ValidateAuthToken(AuthToken authToken)
            => CryptoService.ValidateAuthToken(authToken, Secret.SecretKey);

        public List<Interfaces.Player> GetPlayerListFromSession(AuthToken authToken, List<Guid> P)
        {
            this.CheckAuthTokenOrThrowException(authToken);

            return OpenDnDContext.Players.Where(x => P.Contains(x.PlayerId))
                .Select(x => new Interfaces.Player(x.PlayerId, x.UserName)).ToList();
        }

        public Interfaces.Player GetPlayerByName(AuthToken authToken, string name)
        {
            this.CheckAuthTokenOrThrowException(authToken);

            var player = OpenDnDContext.Players.FirstOrDefault(x => x.UserName == name);

            if (player is null)
            {
                throw new Exception("Player not found");
            }

            return new Interfaces.Player(player.PlayerId, player.UserName);
        }
    }
}
