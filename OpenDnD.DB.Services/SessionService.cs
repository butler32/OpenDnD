using Microsoft.EntityFrameworkCore;
using OpenDnD.Interfaces;

namespace OpenDnD.DB.Services
{

    public class SessionService : ISessionService
    {
        public OpenDnDContext OpenDnDContext { get; }

        public event ISessionService.CurrentSessionMapChanged OnCurrentSessionMapChanged;
        public event ISessionService.SessionChatMessageChanged OnSessionChatMessageChanged;

        public SessionService(OpenDnDContext openDnDContext)
        {
            OpenDnDContext = openDnDContext;
        }

        public void AddPlayerToSession(AuthToken authToken, Guid sessionId, Guid userId, string playerRole)
        {
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
            throw new NotImplementedException();
        }

        public Guid Create(AuthToken authToken, SessionRequest request)
        {
            ArgumentNullException.ThrowIfNull(request.SessionName, nameof(SessionRequest.SessionName));
            var session = new Session
            {
                SessionName = request.SessionName,
                Players = request.PlayersIds?.Select(p => new SessionPlayer
                {
                    PlayerId = p,
                    PlayerRole = "Player"
                }).ToList()
            };
            OpenDnDContext.Sessions.Add(session);
            OpenDnDContext.SaveChanges();

            return session.SessionId;
        }

        public void Delete(AuthToken authToken, Guid id)
        {
            OpenDnDContext.Sessions.Where(x => x.SessionId == id).ExecuteDelete();
        }

        public Interfaces.Session Get(AuthToken authToken, Guid id)
        {
            var session = OpenDnDContext.Sessions.FirstOrDefault(x => x.SessionId == id);
            return new Interfaces.Session
            {
                SessionId = session.SessionId,
                SessionName = session.SessionName
            };
        }

        public List<Interfaces.Session> GetList(AuthToken authToken)
        {
            return OpenDnDContext.Sessions
                .Select(x => new Interfaces.Session
                {
                    SessionId = x.SessionId,
                    SessionName = x.SessionName
                })
                .ToList();
        }

        public void RemovePlayerFromSession(AuthToken authToken, Guid sessionId, Guid userId)
        {
            OpenDnDContext.SessionPlayers.Where(x => x.SessionId == sessionId && x.PlayerId == userId).ExecuteDelete();
        }

        public void Update(AuthToken authToken, Guid id, SessionRequest request)
        {
            var session = OpenDnDContext.Sessions.FirstOrDefault(x => x.SessionId == id);
            if (request.SessionName is not null)
            {
                session.SessionName = request.SessionName;
            }
            
            OpenDnDContext.SaveChanges();
        }

        public List<SessionPlayer> GetSessionPlayers(AuthToken authToken, Guid sessionId)
        {
            return OpenDnDContext.SessionPlayers
                .Where(x => x.SessionId == sessionId)
                .ToList();
        }
    }
}
