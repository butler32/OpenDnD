using Microsoft.Extensions.DependencyInjection;
using OpenDnD.Interfaces;
using System.Windows;
using System.Windows.Input;

namespace OpenDnD.Windows
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow(IServiceProvider serviceProvider, IAuthService authService)
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Top = this.Top;
            loginWindow.Left = this.Left;
            loginWindow.Width = this.Width;
            loginWindow.Height = this.Height;
            loginWindow.Show();
            this.Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            InfoLabel.Visibility = Visibility.Hidden;
            try
            {
                var authToken = AuthService.Register(new Uri("http://localhost:222"), loginEnter.Text, passwordEnter.Text);
                var ssw = ServiceProvider.GetRequiredService<SessionSelectionWindow>();
                this.Close();

                ssw.SetAuthToken(authToken);
                ssw.Show();
                ssw.Begin();
            }
            catch (Exception ex)
            {
                InfoLabel.Content = ex.Message;
                InfoLabel.Visibility = Visibility.Visible;
                return;
            }
        }
    }
}
