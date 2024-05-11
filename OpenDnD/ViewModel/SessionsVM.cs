using Microsoft.Extensions.DependencyInjection;
using OpenDnD.DB.Services;
using OpenDnD.Interfaces;
using OpenDnD.Model;
using OpenDnD.Utilities;
using OpenDnD.Utilities.DI;
using System.Collections.ObjectModel;

namespace OpenDnD.ViewModel
{
    class SessionsVM : ViewModelBase
    {
        [FromDI]
        public IServiceProvider ServiceProvider { get; private set; }
        [FromDI]
        public ISessionService SessionService { get; private set; }
        [FromDI]
        public UserAuthToken UserAuthToken { get; private set; }
        [FromDI]
        public IPlayerService PlayerService { get; private set; }

        private List<SessionModel> _sessions;
        public List<SessionModel> Sessions
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

                CurentSessions.Clear();

                if (string.IsNullOrWhiteSpace(SearchBox))
                {
                    foreach (var s in Sessions)
                        CurentSessions.Add(s);
                }
                else
                {
                    foreach (var s in Sessions.Where(x => x.SessionName.Contains(SearchBox, StringComparison.InvariantCultureIgnoreCase)))
                        CurentSessions.Add(s);
                }

                CurentSessions.Insert(0, null);

                OnPropertyChanged(); 
            }
        }

        public ICommand CreateSessionCommand { get; set; }
        public ICommand<SessionModel> EditSessionCommand { get; set; }
        public ICommand<SessionModel> ExitSessionCommand { get; set; }
        public ICommand<object> JoinSessionCommand { get; }

        private void ExitSession(SessionModel session)
        {
            SessionService.Delete(UserAuthToken.AuthToken, session.SessionId);
            SearchBox = string.Empty;
            GetSessions();
        }


        private void JoinSession(object obj)
        {

        }

        private void GetSessions()
        {

            Sessions = SessionService.GetList(UserAuthToken.AuthToken)
                .Select(Converters.SessionConverter)
                .ToList();

            CurentSessions = new ObservableCollection<SessionModel>(Sessions);

            CurentSessions.Insert(0, null);
        }


        public SessionsVM(IServiceProvider serviceProvider)
        {
            serviceProvider.UseDI(this);

            ExitSessionCommand = ICommand.From<SessionModel>(ExitSession);
            JoinSessionCommand = ICommand.From<object>(JoinSession);

            GetSessions();
        }
    }
}
