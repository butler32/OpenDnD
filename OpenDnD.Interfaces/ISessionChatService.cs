
namespace OpenDnD.Interfaces
{
    public class SessionChatMessage
    {
        public Guid SessionChatMessageId { get; set; }
        public string Message { get; set; }
        public Guid PlayerId { get; set; }
        public DateTime DateTime { get; set; }

        public SessionChatMessage(string message, Guid playerId, DateTime dateTime, Guid sessionChatMessageId)
        {
            Message = message;
            PlayerId = playerId;
            DateTime = dateTime;
            SessionChatMessageId = sessionChatMessageId;
        }
    }
    public interface ISessionChatService
    {
        delegate void SessionChatMessageChanged(SessionChatMessage sessionChatMessage);
        event SessionChatMessageChanged? OnSessionChatMessageChanged;

        delegate void SessionChatMessageDeleted(Guid messageId);
        event SessionChatMessageDeleted? OnSessionChatMessageDeleted;

        delegate void SessionChatMessageCreated(SessionChatMessage sessionChatMessage);
        event SessionChatMessageCreated? OnSessionChatMessageCreated;

        Guid CreateChatMessage(AuthToken authToken, Guid sessionId, string message);
        void DeleteChatMessage(AuthToken authToken, Guid messageId);
        void UpdateChatMessage(AuthToken authToken, Guid messageId, string message);
        List<SessionChatMessage> GetChatMessageList(AuthToken authToken, Guid sessionId);
    }

}
