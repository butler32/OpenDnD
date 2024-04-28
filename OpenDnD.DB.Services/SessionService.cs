﻿using Microsoft.EntityFrameworkCore;
using OpenDnD.Interfaces;

namespace OpenDnD.DB.Services
{
    public class SessionService : ISessionService
    {
        public OpenDnDContext OpenDnDContext { get; }
        public IAuthService AuthService { get; }

        public event ISessionService.CurrentSessionMapChanged OnCurrentSessionMapChanged;
        public event ISessionService.SessionChatMessageChanged OnSessionChatMessageChanged;

        public SessionService(OpenDnDContext openDnDContext, IAuthService authService)
        {
            OpenDnDContext = openDnDContext;
            AuthService = authService;
        }

        public void AddPlayerToSession(AuthToken authToken, Guid sessionId, Guid userId, string playerRole)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);

            if (!OpenDnDContext.SessionPlayers.Any(x => x.PlayerId == authToken.PlayerId && x.SessionId == sessionId && x.PlayerRole == "OWNER"))
                throw new Exception("No acces to this action");

            OpenDnDContext.SessionPlayers.Add(new SessionPlayer
            {
                SessionId = sessionId,
                PlayerId = userId,
                PlayerRole = playerRole
            });
            OpenDnDContext.SaveChanges();
        }

        public void ChangeCurrentSessionMap(AuthToken authToken, Guid sessionId, Guid sessionMapId)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);

            if (!OpenDnDContext.SessionPlayers.Any(x => x.PlayerId == authToken.PlayerId && x.SessionId == sessionId && x.PlayerRole == "OWNER"))
                throw new Exception("No acces to this action");

            throw new NotImplementedException();
        }

        public Guid Create(AuthToken authToken, SessionRequest request)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);

            ArgumentNullException.ThrowIfNull(request.SessionName, nameof(SessionRequest.SessionName));
            var session = new Session
            {
                SessionName = request.SessionName,
                Players = request.PlayersIds?
                .Select(p => new SessionPlayer
                {
                    PlayerId = p,
                    PlayerRole = "Player"
                }).ToList()
            };
            OpenDnDContext.Sessions.Add(session);
            OpenDnDContext.SaveChanges();

            var owner = session.Players?.FirstOrDefault(x => x.PlayerId == authToken.PlayerId);

            if (owner == null)
            {
                owner = new SessionPlayer
                {
                    PlayerId = authToken.PlayerId,
                    SessionId = session.SessionId,
                };
                OpenDnDContext.SessionPlayers.Add(owner);
            }
            owner.PlayerRole = "OWNER";
            OpenDnDContext.SaveChanges();

            return session.SessionId;
        }

        public void Delete(AuthToken authToken, Guid id)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);

            if (!OpenDnDContext.SessionPlayers.Any(x => x.PlayerId == authToken.PlayerId && x.SessionId == id && x.PlayerRole == "OWNER"))
                throw new Exception("No acces to this action");

            OpenDnDContext.SessionPlayers
                .Where(x => x.SessionId == id)
                .ExecuteDelete();

            OpenDnDContext.Sessions
                .Where(x => x.SessionId == id)
                .ExecuteDelete();
        }

        public Interfaces.Session Get(AuthToken authToken, Guid id)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);

            if (!OpenDnDContext.SessionPlayers.Any(x => x.PlayerId == authToken.PlayerId && x.SessionId == id))
                throw new Exception("No acces to this action");

            var session = OpenDnDContext.Sessions.FirstOrDefault(x => x.SessionId == id);
            return new Interfaces.Session
            {
                SessionId = session.SessionId,
                SessionName = session.SessionName
            };
        }

        public List<Interfaces.Session> GetList(AuthToken authToken)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);

            return OpenDnDContext.Sessions
                .Where(x => x.Players.Any(y => y.PlayerId == authToken.PlayerId))
                .Select(x => new Interfaces.Session
                {
                    SessionId = x.SessionId,
                    SessionName = x.SessionName
                })
                .ToList();
        }

        public void RemovePlayerFromSession(AuthToken authToken, Guid sessionId, Guid userId)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);

            OpenDnDContext.SessionPlayers.Where(x => x.SessionId == sessionId && x.PlayerId == userId).ExecuteDelete();
        }

        public void Update(AuthToken authToken, Guid id, SessionRequest request)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);

            var session = OpenDnDContext.Sessions.FirstOrDefault(x => x.SessionId == id);
            if (request.SessionName is not null)
            {
                session.SessionName = request.SessionName;
            }

            OpenDnDContext.SaveChanges();
        }

        public List<SessionPlayer> GetSessionPlayers(AuthToken authToken, Guid sessionId)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);

            return OpenDnDContext.SessionPlayers
                .Where(x => x.SessionId == sessionId)
                .ToList();
        }

        public bool CheckPlayerHasMinimusAccess(AuthToken authToken, Guid sessionId, Guid playerId, string role)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);

            IQueryable<SessionPlayer> players = OpenDnDContext.SessionPlayers;
            
            players = role switch
            {
                "MASTER" => players.Where(x => x.PlayerRole == "MASTER"),
                "PLAYER" => players.Where(x => x.PlayerRole == "MASTER" || x.PlayerRole == "PLAYER"),
                "SPECTATOR" => players.Where(x => x.PlayerRole == "MASTER" || x.PlayerRole == "PLAYER" || x.PlayerRole == "SPECTATOR"),
                _ => throw new Exception("Invalid role name")
            };

            return players.Any(x => x.SessionId == sessionId && x.PlayerId == playerId);
        }
    }
}
