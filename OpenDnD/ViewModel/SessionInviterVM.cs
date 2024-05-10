using OpenDnD.Interfaces;
using OpenDnD.Model;
using OpenDnD.Utilities;

namespace OpenDnD.ViewModel
{
    class SessionInviterVM : ViewModelBase
    {
        public SessionInviterVM(IServiceProvider serviceProvider, ISessionService sessionService, IPlayerService playerService, UserAuthToken userAuthToken)
        {
            ServiceProvider = serviceProvider;
            SessionService = sessionService;
            PlayerService = playerService;
            UserAuthToken = userAuthToken;
        }

        private SessionModel _currentSession;
        public SessionModel CurrentSession
        {
            get { return _currentSession; }
            set { _currentSession = value; OnPropertyChanged(); }
        }

        public IServiceProvider ServiceProvider { get; }
        public ISessionService SessionService { get; }
        public IPlayerService PlayerService { get; }
        public UserAuthToken UserAuthToken { get; }

        public void SetCurrentSession(Guid sessionId)
        {
            CurrentSession = Converters.SessionConverter(SessionService.Get(UserAuthToken.AuthToken, sessionId));
        }
    }
}
