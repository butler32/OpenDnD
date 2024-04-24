namespace OpenDnD.Interfaces
{
    public class SessionRequest
    {

    }

    public class Session
    {

    }
    public interface ISessionService : ICRUDService<Session, SessionMapRequest>
    {
        public void AddPlayerToSession(AuthToken authToken, Guid sessionId, Guid userId);
        public void RemovePlayerFromSession(AuthToken authToken, Guid sessionId, Guid userId);

        delegate void CurrentSessionMapChanged(SessionMapEntity sessionMapEntity);
        event CurrentSessionMapChanged OnCurrentSessionMapChanged;

        delegate void SessionChatMessageChanged(SessionChatMessage sessionChatMessage);
        event SessionChatMessageChanged OnSessionChatMessageChanged;

        public void ChangeCurrentSessionMap(AuthToken authToken, Guid sessionId, Guid sessionMapId);

    }

}
