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
    /// Interaction logic for SectionSelectionWindow.xaml
    /// </summary>
    public partial class SectionSelectionWindow : Window
    {
        public ISessionService SessionService { get; }
        public IAuthService AuthService { get; }
        public AuthToken AuthToken { get; private set; }

        public List<Session> Sessions { get; private set; }
        public SectionSelectionWindow(ISessionService sessionService, IAuthService authService)
        {
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
            Sessions.Add(new Session
            {
                SessionId = new Guid(),
                SessionName = "Session 1"
            });
            Sessions.Add(new Session
            {
                SessionId = new Guid(),
                SessionName = "Session 2"
            });
            Sessions.Add(new Session
            {
                SessionId = new Guid(),
                SessionName = "Session 3"
            });
            Sessions.Add(new Session
            {
                SessionId = new Guid(),
                SessionName = "Session 4"
            });

            SessionsList.ItemsSource = Sessions;
        }

        private void DeleteSessionButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedSession = SessionsList.SelectedItem as Session;

            if (selectedSession != null)
            {
                Sessions.Remove(selectedSession);
                SessionsList.Items.Refresh();
            }
            else
            {
                MessageBox.Show("No session is selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateSessionButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
