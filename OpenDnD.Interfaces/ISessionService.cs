using OpenDnD.DB;
using System.Runtime.Serialization;

namespace OpenDnD.Interfaces
{

    public class SessionRequest 
    {
        public string? SessionName { get; set; }
        public List<Guid> PlayersIds {  get; set; }
    }

    public class Session
    {
        public Guid SessionId { get; set; }
        public string SessionName { get; set; }
    }
    public interface ISessionService : ICRUDService<Session, SessionRequest>
    {
        public bool CheckPlayerHasMinimusAccess(AuthToken authToken, Guid sessionId, Guid playerId, string role);
        public void AddPlayerToSession(AuthToken authToken, Guid sessionId, Guid userId, string playerRole);
        public void RemovePlayerFromSession(AuthToken authToken, Guid sessionId, Guid userId);

        delegate void CurrentSessionMapChanged(SessionMapEntity sessionMapEntity);
        event CurrentSessionMapChanged OnCurrentSessionMapChanged;

        delegate void SessionChatMessageChanged(SessionChatMessage sessionChatMessage);
        event SessionChatMessageChanged OnSessionChatMessageChanged;

        public void ChangeCurrentSessionMap(AuthToken authToken, Guid sessionId, Guid sessionMapId);
        public List<SessionPlayer> GetSessionPlayers(AuthToken authToken, Guid sessionId);

    }

    public static class ISessionServiceExt
    {
        public static void CheckPlayerHasMinimusAccessOrThrowException(this ISessionService sessionService, AuthToken authToken, Guid sessionId, Guid playerId, string role)
        {
            if (!sessionService.CheckPlayerHasMinimusAccess(authToken, sessionId, playerId, role))
                throw new NoAccessToActionException();
        }
    }

    public class NoAccessToActionException : Exception
    {
        public const string DefaultMessage = "No Access to this acction";
        public NoAccessToActionException() : base(DefaultMessage)
        {
        }

        public NoAccessToActionException(string? message) : base(message ?? DefaultMessage)
        {
        }

        public NoAccessToActionException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
        {
        }

        protected NoAccessToActionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
