using Microsoft.Extensions.DependencyInjection;
using OpenDnD.Interfaces;
using System.Net;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Input;

namespace OpenDnD.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow(IServiceProvider serviceProvider, IAuthService authService)
        {
            InitializeComponent();
            ServiceProvider = serviceProvider;
            AuthService = authService;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private bool isMaximized = false;

        public IServiceProvider ServiceProvider { get; }
        public IAuthService AuthService { get; }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (isMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1280;
                    this.Height = 720;

                    isMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    isMaximized = true;
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = ServiceProvider.GetRequiredService<RegisterWindow>();
            registerWindow.Left = this.Left;
            registerWindow.Top = this.Top;
            registerWindow.Width = this.Width;
            registerWindow.Height = this.Height;
            registerWindow.Show();

            this.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            InfoLabel.Visibility = Visibility.Hidden;
            AuthToken authToken;
            try
            {
                authToken = AuthService.Authenticate(new Uri("http://localhost:222"), loginEnter.Text, passwordEnter.Password);
            }
            catch (Exception ex)
            {
                InfoLabel.Content = ex.Message;
                InfoLabel.Visibility = Visibility.Visible;
                return;
            }
            var ssw = ServiceProvider.GetRequiredService<SessionSelectionWindow>();
            this.Close();

            ssw.SetAuthToken(authToken);
            ssw.Show();
            ssw.Begin();
        }
    }
}
