using Microsoft.EntityFrameworkCore.Query.Internal;
using OpenDnD.DB;
using OpenDnD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OpenDnD.Windows
{
    /// <summary>
    /// Interaction logic for SessionCreationWindow.xaml
    /// </summary>
    public partial class SessionCreationWindow : Window
    {
        public ISessionService SessionService { get; }
        public IAuthService AuthService { get; }
        public IServiceProvider ServiceProvider { get; }
        public AuthToken AuthToken { get; private set; }
        public Guid? SessionId { get; private set; }
        public Interfaces.Session CurrentSession { get; private set; }
        public List<SessionPlayer> Players { get; private set; }

        public SessionCreationWindow(ISessionService sessionService, IAuthService authService, IServiceProvider serviceProvider)
        {
            SessionService = sessionService;
            AuthService = authService;
            ServiceProvider = serviceProvider;
            InitializeComponent();
        }

        public void SetAuthToken(AuthToken authToken)
        {
            AuthToken = authToken;
        }

        public void SetSessionId(Guid sessionId)
        {
            SessionId = sessionId;
            CurrentSession = SessionService.Get(AuthToken, SessionId.Value);
            Players = SessionService.GetSessionPlayers(AuthToken, SessionId.Value);
            PlayersList.ItemsSource = Players;
        }

        //protected override void OnClosed(EventArgs e)
        //{
        //    SessionService.Update(AuthToken, CurrentSession.SessionId, new SessionRequest
        //    {
        //        SessionId = CurrentSession.SessionId,
        //        SessionName = SessionName.Text,
        //    });

        //    base.OnClosed(e);
        //}

        private void InvitePlayerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveSessionButton_Click(object sender, RoutedEventArgs e)
        {
            if (SessionId == null) 
            {
                SessionId = SessionService.Create(AuthToken, new SessionRequest
                {
                    SessionName = SessionName.Text,
                });
            }
            else
            {
                SessionService.Update(AuthToken, CurrentSession.SessionId, new SessionRequest
                {
                    SessionName = SessionName.Text,
                });
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
