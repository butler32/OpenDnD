using OpenDnD.DB.Services;
using OpenDnD.Interfaces;
using System.Windows;

namespace OpenDnD.Windows
{
    /// <summary>
    /// Interaction logic for SessionInviterWindow.xaml
    /// </summary>
    public partial class SessionInviterWindow : Window
    {
        public SessionInviterWindow(ISessionService sessionService, IPlayerService playerService)
        {
            InitializeComponent();
            SessionService = sessionService;
            PlayerService = playerService;
        }

        public AuthToken AuthToken { get; private set; }
        public Guid SessionId { get; private set; }
        public ISessionService SessionService { get; }
        public IPlayerService PlayerService { get; }

        public void SetAuthToken(AuthToken authToken)
        {
            AuthToken = authToken;
        }

        public void SetSessionId(Guid sessionId)
        {
            SessionId = sessionId;
        }

        private void InviteButton_Click(object sender, RoutedEventArgs e)
        {
            Interfaces.Player player;
            try
            {
                player = PlayerService.GetPlayerByName(AuthToken, UserName.Text);
            }
            catch (Exception w) 
            {
                MessageBox.Show(w.Message);
                return;
            }

            SessionService.AddPlayerToSession(AuthToken, SessionId, player.PlayerId, RoleSelector.SelectedItem == "Игрок" ? RoleEnum.Player : RoleEnum.Spectator);
            Close();
        }
    }
}
