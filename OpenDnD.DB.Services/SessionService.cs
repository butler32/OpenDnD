using Microsoft.EntityFrameworkCore;
using OpenDnD.Interfaces;

namespace OpenDnD.DB.Services
{
    public class SessionService : ISessionService
    {
        public OpenDnDContext OpenDnDContext { get; }
        public IAuthService AuthService { get; }

        public event ISessionService.CurrentSessionMapChanged? OnCurrentSessionMapChanged;
        public event ISessionService.SessionChatMessageChanged? OnSessionChatMessageChanged;

        public SessionService(OpenDnDContext openDnDContext, IAuthService authService)
        {
            OpenDnDContext = openDnDContext;
            AuthService = authService;
        }

        public void AddPlayerToSession(AuthToken authToken, Guid sessionId, Guid userId, RoleEnum playerRole)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);
            this.CheckPlayerHasMinimusAccessOrThrowException(authToken, sessionId, authToken.PlayerId, RoleEnum.Master);

            OpenDnDContext.SessionPlayers.Add(new SessionPlayer
            {
                SessionId = sessionId,
                PlayerId = userId,
                PlayerRole = (int)playerRole
            });
            OpenDnDContext.SaveChanges();
        }

        public void ChangeCurrentSessionMap(AuthToken authToken, Guid sessionId, Guid sessionMapId)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);
            this.CheckPlayerHasMinimusAccessOrThrowException(authToken, sessionId, authToken.PlayerId, RoleEnum.Master);

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
                    PlayerRole = (int)RoleEnum.Player
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
            owner.PlayerRole = (int)RoleEnum.Master;
            OpenDnDContext.SaveChanges();

            return session.SessionId;
        }

        public void Delete(AuthToken authToken, Guid id)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);
            this.CheckPlayerHasMinimusAccessOrThrowException(authToken, id, authToken.PlayerId, RoleEnum.Master);

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

            var session = OpenDnDContext.Sessions.FirstOrDefault(x => x.SessionId == id);
            if (session is null)
                throw new NoEntryWithRequiredIdException<Session>(id);

            this.CheckPlayerHasMinimusAccessOrThrowException(authToken, id, authToken.PlayerId, RoleEnum.Spectator);
            
            return new Interfaces.Session(session.SessionId, session.SessionName);
        }

        public List<Interfaces.Session> GetList(AuthToken authToken)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);

            return OpenDnDContext.Sessions
                .Where(x => x.Players.Any(y => y.PlayerId == authToken.PlayerId))
                .Select(x => new Interfaces.Session(x.SessionId, x.SessionName))
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
            if (session is null)
                throw new NoEntryWithRequiredIdException<Session>(id);

            if (request.SessionName is not null)
            {
                session.SessionName = request.SessionName;
            }

            OpenDnDContext.SaveChanges();
        }

        public List<Interfaces.SessionPlayer> GetSessionPlayers(AuthToken authToken, Guid sessionId)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);

            return OpenDnDContext.SessionPlayers
                .Where(x => x.SessionId == sessionId)
                .Select(x => new Interfaces.SessionPlayer(x.PlayerId, x.Player.UserName, (RoleEnum)x.PlayerRole))
                .ToList();
        }

        public bool CheckPlayerHasMinimusAccess(AuthToken authToken, Guid sessionId, Guid playerId, RoleEnum role)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);

            IQueryable<SessionPlayer> players = OpenDnDContext.SessionPlayers;
            
            players = role switch
            {
                RoleEnum.Master => players.Where(x => x.PlayerRole == (int)RoleEnum.Master),
                RoleEnum.Player => players.Where(x => x.PlayerRole == (int)RoleEnum.Master || x.PlayerRole == (int)RoleEnum.Player),
                RoleEnum.Spectator => players.Where(x => x.PlayerRole == (int)RoleEnum.Master || x.PlayerRole == (int)RoleEnum.Player || x.PlayerRole == (int)RoleEnum.Spectator),
                _ => throw new Exception("Invalid role name")
            };

            return players.Any(x => x.SessionId == sessionId && x.PlayerId == playerId);
        }

        public List<Interfaces.Session> GetSessionsByIds(AuthToken authToken, List<Guid> sessionId)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);

            return OpenDnDContext.Sessions
                .Where(x => sessionId.Contains(x.SessionId))
                .Select(x => new Interfaces.Session(x.SessionId, x.SessionName))
                .ToList();
        }
    }
}
