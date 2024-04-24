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

        public Guid CreateSession(AuthToken authToken, SessionMapRequest request)
        {
            var session = new Session
            {
                SessionName = request.SessionName
            };
            OpenDnDContext.Sessions.Add(session);
            OpenDnDContext.SaveChanges();

            return session.SessionId;
        }

        public void DeleteSession(AuthToken authToken, Guid id)
        {
            OpenDnDContext.Sessions.Where(x => x.SessionId == id).ExecuteDelete();
        }

        public Interfaces.Session GetSession(AuthToken authToken, Guid id)
        {
            var session = OpenDnDContext.Sessions.FirstOrDefault(x => x.SessionId == id);
            return new Interfaces.Session
            {
                SessionId = session.SessionId,
                SessionName = session.SessionName
            };
        }

        public List<Interfaces.Session> GetSessionList(AuthToken authToken)
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

        public void UpdateSession(AuthToken authToken, Guid id, SessionMapRequest request)
        {
            var session = OpenDnDContext.Sessions.FirstOrDefault(x => x.SessionId == id);
            session.SessionName = request.SessionName;
            OpenDnDContext.SaveChanges();
        }
    }
}
