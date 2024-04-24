namespace OpenDnD.Interfaces
{
    public class SessionRequest
    {

    }

    public class Session
    {
        public Guid SessionId { get; set; }
        public string SessionName { get; set; }
    }
    public interface ISessionService : ICRUDService<Session, SessionMapRequest>
    {
        public void AddPlayerToSession(AuthToken authToken, Guid sessionId, Guid userId, string playerRole);
        public void RemovePlayerFromSession(AuthToken authToken, Guid sessionId, Guid userId);

        delegate void CurrentSessionMapChanged(SessionMapEntity sessionMapEntity);
        event CurrentSessionMapChanged OnCurrentSessionMapChanged;

        delegate void SessionChatMessageChanged(SessionChatMessage sessionChatMessage);
        event SessionChatMessageChanged OnSessionChatMessageChanged;

        public void ChangeCurrentSessionMap(AuthToken authToken, Guid sessionId, Guid sessionMapId);

    }

}
