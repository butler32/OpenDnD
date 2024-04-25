using OpenDnD.DB;

namespace OpenDnD.Interfaces
{
    public class SessionRequest
    {
        public Guid SessionId { get; set; }
        public string SessionName { get; set; }
        public List<Guid> PlayersIds {  get; set; }
    }

    public class Session
    {
        public Guid SessionId { get; set; }
        public string SessionName { get; set; }
    }
    public interface ISessionService : ICRUDService<Session, SessionRequest>
    {
        public void AddPlayerToSession(AuthToken authToken, Guid sessionId, Guid userId, string playerRole);
        public void RemovePlayerFromSession(AuthToken authToken, Guid sessionId, Guid userId);

        delegate void CurrentSessionMapChanged(SessionMapEntity sessionMapEntity);
        event CurrentSessionMapChanged OnCurrentSessionMapChanged;

        delegate void SessionChatMessageChanged(SessionChatMessage sessionChatMessage);
        event SessionChatMessageChanged OnSessionChatMessageChanged;

        public void ChangeCurrentSessionMap(AuthToken authToken, Guid sessionId, Guid sessionMapId);
        public List<SessionPlayer> GetSessionPlayers(AuthToken authToken, Guid sessionId);

    }

}
