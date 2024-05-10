using Microsoft.EntityFrameworkCore;
using OpenDnD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDnD.DB.Services
{
    public class SessionChatService : ISessionChatService
    {
        public SessionChatService(OpenDnDContext openDnDContext, IAuthService authService, ISessionService sessionService)
        {
            OpenDnDContext = openDnDContext;
            AuthService = authService;
            SessionService = sessionService;
        }

        public OpenDnDContext OpenDnDContext { get; }
        public IAuthService AuthService { get; }
        public ISessionService SessionService { get; }

        public event ISessionChatService.SessionChatMessageChanged? OnSessionChatMessageChanged;
        public event ISessionChatService.SessionChatMessageDeleted? OnSessionChatMessageDeleted;
        public event ISessionChatService.SessionChatMessageCreated? OnSessionChatMessageCreated;

        private string RenderSpecialString(string str)
        {
            //For future transform commands like
            // roll 1d20 -> [{result}]
            // now empty
            return str;
        } 

        public Guid CreateChatMessage(AuthToken authToken, Guid sessionId, string message)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);
            SessionService.CheckPlayerHasMinimusAccessOrThrowException(authToken, sessionId, authToken.PlayerId, RoleEnum.Player);

            var renderedMessage = RenderSpecialString(message);
            var sessionChatMessage = new SessionChatMessage
            {
                DateTime = DateTime.Now,
                Message = renderedMessage,
                PlayerId = authToken.PlayerId,
                SessionId = sessionId,
            };

            OpenDnDContext.SessionChatMessages.Add(sessionChatMessage);
            OpenDnDContext.SaveChanges();

            OnSessionChatMessageCreated?.Invoke(new Interfaces.SessionChatMessage(message, authToken.PlayerId, sessionChatMessage.DateTime, sessionChatMessage.SessionChatMessageId));

            return sessionChatMessage.SessionChatMessageId;
        }

        public void DeleteChatMessage(AuthToken authToken, Guid messageId)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);
            var message = OpenDnDContext.SessionChatMessages.FirstOrDefault(x => x.SessionChatMessageId == messageId);
            if (message is null)
                throw new NoEntryWithRequiredIdException<SessionChatMessage>(messageId);

            if (message.PlayerId == authToken.PlayerId || SessionService.CheckPlayerHasMinimusAccess(authToken, message.SessionId, authToken.PlayerId, RoleEnum.Master)) {
                OpenDnDContext.SessionChatMessages.Where(x => x.SessionChatMessageId == messageId).ExecuteDelete();
                OnSessionChatMessageDeleted?.Invoke(messageId);
            }
            else
                throw new NoAccessToActionException();
        }

        public List<Interfaces.SessionChatMessage> GetChatMessageList(AuthToken authToken, Guid sessionId)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);
            SessionService.CheckPlayerHasMinimusAccessOrThrowException(authToken, sessionId, authToken.PlayerId, RoleEnum.Spectator);
            return OpenDnDContext.SessionChatMessages.Select(x => new Interfaces.SessionChatMessage(x.Message, x.PlayerId, x.DateTime, x.SessionChatMessageId)).ToList();
        }

        public void UpdateChatMessage(AuthToken authToken, Guid messageId, string message)
        {
            AuthService.CheckAuthTokenOrThrowException(authToken);
            var sessionChatMessage = OpenDnDContext.SessionChatMessages.FirstOrDefault(x => x.SessionChatMessageId == messageId);
            if (sessionChatMessage is null)
                throw new NoEntryWithRequiredIdException<SessionChatMessage>(messageId);
            if (sessionChatMessage.PlayerId == authToken.PlayerId)
            {
                var renderedMessage = RenderSpecialString(message);
                sessionChatMessage.Message = renderedMessage;
                OpenDnDContext.SaveChanges();
                OnSessionChatMessageChanged?.Invoke(new Interfaces.SessionChatMessage(sessionChatMessage.Message, sessionChatMessage.PlayerId, sessionChatMessage.DateTime, sessionChatMessage.SessionChatMessageId));
            }
            else
                throw new NoAccessToActionException();

        }
    }
}
