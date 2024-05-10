using OpenDnD.DB;
using System.Runtime.Serialization;

namespace OpenDnD.Interfaces
{

    public class SessionRequest 
    {
        public string? SessionName { get; set; }
        public List<Guid>? PlayersIds {  get; set; }
    }

    public class Session
    {
        public Guid SessionId { get; set; }
        public string SessionName { get; set; }
        public Session(Guid sessionId, string sessionName)
        {
            SessionId = sessionId;
            SessionName = sessionName;
        }
    }
    public interface ISessionService : ICRUDService<Session, SessionRequest>
    {
        public bool CheckPlayerHasMinimusAccess(AuthToken authToken, Guid sessionId, Guid playerId, RoleEnum role);
        public void AddPlayerToSession(AuthToken authToken, Guid sessionId, Guid userId, RoleEnum playerRole);
        public void RemovePlayerFromSession(AuthToken authToken, Guid sessionId, Guid userId);

        delegate void CurrentSessionMapChanged(SessionMapEntity sessionMapEntity);
        event CurrentSessionMapChanged? OnCurrentSessionMapChanged;

        delegate void SessionChatMessageChanged(SessionChatMessage sessionChatMessage);
        event SessionChatMessageChanged? OnSessionChatMessageChanged;

        public void ChangeCurrentSessionMap(AuthToken authToken, Guid sessionId, Guid sessionMapId);
        public List<SessionPlayer> GetSessionPlayers(AuthToken authToken, Guid sessionId);
        public List<Session> GetSessionsByIds(AuthToken authToken, List<Guid> sessionId);

    }

    public static class ISessionServiceExt
    {
        public static void CheckPlayerHasMinimusAccessOrThrowException(this ISessionService sessionService, AuthToken authToken, Guid sessionId, Guid playerId, RoleEnum role)
        {
            if (!sessionService.CheckPlayerHasMinimusAccess(authToken, sessionId, playerId, role))
                throw new NoAccessToActionException();
        }
    }

    

}
