
namespace OpenDnD.Interfaces
{
    public class SessionChatMessage
    {
        public string Message { get; set; }
        public Guid PlayerId { get; set; }
        public DateTime DateTime { get; set; }

        public SessionChatMessage(string message, Guid playerId, DateTime dateTime)
        {
            Message = message;
            PlayerId = playerId;
            DateTime = dateTime;
        }
    }
    public class SessionChatMessageRequest
    {
        public Guid SessionId { get; set; }
        public string Message { get; set; }
    }
    public interface ISessionChatService : ICRUDService<SessionChatMessage, SessionChatMessageRequest>
    {

    }

}
