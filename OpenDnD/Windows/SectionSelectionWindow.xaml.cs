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
        }
        
    }
}
