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
            OpenDnDContext.Players.Where(x => x.PlayerId == id).ExecuteDelete();
        }

        public Interfaces.Player Get(AuthToken authToken, Guid id)
        {
            var player = OpenDnDContext.Players.FirstOrDefault(x => x.PlayerId == id);
            return new Interfaces.Player
            {
                PlayerId = player.PlayerId,
                UserName = player.UserName
            };
        }

        public List<Interfaces.Player> GetList(AuthToken authToken)
        {
            throw new NotImplementedException();
        }

        public void Update(AuthToken authToken, Guid id, PlayerRequest request)
        {
            throw new NotImplementedException();
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
    }
}
