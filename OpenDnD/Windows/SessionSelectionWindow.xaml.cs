using Microsoft.Extensions.DependencyInjection;
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
    /// Interaction logic for SessionSelectionWindow.xaml
    /// </summary>
    public partial class SessionSelectionWindow : Window
    {
        public ISessionService SessionService { get; }
        public IAuthService AuthService { get; }
        public IServiceProvider ServiceProvider { get; }
        public AuthToken AuthToken { get; private set; }

        public List<Session> Sessions { get; private set; }
        public SessionSelectionWindow(ISessionService sessionService, IAuthService authService, IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            SessionService = sessionService;
            AuthService = authService;
            InitializeComponent();
        }

        public void SetAuthToken(AuthToken authToken)
        {
            AuthToken = authToken;
        }

        public void Begin()
        {
            Sessions = SessionService.GetSessionList(AuthToken);
            
            SessionsList.ItemsSource = Sessions;
        }

        private void DeleteSessionButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedSession = SessionsList.SelectedItem as Session;

            if (selectedSession != null)
            {
                SessionService.DeleteSession(AuthToken, selectedSession.SessionId);
                Begin();
            }
            else
            {
                MessageBox.Show("No session is selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateSessionButton_Click(object sender, RoutedEventArgs e)
        {
            var sessionId = SessionService.CreateSession(AuthToken, new SessionRequest
            {
                SessionName = "New Session",
                SessionId = new Guid(),
            });

            var scw = ServiceProvider.GetRequiredService<SessionCreationWindow>();
            scw.SetAuthToken(AuthToken);
            scw.SetSession(sessionId);
            scw.ShowDialog();
            Begin();
        }
    }
}
