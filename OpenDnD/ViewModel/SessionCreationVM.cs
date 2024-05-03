using OpenDnD.Interfaces;
using OpenDnD.Utilities;

namespace OpenDnD.ViewModel
{
    class SessionCreationVM : ViewModelBase
    {
        public SessionCreationVM(IServiceProvider serviceProvider, ISessionService sessionService, UserAuthToken userAuthToken)
        {
            ServiceProvider = serviceProvider;
            SessionService = sessionService;
            UserAuthToken = userAuthToken;

            SessionService.Create(UserAuthToken.AuthToken, new SessionRequest
            {
                SessionName = "new session",
                PlayersIds = new List<Guid>()
            });
        }

        public IServiceProvider ServiceProvider { get; }
        public ISessionService SessionService { get; }
        public UserAuthToken UserAuthToken { get; }
    }
}
