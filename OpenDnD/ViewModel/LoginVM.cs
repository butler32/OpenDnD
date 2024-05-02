using Microsoft.Extensions.DependencyInjection;
using OpenDnD.Interfaces;
using OpenDnD.Utilities;
using System.Windows.Input;

namespace OpenDnD.ViewModel
{
    public class LoginVM : ViewModelBase, ICloseWindow
    {
        private string _infoLabel;
        public string InfoLabel
        {
            get { return _infoLabel; }
            set { _infoLabel = value; OnPropertyChanged(); }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged();}
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged();}
        }

        public Action Close { get; set; }

        void CloseWindow()
        {
            Close?.Invoke();
        }

        public IServiceProvider ServiceProvider { get; }
        public IAuthService AuthService { get; }
        public ICommand LoginCommand { get; set; }
        public ICommand RegistrationCommand { get; set; }

        private void Login(object obj)
        {
            try
            {
                var token = AuthService.Authenticate(new Uri("http://localhost:222"), UserName, Password);

                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
                CloseWindow();
            }
            catch (Exception ex)
            {
                InfoLabel = ex.Message;
            }
        }

        private void Registration(object obj)
        {
            try
            {
                var token = AuthService.Register(new Uri("http://localhost:222"), UserName, Password);

                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
                CloseWindow();
            }
            catch (Exception ex)
            {
                InfoLabel = ex.Message;
            }
        }

        public bool CanClose()
        {
            return true;
        }

        public LoginVM(IServiceProvider serviceProvider, IAuthService authService)
        {
            ServiceProvider = serviceProvider;
            AuthService = authService;

            LoginCommand = new RelayCommand(Login);
        }
    }
}
