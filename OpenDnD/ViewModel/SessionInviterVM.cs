using OpenDnD.Interfaces;
using OpenDnD.Model;
using OpenDnD.Utilities;
using OpenDnD.Utilities.DI;

namespace OpenDnD.ViewModel
{
    class SessionInviterVM : ViewModelBase
    {
        public SessionInviterVM(IServiceProvider serviceProvider)
        {
            serviceProvider.UseDI(this);
        }

        private SessionModel _currentSession;
        public SessionModel CurrentSession
        {
            get { return _currentSession; }
            set { _currentSession = value; OnPropertyChanged(); }
        }

        [FromDI]
        public IServiceProvider ServiceProvider { get; private set; }
        [FromDI]
        public ISessionService SessionService { get; private set; }
        [FromDI]
        public IPlayerService PlayerService { get; private set; }
        [FromDI]
        public UserAuthToken UserAuthToken { get; private set; }
    }
}
