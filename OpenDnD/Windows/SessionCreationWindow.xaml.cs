using Microsoft.Extensions.DependencyInjection;
using OpenDnD.Interfaces;
using System.Data.SqlTypes;
using System.Windows;

namespace OpenDnD.Windows
{
    /// <summary>
    /// Interaction logic for SessionCreationWindow.xaml
    /// </summary>
    public partial class SessionCreationWindow : Window
    {
        public ISessionService SessionService { get; }
        public IPlayerService PlayerService { get; }
        public IServiceProvider ServiceProvider { get; }
        public AuthToken AuthToken { get; private set; }
        public Guid? SessionId { get; private set; }
        public Interfaces.Session CurrentSession { get; private set; }
        public List<Interfaces.SessionPlayer> Players { get; private set; }

        public SessionCreationWindow(ISessionService sessionService, IServiceProvider serviceProvider, IPlayerService playerService)
        {
            SessionService = sessionService;
            PlayerService = playerService;
            ServiceProvider = serviceProvider;
            InitializeComponent();
        }

        public void SetAuthToken(AuthToken authToken)
        {
            AuthToken = authToken;
        }

        private void RefreshPlayerList()
        {
            Players = SessionService.GetSessionPlayers(AuthToken, SessionId.Value);

            PlayersList.ItemsSource = Players;
        }

        public void SetSessionId(Guid sessionId)
        {
            SessionId = sessionId;
            CurrentSession = SessionService.Get(AuthToken, SessionId.Value);
            SessionName.Text = CurrentSession.SessionName;
            RefreshPlayerList();
        }

        private void InvitePlayerButton_Click(object sender, RoutedEventArgs e)
        {
            var siw = ServiceProvider.GetRequiredService<SessionInviterWindow>();
            siw.SetAuthToken(AuthToken);
            if (SessionId is null)
            {
                SaveSessionButton_Click(sender, e);
            }

            siw.SetSessionId(SessionId.Value);
            siw.ShowDialog();
            RefreshPlayerList();
        }

        private void SaveSessionButton_Click(object sender, RoutedEventArgs e)
        {
            if (SessionId is null)
            {
                SessionId = SessionService.Create(AuthToken, new SessionRequest
                {
                    SessionName = SessionName.Text,
                });
                CurrentSession = SessionService.Get(AuthToken, SessionId.Value);

                RefreshPlayerList();
            }
            else
            {
                List<Guid> guids = new List<Guid>();

                if (Players is not null)
                {
                    foreach (var player in Players)
                    {
                        guids.Add(player.PlayerId);
                    }
                }
                else
                    guids = null;

                SessionService.Update(AuthToken, CurrentSession.SessionId, new SessionRequest
                {
                    SessionName = SessionName.Text,
                    PlayersIds = guids
                });
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
