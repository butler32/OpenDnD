using Microsoft.Extensions.DependencyInjection;
using OpenDnD.Interfaces;
using System.Windows;

namespace OpenDnD.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindowOld : Window
    {
        public LoginWindowOld(IAuthService authService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            AuthService = authService;
            ServiceProvider = serviceProvider;
        }

        public IAuthService AuthService { get; }
        public IServiceProvider ServiceProvider { get; }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            var authToken = AuthService.Authenticate(new Uri(Address.Text), UserName.Text, Password.Text);
            var ssw = ServiceProvider.GetRequiredService<SessionSelectionWindow>();
            this.Close();

            ssw.SetAuthToken(authToken);
            ssw.Show();
            ssw.Begin();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var authToken = AuthService.Register(new Uri(Address.Text), UserName.Text, Password.Text);
                MessageBox.Show("Успешно");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NewWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var lrw = ServiceProvider.GetRequiredService<LoginWindow>();
            lrw.Top = (SystemParameters.PrimaryScreenHeight - lrw.Height) / 2;
            lrw.Left = (SystemParameters.PrimaryScreenWidth - lrw.Width) / 2;
            lrw.Show();
            this.Close();
        }
    }
}
