using Microsoft.Extensions.DependencyInjection;
using OpenDnD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OpenDnD.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow(IAuthService authService, IServiceProvider serviceProvider)
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
    }
}
