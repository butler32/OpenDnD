using Microsoft.Extensions.DependencyInjection;
using OpenDnD.Interfaces;
using OpenDnD.Model;
using OpenDnD.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OpenDnD.ViewModel
{
    class SessionsVM : ViewModelBase
    {
        public IServiceProvider ServiceProvider { get; }
        public ISessionService SessionService { get; }
        public UserAuthToken UserAuthToken { get; }
        public IPlayerService PlayerService { get; }

        private ObservableCollection<SessionModel> _sessions;
        public ObservableCollection<SessionModel> Sessions
        {
            get { return _sessions; }
            set { _sessions = value; OnPropertyChanged(); }
        }

        private ObservableCollection<SessionModel> _curentSessions;
        public ObservableCollection<SessionModel> CurentSessions
        {
            get { return _curentSessions; }
            set { _curentSessions = value; OnPropertyChanged(); }
        }

        private string _searchBox;
        public string SearchBox 
        { 
            get { return _searchBox; }
            set 
            { 
                _searchBox = value;

                CurentSessionsRefresh();
                
                if (string.IsNullOrEmpty(SearchBox))
                {
                    CurentSessions = Sessions;
                }
                else
                {
                    CurentSessions.Remove(null);
                    var a = CurentSessions.Where(x => x.SessionName.Contains(SearchBox)).ToList();
                    CurentSessions = Converters.ConvertToObservableCollectionSession(a);
                }

                OnPropertyChanged(); 
            }
        }

        public ICommand CreateSessionCommand { get; }
        public ICommand EditSessionCommand { get; }
        public ICommand ExitSessionCommand { get; }
        public ICommand JoinSessionCommand { get; }

        public Action<object?> SessionCreationRequested;

        private void CreateSession(object obj)
        {
            SessionCreationRequested?.Invoke(null);
        }

        private void EditSession(object obj)
        {
            SessionCreationRequested?.Invoke(obj);
        }

        private void ExitSession(object obj)
        {
            if (obj is SessionModel session)
            {
                SessionService.Delete(UserAuthToken.AuthToken, session.SessionId);
            }
            SearchBox = string.Empty;
            GetSessions();
        }


        private void JoinSession(object obj)
        {

        }

        private void GetSessions()
        {
            Sessions = new ObservableCollection<SessionModel>
            {
                null
            };
            foreach (var session in Converters.ConvertToObservableCollectionSession(SessionService.GetList(UserAuthToken.AuthToken)))
            {
                Sessions.Add(session);
            }

            CurentSessionsRefresh();
        }

        private void CurentSessionsRefresh()
        {
            CurentSessions = new ObservableCollection<SessionModel>(Sessions);
        }







        public SessionsVM(IServiceProvider serviceProvider, ISessionService sessionService, UserAuthToken userAuthToken, IPlayerService playerService)
        {
            ServiceProvider = serviceProvider;
            SessionService = sessionService;
            UserAuthToken = userAuthToken;
            PlayerService = playerService;

            CreateSessionCommand = new RelayCommand(CreateSession);
            EditSessionCommand = new RelayCommand(EditSession);
            ExitSessionCommand = new RelayCommand(ExitSession);
            JoinSessionCommand = new RelayCommand(JoinSession);

            GetSessions();
        }
    }
}
