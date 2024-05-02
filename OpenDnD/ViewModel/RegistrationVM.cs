using OpenDnD.Interfaces;
using OpenDnD.Utilities;
using System.Windows.Input;

namespace OpenDnD.ViewModel
{
    public class RegistrationVM : ViewModelBase
    {
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        public IServiceProvider ServiceProvider { get; }
        public IAuthService AuthService { get; }

        public ICommand LoginCommand { get; set; }

        private async void Register(object obj)
        {
            var token = AuthService.Register(new Uri("http://localhost:222"), UserName, UserName);
        }

        public RegistrationVM(IServiceProvider serviceProvider, IAuthService authService)
        {
            ServiceProvider = serviceProvider;
            AuthService = authService;

            LoginCommand = new RelayCommand(Register);
        }
    }
}
