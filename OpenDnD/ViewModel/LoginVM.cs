using Microsoft.Extensions.DependencyInjection;
using OpenDnD.Interfaces;
using OpenDnD.Utilities;
using OpenDnD.Utilities.DI;

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

        [FromDI]
        public IServiceProvider ServiceProvider { get; private set; }
        [FromDI]
        public IAuthService AuthService { get; private set; }
        [FromDI]
        public UserAuthToken UserAuthToken { get; private set; }
        public ICommand LoginCommand { get; set; }
        public ICommand RegistrationCommand { get; set; }

        private void Login()
        {
            try
            {
                var token = AuthService.Authenticate(new Uri("http://localhost:222"), UserName, Password);

                this.UserAuthToken.AuthToken = token;

                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
                CloseWindow();
            }
            catch (Exception ex)
            {
                InfoLabel = ex.Message;
            }
        }

        private void Registration()
        {
            try
            {
                var token = AuthService.Register(new Uri("http://localhost:222"), UserName, Password);

                this.UserAuthToken.AuthToken = token;

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

        public LoginVM(IServiceProvider serviceProvider)
        {   
            serviceProvider.UseDI(this);

            LoginCommand = new RelayCommand(Login);
            RegistrationCommand = new RelayCommand(Registration);
        }
    }
}
